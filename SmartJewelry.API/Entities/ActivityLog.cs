namespace SmartJewelry.API.Entities;

public class ActivityLog
{
    public int LogId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public int? EntityId { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public int? UserId { get; set; }
    public DateTime ActionTimestamp { get; set; } = DateTime.UtcNow;
    public string? OldValuesJson { get; set; }
    public string? NewValuesJson { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }

    // Navigation properties
    public virtual User? User { get; set; }
}
