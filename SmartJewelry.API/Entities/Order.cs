using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public string OrderNumber { get; set; } = null!;

    public int CustomerId { get; set; }

    public int? SalesStaffId { get; set; }

    public string OrderType { get; set; } = null!;

    public string? OrderStatus { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal GrandTotal { get; set; }

    public string? PromotionCode { get; set; }

    public string? ShippingInfo { get; set; }

    public string? StatusHistory { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual CustomOrderDetail? CustomOrderDetail { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual SalesStaff? SalesStaff { get; set; }
}
