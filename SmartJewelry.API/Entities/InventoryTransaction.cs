using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class InventoryTransaction
{
    public int TransactionId { get; set; }

    public int ProductId { get; set; }

    public string TransactionType { get; set; } = null!;

    public int QuantityChange { get; set; }

    public string? ReferenceType { get; set; }

    public int? ReferenceId { get; set; }

    public int? StaffId { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? Notes { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User? Staff { get; set; }
}
