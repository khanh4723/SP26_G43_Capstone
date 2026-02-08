namespace SmartJewelry.API.Entities;

public class Order
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int? SalesStaffId { get; set; }
    public string OrderType { get; set; } = string.Empty; // retail, custom
    public string OrderStatus { get; set; } = "pending";
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; } = 0;
    public decimal TaxAmount { get; set; } = 0;
    public decimal GrandTotal { get; set; }
    public string? PromotionCode { get; set; }
    public string? ShippingInfo { get; set; } // JSON
    public string? StatusHistory { get; set; } // JSON
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
    public virtual SalesStaff? SalesStaff { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual CustomOrderDetail? CustomOrderDetail { get; set; }
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
