using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class SalesConfig
{
    public int ConfigId { get; set; }

    public int SalesStaffId { get; set; }

    public string? Specialties { get; set; }

    public int? MaxActiveTickets { get; set; }

    public int? CurrentActiveTickets { get; set; }

    public string? ShiftSchedule { get; set; }

    public bool? IsOnline { get; set; }

    public DateTime? LastOnlineAt { get; set; }

    public string? PerformanceKpi { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual SalesStaff SalesStaff { get; set; } = null!;
}
