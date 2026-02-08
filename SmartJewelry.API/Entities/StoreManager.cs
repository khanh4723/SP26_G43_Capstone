namespace SmartJewelry.API.Entities;

public class StoreManager
{
    public int StoreManagerId { get; set; }
    public int UserId { get; set; }
    public string? ManagedDepartment { get; set; }
    public int SupervisedStaffCount { get; set; } = 0;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    public virtual ICollection<PublishRequest> PublishRequests { get; set; } = new List<PublishRequest>();
    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}
