namespace SmartJewelry.API.Entities;

public class Cart
{
    public int CartId { get; set; }
    public int CustomerId { get; set; }
    public string? Items { get; set; } // JSON
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
}
