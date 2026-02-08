using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class LoyaltyTransaction
{
    public int TransactionId { get; set; }

    public int CustomerId { get; set; }

    public string TransactionType { get; set; } = null!;

    public int PointsChange { get; set; }

    public int? OrderId { get; set; }

    public string? Description { get; set; }

    public DateTime? TransactionDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
