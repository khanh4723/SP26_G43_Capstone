namespace SmartJewelry.API.Entities;

public class Address
{
    public int AddressId { get; set; }
    public int CustomerId { get; set; }
    public string RecipientName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string AddressLine { get; set; } = string.Empty;
    public string? Ward { get; set; }
    public string? WardCode { get; set; }
    public string District { get; set; } = string.Empty;
    public string? DistrictCode { get; set; }
    public string City { get; set; } = string.Empty;
    public string? CityCode { get; set; }
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
}
