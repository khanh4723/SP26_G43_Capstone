namespace SmartJewelry.API.Entities;

public class InventoryManager
{
    public int InventoryManagerId { get; set; }
    public int UserId { get; set; }
    public string? WarehouseLocation { get; set; }
    public string? CertificationLevel { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<GoodsReceiptNote> GoodsReceiptNotes { get; set; } = new List<GoodsReceiptNote>();
    public virtual ICollection<PublishRequest> PublishRequests { get; set; } = new List<PublishRequest>();
}
