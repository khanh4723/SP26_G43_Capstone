namespace SmartJewelry.API.Entities;

public class PurchaseOrder
{
    public int PurchaseOrderId { get; set; }
    public string PoNumber { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public int? StoreManagerId { get; set; }
    public string OrderStatus { get; set; } = "draft";
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpectedDeliveryDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? Notes { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Navigation properties
    public virtual Supplier Supplier { get; set; } = null!;
    public virtual StoreManager? StoreManager { get; set; }
    public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();
    public virtual ICollection<GoodsReceiptNote> GoodsReceiptNotes { get; set; } = new List<GoodsReceiptNote>();
}
