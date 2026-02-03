using SmartJewelry.API.Entities;

namespace SmartJewelry.API.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user, IEnumerable<string> roles);
    string GenerateRefreshToken();
    string GeneratePasswordResetToken();
    string GenerateEmailVerificationToken();
    bool ValidateToken(string token);
    int? GetUserIdFromToken(string token);
}
