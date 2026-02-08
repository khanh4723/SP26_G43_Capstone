using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class Collection
{
    public int CollectionId { get; set; }

    public string CollectionName { get; set; } = null!;

    public string CollectionType { get; set; } = null!;

    public string? Description { get; set; }

    public string? BannerImageUrl { get; set; }

    public string? Products { get; set; }

    public int? DisplayOrder { get; set; }

    public bool? IsActive { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ContentCreator? CreatedByNavigation { get; set; }

    // Alias for CreatedByNavigation (used in DbContext)
    public virtual ContentCreator? Creator => CreatedByNavigation;
}
