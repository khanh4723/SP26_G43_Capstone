using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class PurchaseOrder
{
    public int PurchaseOrderId { get; set; }

    public string PoNumber { get; set; } = null!;

    public int SupplierId { get; set; }

    public int? StoreManagerId { get; set; }

    public string? OrderStatus { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateOnly? ExpectedDeliveryDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Notes { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual ICollection<GoodsReceiptNote> GoodsReceiptNotes { get; set; } = new List<GoodsReceiptNote>();

    public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();

    public virtual StoreManager? StoreManager { get; set; }

    public virtual Supplier Supplier { get; set; } = null!;
}
