using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class ProductImage
{
    public int ImageId { get; set; }

    public int ProductId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string? ImageType { get; set; }

    public int? DisplayOrder { get; set; }

    public string? AltText { get; set; }

    public DateTime? UploadedAt { get; set; }

    public virtual Product Product { get; set; } = null!;
}
