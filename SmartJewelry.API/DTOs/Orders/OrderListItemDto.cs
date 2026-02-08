namespace SmartJewelry.API.DTOs.Orders;

public class OrderListItemDto
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;

    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }

    public string OrderType { get; set; } = string.Empty;
    public string? OrderStatus { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal GrandTotal { get; set; }

    public DateTime? OrderDate { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int ItemsCount { get; set; }
    public string? LatestPaymentStatus { get; set; }
    public string? LatestPaymentMethod { get; set; }
}
