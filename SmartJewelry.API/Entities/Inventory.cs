namespace SmartJewelry.API.Entities;

public class Inventory
{
    public int InventoryId { get; set; }
    public int ProductId { get; set; }
    public int CurrentStock { get; set; } = 0;
    public int MinimumStockThreshold { get; set; } = 10;
    public string? WarehouseLocation { get; set; }
    public DateTime? StockedSince { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Product Product { get; set; } = null!;
}
