namespace SmartJewelry.API.Entities;

public class Gemstone
{
    public int GemstoneId { get; set; }
    public string GemstoneCode { get; set; } = string.Empty;
    public string GemstoneType { get; set; } = string.Empty;
    public decimal? WeightCarats { get; set; }
    public string? Shape { get; set; }
    public string? ClarityGrade { get; set; }
    public string? ColorGrade { get; set; }
    public string? CutGrade { get; set; }
    public string? Treatment { get; set; }
    public string? OriginCountry { get; set; }
    public string? Fluorescence { get; set; }
    public string? CertificateNumber { get; set; }
    public string? CertificateLab { get; set; }
    public string? CertFileUrl { get; set; }
    public string? Image360Url { get; set; }
    public string? VideoUrl { get; set; }
    public decimal? PurchasePrice { get; set; }
    public decimal? SellingPrice { get; set; }
    public decimal? MarkupPercentage { get; set; }
    public string GemstoneStatus { get; set; } = "available";
    public DateTime? ReservedUntil { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<CustomOrderDetail> CustomOrderDetails { get; set; } = new List<CustomOrderDetail>();
}
