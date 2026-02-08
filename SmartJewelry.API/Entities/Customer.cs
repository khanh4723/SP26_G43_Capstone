using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int UserId { get; set; }

    public int? LoyaltyPoints { get; set; }

    public string? Phone { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? CustomerTier { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<ConsultationTicket> ConsultationTickets { get; set; } = new List<ConsultationTicket>();

    public virtual CustomerProfile? CustomerProfile { get; set; }

    public virtual ICollection<LoyaltyTransaction> LoyaltyTransactions { get; set; } = new List<LoyaltyTransaction>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual User User { get; set; } = null!;
}
