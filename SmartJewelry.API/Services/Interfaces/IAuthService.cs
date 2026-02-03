using SmartJewelry.API.DTOs.Auth;

namespace SmartJewelry.API.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto> SocialLoginAsync(SocialLoginDto socialLoginDto);
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, string? ipAddress = null, string? userAgent = null);
    Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto, string ipAddress, string userAgent);
    Task<AuthResponseDto> VerifyResetCodeAsync(VerifyResetCodeDto verifyResetCodeDto);
    Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<bool> ValidateResetTokenAsync(string token, string email);
    Task<AuthResponseDto> VerifyEmailAsync(string token, string email);
    Task<bool> ResendVerificationEmailAsync(string email, string ipAddress);
    Task<bool> RevokeTokenAsync(string refreshToken, string? reason = null);
    Task<bool> RevokeAllUserTokensAsync(int userId, string? reason = null);
    Task<bool> LogoutAsync(int userId);
}
