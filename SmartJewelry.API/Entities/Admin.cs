using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class Admin
{
    public int AdminId { get; set; }

    public int UserId { get; set; }

    public string? PermissionLevel { get; set; }

    public virtual ICollection<GoldRate> GoldRates { get; set; } = new List<GoldRate>();

    public virtual ICollection<SystemConfig> SystemConfigs { get; set; } = new List<SystemConfig>();

    public virtual User User { get; set; } = null!;
}
