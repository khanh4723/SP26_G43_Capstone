namespace SmartJewelry.API.Entities;

public class Review
{
    public int ReviewId { get; set; }
    public int ProductId { get; set; }
    public int CustomerId { get; set; }
    public int? OrderId { get; set; }
    public int RatingScore { get; set; }
    public string? ReviewTitle { get; set; }
    public string? ReviewText { get; set; }
    public string? ReviewImages { get; set; } // JSON
    public int HelpfulVotes { get; set; } = 0;
    public int UnhelpfulVotes { get; set; } = 0;
    public bool IsVerifiedPurchase { get; set; } = false;
    public string ReviewStatus { get; set; } = "pending";
    public int? ReviewedBy { get; set; }
    public DateTime ReviewTime { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Product Product { get; set; } = null!;
    public virtual Customer Customer { get; set; } = null!;
    public virtual Order? Order { get; set; }
    public virtual User? Reviewer { get; set; }
}
