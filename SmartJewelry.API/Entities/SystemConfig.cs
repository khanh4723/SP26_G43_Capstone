using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class SystemConfig
{
    public int ConfigId { get; set; }

    public string ConfigKey { get; set; } = null!;

    public string ConfigValue { get; set; } = null!;

    public string ConfigType { get; set; } = null!;

    public string? Description { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Admin? UpdatedByNavigation { get; set; }
}
