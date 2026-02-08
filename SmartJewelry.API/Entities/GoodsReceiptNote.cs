using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class GoodsReceiptNote
{
    public int GrnId { get; set; }

    public string GrnNumber { get; set; } = null!;

    public int PurchaseOrderId { get; set; }

    public int? InventoryManagerId { get; set; }

    public string? ReceiptStatus { get; set; }

    public DateTime? ReceiptDate { get; set; }

    public DateTime? PostedAt { get; set; }

    public string? Lines { get; set; }

    public string? Notes { get; set; }

    public virtual InventoryManager? InventoryManager { get; set; }

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
}
