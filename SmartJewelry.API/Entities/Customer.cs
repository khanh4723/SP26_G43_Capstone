namespace SmartJewelry.API.Entities;

public class Customer
{
    public int CustomerId { get; set; }
    public int UserId { get; set; }
    public int LoyaltyPoints { get; set; } = 0;
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string CustomerTier { get; set; } = "bronze";

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual CustomerProfile? CustomerProfile { get; set; }
    public virtual ICollection<LoyaltyTransaction> LoyaltyTransactions { get; set; } = new List<LoyaltyTransaction>();
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<ConsultationTicket> ConsultationTickets { get; set; } = new List<ConsultationTicket>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
