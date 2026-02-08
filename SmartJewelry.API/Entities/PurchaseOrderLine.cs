namespace SmartJewelry.API.Entities;

public class PurchaseOrderLine
{
    public int PoLineId { get; set; }
    public int PurchaseOrderId { get; set; }
    public int LineNumber { get; set; }
    public string LineType { get; set; } = string.Empty; // existing_sku, new_item
    public int? ProductId { get; set; }
    public string? NewItemSpec { get; set; } // JSON
    public string? RequiredChecklist { get; set; } // JSON
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
    public virtual Product? Product { get; set; }
}
