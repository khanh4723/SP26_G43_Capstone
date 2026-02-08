using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class StoreManager
{
    public int StoreManagerId { get; set; }

    public int UserId { get; set; }

    public string? ManagedDepartment { get; set; }

    public int? SupervisedStaffCount { get; set; }

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();

    public virtual ICollection<PublishRequest> PublishRequests { get; set; } = new List<PublishRequest>();

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public virtual User User { get; set; } = null!;
}
