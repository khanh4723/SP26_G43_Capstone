using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class CustomerProfile
{
    public int ProfileId { get; set; }

    public int CustomerId { get; set; }

    public string? RingSizes { get; set; }

    public string? Addresses { get; set; }

    public string? Preferences { get; set; }

    public string? Vouchers { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
