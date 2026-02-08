using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class Cart
{
    public int CartId { get; set; }

    public int CustomerId { get; set; }

    public string? Items { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
