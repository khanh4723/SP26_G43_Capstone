using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class EmailVerificationToken
{
    public int TokenId { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    public DateTime? UsedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? IpAddress { get; set; }

    public virtual User User { get; set; } = null!;
}
