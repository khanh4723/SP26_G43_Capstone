namespace SmartJewelry.API.Entities;

public class GoodsReceiptNote
{
    public int GrnId { get; set; }
    public string GrnNumber { get; set; } = string.Empty;
    public int PurchaseOrderId { get; set; }
    public int? InventoryManagerId { get; set; }
    public string ReceiptStatus { get; set; } = "draft";
    public DateTime ReceiptDate { get; set; } = DateTime.UtcNow;
    public DateTime? PostedAt { get; set; }
    public string? Lines { get; set; } // JSON
    public string? Notes { get; set; }

    // Navigation properties
    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
    public virtual InventoryManager? InventoryManager { get; set; }
}
