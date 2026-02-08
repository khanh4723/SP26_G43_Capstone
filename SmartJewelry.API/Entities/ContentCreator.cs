namespace SmartJewelry.API.Entities;

public class ContentCreator
{
    public int ContentCreatorId { get; set; }
    public int UserId { get; set; }
    public string? SpecialtyArea { get; set; }
    public int ContentCount { get; set; } = 0;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Collection> Collections { get; set; } = new List<Collection>();
    public virtual ICollection<Content> Contents { get; set; } = new List<Content>();
    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}
