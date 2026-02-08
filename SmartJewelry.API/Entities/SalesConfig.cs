namespace SmartJewelry.API.Entities;

public class SalesConfig
{
    public int ConfigId { get; set; }
    public int SalesStaffId { get; set; }
    public string? Specialties { get; set; } // JSON
    public int MaxActiveTickets { get; set; } = 20;
    public int CurrentActiveTickets { get; set; } = 0;
    public string? ShiftSchedule { get; set; } // JSON
    public bool IsOnline { get; set; } = false;
    public DateTime? LastOnlineAt { get; set; }
    public string? PerformanceKpi { get; set; } // JSON
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual SalesStaff SalesStaff { get; set; } = null!;
}
