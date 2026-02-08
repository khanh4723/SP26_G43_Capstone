using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class ContentCreator
{
    public int ContentCreatorId { get; set; }

    public int UserId { get; set; }

    public string? SpecialtyArea { get; set; }

    public int? ContentCount { get; set; }

    public virtual ICollection<Collection> Collections { get; set; } = new List<Collection>();

    public virtual ICollection<Content> Contents { get; set; } = new List<Content>();

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();

    public virtual User User { get; set; } = null!;
}
