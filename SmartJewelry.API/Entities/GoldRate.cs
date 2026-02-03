namespace SmartJewelry.API.Entities;

public class GoldRate
{
    public int RateId { get; set; }
    public string GoldType { get; set; } = string.Empty;
    public decimal RatePerGram { get; set; }
    public string? RateSource { get; set; }
    public DateTime EffectiveDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Admin? Creator { get; set; }
}
