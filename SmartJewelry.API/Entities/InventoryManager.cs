using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class InventoryManager
{
    public int InventoryManagerId { get; set; }

    public int UserId { get; set; }

    public string? WarehouseLocation { get; set; }

    public string? CertificationLevel { get; set; }

    public virtual ICollection<GoodsReceiptNote> GoodsReceiptNotes { get; set; } = new List<GoodsReceiptNote>();

    public virtual ICollection<PublishRequest> PublishRequests { get; set; } = new List<PublishRequest>();

    public virtual User User { get; set; } = null!;
}
