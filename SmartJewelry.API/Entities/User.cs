namespace SmartJewelry.API.Entities;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? SocialLoginProvider { get; set; }
    public string? SocialLoginId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }
    public bool IsActive { get; set; } = true;
    public bool EmailVerified { get; set; } = false;

    // Navigation properties
    public virtual Customer? Customer { get; set; }
    public virtual SalesStaff? SalesStaff { get; set; }
    public virtual InventoryManager? InventoryManager { get; set; }
    public virtual StoreManager? StoreManager { get; set; }
    public virtual Admin? Admin { get; set; }
    public virtual ContentCreator? ContentCreator { get; set; }
    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
    public virtual ICollection<EmailVerificationToken> EmailVerificationTokens { get; set; } = new List<EmailVerificationToken>();
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}

// Role constants
public static class UserRoles
{
    public const string Guest = "guest";
    public const string Customer = "customer";
    public const string SalesStaff = "sales_staff";
    public const string InventoryManager = "inventory_manager";
    public const string StoreManager = "store_manager";
    public const string Admin = "admin";
    public const string ContentCreator = "content_creator";
}
