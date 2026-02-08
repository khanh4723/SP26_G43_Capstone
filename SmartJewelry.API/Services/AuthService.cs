using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmartJewelry.API.Data;
using SmartJewelry.API.DTOs.Auth;
using SmartJewelry.API.Entities;
using SmartJewelry.API.Enums;
using SmartJewelry.API.Services.Interfaces;
using SmartJewelry.API.Settings;

namespace SmartJewelry.API.Services;

public class AuthService : IAuthService
{
    private readonly AiJgsmsFinalContext _context;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IEmailValidationService _emailValidationService;
    private readonly AppSettings _appSettings;
    private readonly ILogger<AuthService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthService(
        AiJgsmsFinalContext context,
        ITokenService tokenService,
        IEmailService emailService,
        IEmailValidationService emailValidationService,
        IOptions<AppSettings> appSettings,
        ILogger<AuthService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _tokenService = tokenService;
        _emailService = emailService;
        _emailValidationService = emailValidationService;
        _appSettings = appSettings.Value;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            // Validate email (check disposable/fake email)
            var (isValidEmail, emailErrorMessage) = await _emailValidationService.ValidateEmailAsync(registerDto.Email);
            if (!isValidEmail)
            {
                _logger.LogWarning("Invalid/disposable email rejected: {Email}, Reason: {Reason}", 
                    registerDto.Email, emailErrorMessage);
                return new AuthResponseDto
                {
                    Success = false,
                    Message = emailErrorMessage ?? "Email không hợp lệ"
                };
            }

            // Kiểm tra email đã tồn tại chưa
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Email này đã được sử dụng"
                };
            }

            // Kiểm tra username đã tồn tại chưa
            var emailPrefix = registerDto.Email.Split('@')[0];
            var existingUsername = await _context.Users.FirstOrDefaultAsync(u => u.Username == emailPrefix);
            var username = existingUsername != null ? $"{registerDto.Email.Split('@')[0]}_{Guid.NewGuid().ToString().Substring(0, 4)}" : registerDto.Email.Split('@')[0];

            // Check if auto-verify in dev mode
            bool isAutoVerify = _appSettings.IsDevMode && 
                               _appSettings.AutoVerifyEmails != null &&
                               _appSettings.AutoVerifyEmails.Contains(registerDto.Email, StringComparer.OrdinalIgnoreCase);

            if (isAutoVerify)
            {
                _logger.LogInformation("Auto-verifying email in dev mode: {Email}", registerDto.Email);
            }

            // Tạo user mới với role customer
            var user = new User
            {
                Username = username,
                Email = registerDto.Email,
                Role = UserRoles.Customer, // Lưu role trực tiếp
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                IsActive = true,
                EmailVerified = isAutoVerify, // Auto-verify trong dev mode
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Tạo Customer record
            var customer = new Customer
            {
                UserId = user.UserId,
                Phone = registerDto.PhoneNumber,
                LoyaltyPoints = 0,
                CustomerTier = "bronze"
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Tạo CustomerProfile trống
            var profile = new CustomerProfile
            {
                CustomerId = customer.CustomerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.CustomerProfiles.Add(profile);
            await _context.SaveChangesAsync();

            // Tạo email verification token
            var verificationToken = _tokenService.GenerateEmailVerificationToken();
            var emailToken = new EmailVerificationToken
            {
                UserId = user.UserId,
                Token = verificationToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24), // 24 hours
                CreatedAt = DateTime.UtcNow
            };
            _context.EmailVerificationTokens.Add(emailToken);
            await _context.SaveChangesAsync();

            // Tạo verification link
            var verificationLink = $"{_appSettings.FrontendUrl}/verify-email?token={verificationToken}&email={Uri.EscapeDataString(user.Email)}";

            // Tạo token
            var accessToken = _tokenService.GenerateAccessToken(user, new List<string> { user.Role });
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Lưu refresh token
            var newRefreshToken = new RefreshToken
            {
                UserId = user.UserId,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            // Gửi email verification (không block response)
            _ = Task.Run(async () => await _emailService.SendEmailVerificationAsync(user.Email, username, verificationLink, verificationToken));

            _logger.LogInformation("User registered successfully: {Email}", user.Email);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Đăng ký thành công",
                Token = new TokenDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(1)
                },
                User = new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    IsActive = user.IsActive ?? true,
                    CreatedAt = user.CreatedAt ?? DateTime.UtcNow
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email: {Email}", registerDto.Email);
            return new AuthResponseDto
            {
                Success = false,
                Message = "Có lỗi xảy ra trong quá trình đăng ký"
            };
        }
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Email hoặc mật khẩu không chính xác"
                };
            }

            // Kiểm tra tài khoản có active không
            if (!(user.IsActive ?? true))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Tài khoản đã bị khóa. Vui lòng liên hệ hỗ trợ"
                };
            }

            // Kiểm tra email đã được verify chưa (TẠM THỜI TẮT để test)
            // REMOVED: Email verification requirement
            // if (!user.EmailVerified && string.IsNullOrEmpty(user.SocialLoginProvider))
            // {
            //     _logger.LogWarning("Login attempt with unverified email: {Email}", user.Email);
            //     return new AuthResponseDto
            //     {
            //         Success = false,
            //         Message = "Email chưa được xác thực. Vui lòng kiểm tra email và xác thực tài khoản trước khi đăng nhập."
            //     };
            // }

            // Kiểm tra password (cho phép null nếu đăng nhập qua social)
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Tài khoản này sử dụng đăng nhập qua Google/Facebook"
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Email hoặc mật khẩu không chính xác"
                };
            }

            // Cập nhật LastLogin
            user.LastLogin = DateTime.UtcNow;

            // Tạo tokens
            var accessToken = _tokenService.GenerateAccessToken(user, new List<string> { user.Role });
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Lưu refresh token vào database
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.UserId,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
            _context.RefreshTokens.Add(refreshTokenEntity);

            await _context.SaveChangesAsync();

            _logger.LogInformation("User logged in successfully: {Email}", user.Email);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Đăng nhập thành công",
                Token = new TokenDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(1)
                },
                User = new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    IsActive = user.IsActive ?? true,
                    CreatedAt = user.CreatedAt ?? DateTime.UtcNow
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", loginDto.Email);
            return new AuthResponseDto
            {
                Success = false,
                Message = "Có lỗi xảy ra trong quá trình đăng nhập"
            };
        }
    }

    public async Task<AuthResponseDto> SocialLoginAsync(SocialLoginDto socialLoginDto)
    {
        try
        {
            // Validate và lấy thông tin user từ provider
            string? email = null;
            string? name = null;
            string? providerUserId = null;

            // Trường hợp 1: Đã có thông tin user từ Web (OAuth đã xác thực phía Web)
            if (!string.IsNullOrEmpty(socialLoginDto.Email))
            {
                email = socialLoginDto.Email;
                name = socialLoginDto.FullName;
                providerUserId = socialLoginDto.ProviderUserId ?? Guid.NewGuid().ToString();
                
                _logger.LogInformation("Social login with pre-authenticated user: {Email}, Provider: {Provider}", email, socialLoginDto.Provider);
            }
            // Trường hợp 2: Có access token, cần gọi API để lấy thông tin
            else if (!string.IsNullOrEmpty(socialLoginDto.AccessToken))
            {
                if (socialLoginDto.Provider.Equals("Google", StringComparison.OrdinalIgnoreCase))
                {
                    var googleUser = await GetGoogleUserInfoAsync(socialLoginDto.AccessToken);
                    if (googleUser == null)
                    {
                        return new AuthResponseDto { Success = false, Message = "Token Google không hợp lệ" };
                    }
                    email = googleUser.Email;
                    name = googleUser.Name;
                    providerUserId = googleUser.Id;
                }
                else if (socialLoginDto.Provider.Equals("Facebook", StringComparison.OrdinalIgnoreCase))
                {
                    var facebookUser = await GetFacebookUserInfoAsync(socialLoginDto.AccessToken);
                    if (facebookUser == null)
                    {
                        return new AuthResponseDto { Success = false, Message = "Token Facebook không hợp lệ" };
                    }
                    email = facebookUser.Email;
                    name = facebookUser.Name;
                    providerUserId = facebookUser.Id;
                }
                else
                {
                    return new AuthResponseDto { Success = false, Message = "Provider không được hỗ trợ" };
                }
            }
            else
            {
                return new AuthResponseDto { Success = false, Message = "Thiếu thông tin xác thực" };
            }

            if (string.IsNullOrEmpty(email))
            {
                return new AuthResponseDto { Success = false, Message = "Không thể lấy email từ tài khoản social" };
            }

            // Tìm user qua social_login_provider và social_login_id
            var user = await _context.Users
                .FirstOrDefaultAsync(u => 
                    u.SocialLoginProvider == socialLoginDto.Provider && 
                    u.SocialLoginId == providerUserId);

            if (user != null)
            {
                // User đã tồn tại với social login này
                _logger.LogInformation("Existing social user logged in: {Email}, Provider: {Provider}", email, socialLoginDto.Provider);
            }
            else
            {
                // Kiểm tra user có tồn tại qua email không (user đã đăng ký bằng email/password)
                user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user != null)
                {
                    // User đã tồn tại với email, liên kết social login
                    user.SocialLoginProvider = socialLoginDto.Provider;
                    user.SocialLoginId = providerUserId;
                    user.UpdatedAt = DateTime.UtcNow;
                    
                    _logger.LogInformation("Linked social login to existing user: {Email}, Provider: {Provider}", email, socialLoginDto.Provider);
                }
                else
                {
                    // Tạo user mới với social login
                    var username = email.Split('@')[0];
                    var existingUsername = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                    if (existingUsername != null)
                    {
                        username = $"{username}_{Guid.NewGuid().ToString().Substring(0, 4)}";
                    }

                    user = new User
                    {
                        Username = username,
                        Email = email,
                        Role = UserRoles.Customer,
                        PasswordHash = string.Empty, // Social login không cần password
                        SocialLoginProvider = socialLoginDto.Provider,
                        SocialLoginId = providerUserId,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    // Tạo Customer record
                    var customer = new Customer
                    {
                        UserId = user.UserId,
                        LoyaltyPoints = 0,
                        CustomerTier = "bronze"
                    };
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();

                    // Tạo CustomerProfile
                    var profile = new CustomerProfile
                    {
                        CustomerId = customer.CustomerId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.CustomerProfiles.Add(profile);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("New user created via social login: {Email}, Provider: {Provider}", email, socialLoginDto.Provider);

                    // Gửi email chào mừng
                    _ = Task.Run(async () => await _emailService.SendWelcomeEmailAsync(user.Email, user.Username));
                }
            }

            // Cập nhật LastLogin
            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Tạo tokens
            var accessToken = _tokenService.GenerateAccessToken(user, new List<string> { user.Role });
            var refreshToken = _tokenService.GenerateRefreshToken();

            _logger.LogInformation("User logged in via {Provider}: {Email}", socialLoginDto.Provider, user.Email);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Đăng nhập thành công",
                Token = new TokenDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(1)
                },
                User = new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    IsActive = user.IsActive ?? true,
                    CreatedAt = user.CreatedAt ?? DateTime.UtcNow
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during social login with {Provider}", socialLoginDto.Provider);
            return new AuthResponseDto
            {
                Success = false,
                Message = "Có lỗi xảy ra trong quá trình đăng nhập"
            };
        }
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto, string ipAddress, string userAgent)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == forgotPasswordDto.Email);
            
            // Luôn trả về true để tránh leak thông tin email có tồn tại hay không
            if (user == null)
            {
                _logger.LogWarning("Password reset requested for non-existent email: {Email}", forgotPasswordDto.Email);
                return true;
            }

            // Không cho phép reset password cho social login users
            if (!string.IsNullOrEmpty(user.SocialLoginProvider))
            {
                _logger.LogWarning("Password reset requested for social login user: {Email}, Provider: {Provider}", 
                    forgotPasswordDto.Email, user.SocialLoginProvider);
                return true; // Still return true to not reveal account type
            }

            // Vô hiệu hóa các token cũ chưa sử dụng
            var oldTokens = await _context.PasswordResetTokens
                .Where(t => t.UserId == user.UserId && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();
            
            foreach (var oldToken in oldTokens)
            {
                oldToken.IsUsed = true;
                oldToken.UsedAt = DateTime.UtcNow;
            }

            // Tạo mã 6 ký tự
            var code = _tokenService.GeneratePasswordResetToken();
            var resetToken = new PasswordResetToken
            {
                UserId = user.UserId,
                Token = code,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_appSettings.PasswordResetTokenExpirationMinutes),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };

            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();

            // Tạo reset link cho trường hợp user muốn dùng link (optional)
            var resetLink = $"{_appSettings.FrontendUrl}/verify-reset-code?email={Uri.EscapeDataString(user.Email)}";

            // Gửi email với mã 6 ký tự
            await _emailService.SendPasswordResetEmailAsync(user.Email, user.Username, resetLink, code);

            _logger.LogInformation("Password reset email sent to: {Email}", user.Email);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during forgot password for email: {Email}", forgotPasswordDto.Email);
            return false;
        }
    }

    public async Task<AuthResponseDto> VerifyResetCodeAsync(VerifyResetCodeDto verifyResetCodeDto)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == verifyResetCodeDto.Email);

            if (user == null)
            {
                return new AuthResponseDto { Success = false, Message = "Email không tồn tại" };
            }

            // Kiểm tra nếu là social login user
            if (!string.IsNullOrEmpty(user.SocialLoginProvider))
            {
                return new AuthResponseDto 
                { 
                    Success = false, 
                    Message = $"Tài khoản này đăng nhập qua {user.SocialLoginProvider}. Không thể đặt lại mật khẩu." 
                };
            }

            // Tìm token hợp lệ
            var resetToken = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => 
                    t.Token == verifyResetCodeDto.Code && 
                    t.UserId == user.UserId && 
                    !t.IsUsed && 
                    t.ExpiresAt > DateTime.UtcNow);

            if (resetToken == null)
            {
                return new AuthResponseDto 
                { 
                    Success = false, 
                    Message = "Mã xác thực không hợp lệ hoặc đã hết hạn. Vui lòng thử lại." 
                };
            }

            _logger.LogInformation("Reset code verified successfully for: {Email}", user.Email);

            // Trả về token để sử dụng cho bước reset password
            return new AuthResponseDto
            {
                Success = true,
                Message = "Xác thực mã thành công",
                Token = new TokenDto
                {
                    AccessToken = verifyResetCodeDto.Code, // Trả về code để dùng ở bước tiếp theo
                    RefreshToken = string.Empty,
                    ExpiresAt = resetToken.ExpiresAt
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during verify reset code for email: {Email}", verifyResetCodeDto.Email);
            return new AuthResponseDto { Success = false, Message = "Có lỗi xảy ra khi xác thực mã" };
        }
    }

    public async Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        try
        {
            // Tìm token hợp lệ và lấy user từ token
            var resetToken = await _context.PasswordResetTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => 
                    t.Token == resetPasswordDto.Token && 
                    !t.IsUsed && 
                    t.ExpiresAt > DateTime.UtcNow);

            if (resetToken == null)
            {
                return new AuthResponseDto 
                { 
                    Success = false, 
                    Message = "Mã xác thực không hợp lệ hoặc đã hết hạn. Vui lòng thử lại." 
                };
            }

            var user = resetToken.User;

            // Kiểm tra nếu là social login user
            if (!string.IsNullOrEmpty(user.SocialLoginProvider))
            {
                return new AuthResponseDto 
                { 
                    Success = false, 
                    Message = $"Tài khoản này đăng nhập qua {user.SocialLoginProvider}. Không thể đặt lại mật khẩu." 
                };
            }

            // Cập nhật mật khẩu
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            // Đánh dấu token đã sử dụng
            resetToken.IsUsed = true;
            resetToken.UsedAt = DateTime.UtcNow;

            // Vô hiệu hóa tất cả refresh tokens cũ (force logout all devices)
            var oldRefreshTokens = await _context.RefreshTokens
                .Where(t => t.UserId == user.UserId && !t.IsRevoked)
                .ToListAsync();

            foreach (var oldRefreshToken in oldRefreshTokens)
            {
                oldRefreshToken.IsRevoked = true;
                oldRefreshToken.RevokedAt = DateTime.UtcNow;
                oldRefreshToken.RevokedReason = "Password reset";
            }

            await _context.SaveChangesAsync();

            // Tạo tokens mới để user có thể đăng nhập luôn
            var accessToken = _tokenService.GenerateAccessToken(user, new List<string> { user.Role });
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Lưu refresh token mới
            var newRefreshToken = new RefreshToken
            {
                UserId = user.UserId,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Password reset successful for: {Email}", user.Email);

            // Gửi email thông báo password đã thay đổi
            _ = Task.Run(async () => await _emailService.SendPasswordChangedNotificationAsync(user.Email, user.Username));

            return new AuthResponseDto
            {
                Success = true,
                Message = "Đặt lại mật khẩu thành công",
                Token = new TokenDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(1)
                },
                User = new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    IsActive = user.IsActive ?? true,
                    CreatedAt = user.CreatedAt ?? DateTime.UtcNow
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset");
            return new AuthResponseDto { Success = false, Message = "Có lỗi xảy ra khi đặt lại mật khẩu" };
        }
    }

    public async Task<bool> ValidateResetTokenAsync(string token, string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return false;

        var resetToken = await _context.PasswordResetTokens
            .FirstOrDefaultAsync(t => 
                t.Token == token && 
                t.UserId == user.UserId && 
                !t.IsUsed && 
                t.ExpiresAt > DateTime.UtcNow);

        return resetToken != null;
    }

    public async Task<AuthResponseDto> VerifyEmailAsync(string token, string email)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return new AuthResponseDto { Success = false, Message = "Email không tồn tại" };
            }

            if (user.EmailVerified)
            {
                return new AuthResponseDto { Success = true, Message = "Email đã được xác thực trước đó" };
            }

            // Tìm token hợp lệ
            var verificationToken = await _context.EmailVerificationTokens
                .FirstOrDefaultAsync(t =>
                    t.Token == token &&
                    t.UserId == user.UserId &&
                    !t.IsUsed &&
                    t.ExpiresAt > DateTime.UtcNow);

            if (verificationToken == null)
            {
                return new AuthResponseDto 
                { 
                    Success = false, 
                    Message = "Link xác thực không hợp lệ hoặc đã hết hạn" 
                };
            }

            // Cập nhật email verified
            user.EmailVerified = true;
            user.UpdatedAt = DateTime.UtcNow;

            // Đánh dấu token đã sử dụng
            verificationToken.IsUsed = true;
            verificationToken.UsedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Email verified successfully for: {Email}", user.Email);

            // Gửi email chào mừng sau khi verify
            _ = Task.Run(async () => await _emailService.SendWelcomeEmailAsync(user.Email, user.Username));

            return new AuthResponseDto
            {
                Success = true,
                Message = "Email đã được xác thực thành công"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during email verification for: {Email}", email);
            return new AuthResponseDto { Success = false, Message = "Có lỗi xảy ra khi xác thực email" };
        }
    }

    public async Task<bool> ResendVerificationEmailAsync(string email, string ipAddress)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                _logger.LogWarning("Resend verification requested for non-existent email: {Email}", email);
                return true; // Return true to not reveal email existence
            }

            if (user.EmailVerified)
            {
                _logger.LogWarning("Resend verification requested for already verified email: {Email}", email);
                return true;
            }

            // Vô hiệu hóa các token cũ chưa sử dụng
            var oldTokens = await _context.EmailVerificationTokens
                .Where(t => t.UserId == user.UserId && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            foreach (var oldToken in oldTokens)
            {
                oldToken.IsUsed = true;
                oldToken.UsedAt = DateTime.UtcNow;
            }

            // Tạo token mới
            var verificationToken = _tokenService.GenerateEmailVerificationToken();
            var emailToken = new EmailVerificationToken
            {
                UserId = user.UserId,
                Token = verificationToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                CreatedAt = DateTime.UtcNow,
                IpAddress = ipAddress
            };

            _context.EmailVerificationTokens.Add(emailToken);
            await _context.SaveChangesAsync();

            // Tạo verification link
            var verificationLink = $"{_appSettings.FrontendUrl}/verify-email?token={verificationToken}&email={Uri.EscapeDataString(user.Email)}";

            // Gửi email
            await _emailService.SendEmailVerificationAsync(user.Email, user.Username, verificationLink, verificationToken);

            _logger.LogInformation("Verification email resent to: {Email}", user.Email);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during resend verification email for: {Email}", email);
            return false;
        }
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, string? ipAddress = null, string? userAgent = null)
    {
        try
        {
            // Tìm refresh token trong database
            var token = await _context.RefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token == refreshToken);

            if (token == null)
            {
                return new AuthResponseDto { Success = false, Message = "Token không hợp lệ" };
            }

            // Kiểm tra token đã bị revoke
            if (token.IsRevoked)
            {
                _logger.LogWarning("Revoked refresh token used by user {UserId}", token.UserId);
                return new AuthResponseDto { Success = false, Message = "Token đã bị vô hiệu hóa" };
            }

            // Kiểm tra token đã expire
            if (token.IsExpired)
            {
                return new AuthResponseDto { Success = false, Message = "Token đã hết hạn. Vui lòng đăng nhập lại" };
            }

            var user = token.User;

            // Kiểm tra user còn active không
            if (!(user.IsActive ?? true))
            {
                return new AuthResponseDto { Success = false, Message = "Tài khoản đã bị khóa" };
            }

            // Generate new access token
            var newAccessToken = _tokenService.GenerateAccessToken(user, new List<string> { user.Role });

            // Generate new refresh token (rotation)
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            // Revoke old refresh token và thay thế bằng token mới
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
            token.ReplacedByToken = newRefreshToken;
            token.RevokedReason = "Replaced by new token";

            // Lưu refresh token mới
            var newToken = new RefreshToken
            {
                UserId = user.UserId,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };

            _context.RefreshTokens.Add(newToken);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Refresh token rotated for user: {Email}", user.Email);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Token đã được làm mới",
                Token = new TokenDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(1)
                },
                User = new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    IsActive = user.IsActive ?? true,
                    CreatedAt = user.CreatedAt ?? DateTime.UtcNow
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during refresh token");
            return new AuthResponseDto { Success = false, Message = "Có lỗi xảy ra khi làm mới token" };
        }
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken, string? reason = null)
    {
        try
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == refreshToken);

            if (token == null) return false;

            if (token.IsRevoked) return true;

            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
            token.RevokedReason = reason ?? "Revoked without reason";

            await _context.SaveChangesAsync();

            _logger.LogInformation("Refresh token revoked for user: {UserId}", token.UserId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during revoke token");
            return false;
        }
    }

    public async Task<bool> RevokeAllUserTokensAsync(int userId, string? reason = null)
    {
        try
        {
            var tokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId && !t.IsRevoked)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
                token.RevokedReason = reason ?? "All tokens revoked";
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("All refresh tokens revoked for user: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during revoke all tokens for user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> LogoutAsync(int userId)
    {
        try
        {
            // Revoke tất cả refresh tokens của user
            await RevokeAllUserTokensAsync(userId, "User logout");
            _logger.LogInformation("User logged out: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout for user: {UserId}", userId);
            return false;
        }
    }

    #region Private Methods

    private async Task<GoogleUserInfo?> GetGoogleUserInfoAsync(string accessToken)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            var response = await client.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GoogleUserInfo>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting Google user info");
            return null;
        }
    }

    private async Task<FacebookUserInfo?> GetFacebookUserInfoAsync(string accessToken)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://graph.facebook.com/me?fields=id,name,email,picture&access_token={accessToken}");
            
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FacebookUserInfo>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting Facebook user info");
            return null;
        }
    }

    #endregion
}
