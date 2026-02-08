using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class GoldRate
{
    public int RateId { get; set; }

    public string GoldType { get; set; } = null!;

    public decimal RatePerGram { get; set; }

    public string? RateSource { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public int? CreatedBy { get; set; }

    public string? Notes { get; set; }

    public virtual Admin? CreatedByNavigation { get; set; }
}
