using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartJewelry.API.DTOs.Auth;
using SmartJewelry.API.DTOs.Common;
using SmartJewelry.API.Services.Interfaces;

namespace SmartJewelry.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Đăng ký tài khoản mới
    /// </summary>
    /// <param name="registerDto">Thông tin đăng ký</param>
    /// <returns>Token và thông tin user</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse.FailResponse("Dữ liệu không hợp lệ", errors));
        }

        var result = await _authService.RegisterAsync(registerDto);

        if (!result.Success)
        {
            return BadRequest(ApiResponse<AuthResponseDto>.FailResponse(result.Message));
        }

        _logger.LogInformation("New user registered: {Email}", registerDto.Email);
        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, result.Message));
    }

    /// <summary>
    /// Đăng nhập bằng email và mật khẩu
    /// </summary>
    /// <param name="loginDto">Thông tin đăng nhập</param>
    /// <returns>Token và thông tin user</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse.FailResponse("Dữ liệu không hợp lệ", errors));
        }

        var result = await _authService.LoginAsync(loginDto);

        if (!result.Success)
        {
            return Unauthorized(ApiResponse<AuthResponseDto>.FailResponse(result.Message));
        }

        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, result.Message));
    }

    /// <summary>
    /// Đăng nhập bằng Google hoặc Facebook
    /// </summary>
    /// <param name="socialLoginDto">Provider và Access Token từ social login</param>
    /// <returns>Token và thông tin user</returns>
    [HttpPost("social-login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SocialLogin([FromBody] SocialLoginDto socialLoginDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse.FailResponse("Dữ liệu không hợp lệ", errors));
        }

        var result = await _authService.SocialLoginAsync(socialLoginDto);

        if (!result.Success)
        {
            return Unauthorized(ApiResponse<AuthResponseDto>.FailResponse(result.Message));
        }

        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, result.Message));
    }

    /// <summary>
    /// Yêu cầu đặt lại mật khẩu (gửi email chứa link reset)
    /// </summary>
    /// <param name="forgotPasswordDto">Email cần đặt lại mật khẩu</param>
    /// <returns>Thông báo đã gửi email</returns>
    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse.FailResponse("Dữ liệu không hợp lệ", errors));
        }

        // Lấy IP và User Agent để lưu vào token (bảo mật)
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        var userAgent = Request.Headers["User-Agent"].ToString();

        var result = await _authService.ForgotPasswordAsync(forgotPasswordDto, ipAddress, userAgent);

        // Luôn trả về success để tránh leak thông tin
        return Ok(ApiResponse.SuccessResponse("Nếu email tồn tại trong hệ thống, bạn sẽ nhận được link đặt lại mật khẩu"));
    }

    /// <summary>
    /// Xác thực mã reset password (bước 2 sau khi nhận email)
    /// </summary>
    /// <param name="verifyResetCodeDto">Email và mã xác thực 6 ký tự</param>
    /// <returns>Token để sử dụng cho bước đặt lại mật khẩu</returns>
    [HttpPost("verify-reset-code")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeDto verifyResetCodeDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse.FailResponse("Dữ liệu không hợp lệ", errors));
        }

        var result = await _authService.VerifyResetCodeAsync(verifyResetCodeDto);

        if (!result.Success)
        {
            return BadRequest(ApiResponse<AuthResponseDto>.FailResponse(result.Message));
        }

        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, result.Message));
    }

    /// <summary>
    /// Đặt lại mật khẩu với token đã được xác thực (bước 3)
    /// </summary>
    /// <param name="resetPasswordDto">Token và mật khẩu mới</param>
    /// <returns>Token và thông tin user nếu thành công</returns>
    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse.FailResponse("Dữ liệu không hợp lệ", errors));
        }

        var result = await _authService.ResetPasswordAsync(resetPasswordDto);

        if (!result.Success)
        {
            return BadRequest(ApiResponse<AuthResponseDto>.FailResponse(result.Message));
        }

        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, result.Message));
    }

    /// <summary>
    /// Kiểm tra token đặt lại mật khẩu có hợp lệ không
    /// </summary>
    /// <param name="token">Token từ email</param>
    /// <param name="email">Email của user</param>
    /// <returns>True nếu token hợp lệ</returns>
    [HttpGet("validate-reset-token")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateResetToken([FromQuery] string token, [FromQuery] string email)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
        {
            return BadRequest(ApiResponse<bool>.FailResponse("Token và email là bắt buộc"));
        }

        var isValid = await _authService.ValidateResetTokenAsync(token, email);
        
        if (!isValid)
        {
            return Ok(ApiResponse<bool>.SuccessResponse(false, "Token không hợp lệ hoặc đã hết hạn"));
        }

        return Ok(ApiResponse<bool>.SuccessResponse(true, "Token hợp lệ"));
    }

    /// <summary>
    /// Xác thực email với token
    /// </summary>
    /// <param name="token">Token từ email</param>
    /// <param name="email">Email của user</param>
    /// <returns>Thông báo xác thực thành công</returns>
    [HttpGet("verify-email")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token, [FromQuery] string email)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
        {
            return BadRequest(ApiResponse.FailResponse("Token và email là bắt buộc"));
        }

        var result = await _authService.VerifyEmailAsync(token, email);

        if (!result.Success)
        {
            return BadRequest(ApiResponse.FailResponse(result.Message));
        }

        return Ok(ApiResponse.SuccessResponse(result.Message));
    }

    /// <summary>
    /// Gửi lại email xác thực
    /// </summary>
    /// <param name="email">Email cần xác thực</param>
    /// <returns>Thông báo đã gửi email</returns>
    [HttpPost("resend-verification")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ResendVerificationEmail([FromBody] string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest(ApiResponse.FailResponse("Email là bắt buộc"));
        }

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        var result = await _authService.ResendVerificationEmailAsync(email, ipAddress);

        return Ok(ApiResponse.SuccessResponse("Email xác thực đã được gửi"));
    }

    /// <summary>
    /// Refresh access token
    /// </summary>
    /// <param name="refreshToken">Refresh token</param>
    /// <returns>Token mới</returns>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        var userAgent = Request.Headers["User-Agent"].ToString();

        var result = await _authService.RefreshTokenAsync(refreshToken, ipAddress, userAgent);

        if (!result.Success)
        {
            return Unauthorized(ApiResponse<AuthResponseDto>.FailResponse(result.Message));
        }

        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, result.Message));
    }

    /// <summary>
    /// Revoke một refresh token cụ thể
    /// </summary>
    /// <param name="refreshToken">Token cần revoke</param>
    /// <returns>Thông báo revoke thành công</returns>
    [HttpPost("revoke-token")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RevokeToken([FromBody] string refreshToken)
    {
        var result = await _authService.RevokeTokenAsync(refreshToken, "Revoked by user");

        if (!result)
        {
            return BadRequest(ApiResponse.FailResponse("Không thể revoke token"));
        }

        return Ok(ApiResponse.SuccessResponse("Token đã bị vô hiệu hóa"));
    }

    /// <summary>
    /// Đăng xuất
    /// </summary>
    /// <returns>Thông báo đăng xuất thành công</returns>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (int.TryParse(userIdClaim, out int userId))
        {
            await _authService.LogoutAsync(userId);
        }

        return Ok(ApiResponse.SuccessResponse("Đăng xuất thành công"));
    }

    /// <summary>
    /// Lấy thông tin user hiện tại
    /// </summary>
    /// <returns>Thông tin user</returns>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetCurrentUser()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        var emailClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var nameClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(ApiResponse.FailResponse("Unauthorized"));
        }

        var userDto = new UserDto
        {
            UserId = int.Parse(userIdClaim),
            Username = nameClaim ?? string.Empty,
            Email = emailClaim ?? string.Empty,
            Role = roles.FirstOrDefault() ?? string.Empty,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return Ok(ApiResponse<UserDto>.SuccessResponse(userDto, "Lấy thông tin thành công"));
    }
}
