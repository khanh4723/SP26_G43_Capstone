using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class ActivityLog
{
    public int LogId { get; set; }

    public string EntityName { get; set; } = null!;

    public int? EntityId { get; set; }

    public string ActionType { get; set; } = null!;

    public int? UserId { get; set; }

    public DateTime? ActionTimestamp { get; set; }

    public string? OldValuesJson { get; set; }

    public string? NewValuesJson { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public virtual User? User { get; set; }
}
