namespace SmartJewelry.API.Entities;

public class Admin
{
    public int AdminId { get; set; }
    public int UserId { get; set; }
    public string PermissionLevel { get; set; } = "admin";

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<GoldRate> GoldRates { get; set; } = new List<GoldRate>();
    public virtual ICollection<SystemConfig> SystemConfigs { get; set; } = new List<SystemConfig>();
}
