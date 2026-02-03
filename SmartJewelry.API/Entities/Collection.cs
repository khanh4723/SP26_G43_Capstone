namespace SmartJewelry.API.Entities;

public class Collection
{
    public int CollectionId { get; set; }
    public string CollectionName { get; set; } = string.Empty;
    public string CollectionType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? BannerImageUrl { get; set; }
    public string? Products { get; set; } // JSON array of product IDs
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ContentCreator? Creator { get; set; }
}
