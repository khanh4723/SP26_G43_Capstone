using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class Review
{
    public int ReviewId { get; set; }

    public int ProductId { get; set; }

    public int CustomerId { get; set; }

    public int? OrderId { get; set; }

    public int RatingScore { get; set; }

    public string? ReviewTitle { get; set; }

    public string? ReviewText { get; set; }

    public string? ReviewImages { get; set; }

    public int? HelpfulVotes { get; set; }

    public int? UnhelpfulVotes { get; set; }

    public bool? IsVerifiedPurchase { get; set; }

    public string? ReviewStatus { get; set; }

    public int? ReviewedBy { get; set; }

    public DateTime? ReviewTime { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Order? Order { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User? ReviewedByNavigation { get; set; }
}
