namespace SmartJewelry.API.Entities;

public class CustomerProfile
{
    public int ProfileId { get; set; }
    public int CustomerId { get; set; }
    public string? RingSizes { get; set; } // JSON
    public string? Addresses { get; set; } // JSON
    public string? Preferences { get; set; } // JSON
    public string? Vouchers { get; set; } // JSON
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
}
