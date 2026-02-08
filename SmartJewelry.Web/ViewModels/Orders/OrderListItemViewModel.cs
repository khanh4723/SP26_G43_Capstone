namespace SmartJewelry.Web.ViewModels.Orders;

public class OrderListItemViewModel
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }

    public string OrderType { get; set; } = string.Empty;
    public string? OrderStatus { get; set; }

    public decimal GrandTotal { get; set; }
    public DateTime? OrderDate { get; set; }

    public int ItemsCount { get; set; }
    public string? LatestPaymentStatus { get; set; }
    public string? LatestPaymentMethod { get; set; }
}
