namespace SmartJewelry.API.Entities;

public class PublishRequest
{
    public int RequestId { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public int InventoryManagerId { get; set; }
    public int? StoreManagerId { get; set; }
    public string RequestStatus { get; set; } = "pending";
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReviewedAt { get; set; }
    public string? Items { get; set; } // JSON
    public string? Notes { get; set; }
    public string? ReviewerNotes { get; set; }

    // Navigation properties
    public virtual InventoryManager InventoryManager { get; set; } = null!;
    public virtual StoreManager? StoreManager { get; set; }
}
