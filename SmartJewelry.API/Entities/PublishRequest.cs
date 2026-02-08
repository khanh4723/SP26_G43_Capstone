using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class PublishRequest
{
    public int RequestId { get; set; }

    public string RequestNumber { get; set; } = null!;

    public int InventoryManagerId { get; set; }

    public int? StoreManagerId { get; set; }

    public string? RequestStatus { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public string? Items { get; set; }

    public string? Notes { get; set; }

    public string? ReviewerNotes { get; set; }

    public virtual InventoryManager InventoryManager { get; set; } = null!;

    public virtual StoreManager? StoreManager { get; set; }
}
