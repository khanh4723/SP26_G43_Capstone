using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class PurchaseOrderLine
{
    public int PoLineId { get; set; }

    public int PurchaseOrderId { get; set; }

    public int LineNumber { get; set; }

    public string LineType { get; set; } = null!;

    public int? ProductId { get; set; }

    public string? NewItemSpec { get; set; }

    public string? RequiredChecklist { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? LineTotal { get; set; }

    public string? Notes { get; set; }

    public virtual Product? Product { get; set; }

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
}
