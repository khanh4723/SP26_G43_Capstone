namespace SmartJewelry.API.Entities;

public class LoyaltyTransaction
{
    public int TransactionId { get; set; }
    public int CustomerId { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public int PointsChange { get; set; }
    public int? OrderId { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
}
