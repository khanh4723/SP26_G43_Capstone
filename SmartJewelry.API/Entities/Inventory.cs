using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int ProductId { get; set; }

    public int CurrentStock { get; set; }

    public int? MinimumStockThreshold { get; set; }

    public string? WarehouseLocation { get; set; }

    public DateOnly? StockedSince { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual Product Product { get; set; } = null!;
}
