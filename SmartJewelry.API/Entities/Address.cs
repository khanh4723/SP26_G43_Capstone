using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class Address
{
    public int AddressId { get; set; }

    public int CustomerId { get; set; }

    public string RecipientName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string AddressLine { get; set; } = null!;

    public string? Ward { get; set; }

    public string? WardCode { get; set; }

    public string District { get; set; } = null!;

    public string? DistrictCode { get; set; }

    public string City { get; set; } = null!;

    public string? CityCode { get; set; }

    public bool IsDefault { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
