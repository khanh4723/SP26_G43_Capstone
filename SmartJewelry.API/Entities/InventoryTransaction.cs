namespace SmartJewelry.API.Entities;

public class InventoryTransaction
{
    public int TransactionId { get; set; }
    public int ProductId { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public int QuantityChange { get; set; }
    public string? ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
    public int? StaffId { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Product Product { get; set; } = null!;
    public virtual User? Staff { get; set; }
}
