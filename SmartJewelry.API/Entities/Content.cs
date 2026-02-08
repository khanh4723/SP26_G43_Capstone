using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class Content
{
    public int ContentId { get; set; }

    public string ContentTitle { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public string UrlSlug { get; set; } = null!;

    public int? AuthorId { get; set; }

    public string? PublicationStatus { get; set; }

    public string? ContentBody { get; set; }

    public string? Excerpt { get; set; }

    public string? FeaturedImageUrl { get; set; }

    public string? SeoMetadata { get; set; }

    public int? ViewCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? PublishedAt { get; set; }

    public virtual ContentCreator? Author { get; set; }
}
