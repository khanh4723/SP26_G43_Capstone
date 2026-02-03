namespace SmartJewelry.API.Entities;

public class ProductImage
{
    public int ImageId { get; set; }
    public int ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string ImageType { get; set; } = "gallery";
    public int DisplayOrder { get; set; } = 0;
    public string? AltText { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Product Product { get; set; } = null!;
}
