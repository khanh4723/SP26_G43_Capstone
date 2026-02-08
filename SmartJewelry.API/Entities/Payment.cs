namespace SmartJewelry.API.Entities;

public class Payment
{
    public int PaymentId { get; set; }
    public string PaymentNumber { get; set; } = string.Empty;
    public int OrderId { get; set; }
    public string PaymentType { get; set; } = string.Empty; // full, deposit, balance
    public decimal PaymentAmount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = "pending";
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public string? TransactionReference { get; set; }
    public decimal? DepositPercentage { get; set; }
    public string? GatewayResponse { get; set; } // JSON

    // Navigation properties
    public virtual Order Order { get; set; } = null!;
}
