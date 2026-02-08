namespace SmartJewelry.API.Entities;

public class Promotion
{
    public int PromotionId { get; set; }
    public string PromotionCode { get; set; } = string.Empty;
    public string PromotionName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string DiscountType { get; set; } = string.Empty;
    public decimal? DiscountValue { get; set; }
    public string? ApplicableProducts { get; set; } // JSON
    public decimal MinOrderAmount { get; set; } = 0;
    public decimal? MaxDiscountAmount { get; set; }
    public int UsageLimitPerCustomer { get; set; } = 1;
    public int? TotalUsageLimit { get; set; }
    public int CurrentUsageCount { get; set; } = 0;
    public string? UsageHistory { get; set; } // JSON
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? CreatorId { get; set; }
    public int? ManagerId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ContentCreator? Creator { get; set; }
    public virtual StoreManager? Manager { get; set; }
}
