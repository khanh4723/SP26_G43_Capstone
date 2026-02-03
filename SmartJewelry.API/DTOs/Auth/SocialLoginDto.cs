using System.ComponentModel.DataAnnotations;

namespace SmartJewelry.API.DTOs.Auth;

public class SocialLoginDto
{
    [Required(ErrorMessage = "Provider là bắt buộc")]
    public string Provider { get; set; } = string.Empty; // "Google" hoặc "Facebook"

    public string? AccessToken { get; set; }
    
    // Thông tin user từ Web (khi đã xác thực qua OAuth)
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? ProviderUserId { get; set; }
}

public class GoogleUserInfo
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Picture { get; set; }
    public bool VerifiedEmail { get; set; }
}

public class FacebookUserInfo
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public FacebookPicture? Picture { get; set; }
}

public class FacebookPicture
{
    public FacebookPictureData? Data { get; set; }
}

public class FacebookPictureData
{
    public string? Url { get; set; }
}
