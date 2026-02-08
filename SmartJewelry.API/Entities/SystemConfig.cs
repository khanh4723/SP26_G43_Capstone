namespace SmartJewelry.API.Entities;

public class SystemConfig
{
    public int ConfigId { get; set; }
    public string ConfigKey { get; set; } = string.Empty;
    public string ConfigValue { get; set; } = string.Empty;
    public string ConfigType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Admin? Updater { get; set; }
}
