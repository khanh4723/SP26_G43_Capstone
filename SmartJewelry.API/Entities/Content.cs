namespace SmartJewelry.API.Entities;

public class Content
{
    public int ContentId { get; set; }
    public string ContentTitle { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string UrlSlug { get; set; } = string.Empty;
    public int? AuthorId { get; set; }
    public string PublicationStatus { get; set; } = "draft";
    public string? ContentBody { get; set; }
    public string? Excerpt { get; set; }
    public string? FeaturedImageUrl { get; set; }
    public string? SeoMetadata { get; set; } // JSON
    public int ViewCount { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PublishedAt { get; set; }

    // Navigation properties
    public virtual ContentCreator? Author { get; set; }
}
