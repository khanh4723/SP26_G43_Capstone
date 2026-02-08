using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public string PaymentNumber { get; set; } = null!;

    public int OrderId { get; set; }

    public string PaymentType { get; set; } = null!;

    public decimal PaymentAmount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string? PaymentStatus { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? TransactionReference { get; set; }

    public decimal? DepositPercentage { get; set; }

    public string? GatewayResponse { get; set; }

    public virtual Order Order { get; set; } = null!;
}
