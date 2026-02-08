using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string PromotionCode { get; set; } = null!;

    public string PromotionName { get; set; } = null!;

    public string? Description { get; set; }

    public string DiscountType { get; set; } = null!;

    public decimal? DiscountValue { get; set; }

    public string? ApplicableProducts { get; set; }

    public decimal? MinOrderAmount { get; set; }

    public decimal? MaxDiscountAmount { get; set; }

    public int? UsageLimitPerCustomer { get; set; }

    public int? TotalUsageLimit { get; set; }

    public int? CurrentUsageCount { get; set; }

    public string? UsageHistory { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int? CreatorId { get; set; }

    public int? ManagerId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ContentCreator? Creator { get; set; }

    public virtual StoreManager? Manager { get; set; }
}
