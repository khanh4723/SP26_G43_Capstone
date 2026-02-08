namespace SmartJewelry.API.DTOs.Profile;

public class ProfileDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLogin { get; set; }
    
    // Customer specific
    public int? CustomerId { get; set; }
    public int LoyaltyPoints { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string CustomerTier { get; set; } = "bronze";
    
    // Customer Profile
    public List<AddressDto> Addresses { get; set; } = new();
    public List<string> RingSizes { get; set; } = new();
    public Dictionary<string, string> Preferences { get; set; } = new();
}
