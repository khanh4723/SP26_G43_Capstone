namespace SmartJewelry.API.Entities;

public class SalesStaff
{
    public int SalesStaffId { get; set; }
    public int UserId { get; set; }
    public string? DepartmentName { get; set; }
    public decimal? SalesTarget { get; set; }
    public decimal? CommissionRate { get; set; }
    public DateTime? HireDate { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual SalesConfig? SalesConfig { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<ConsultationTicket> ConsultationTickets { get; set; } = new List<ConsultationTicket>();
    public virtual ICollection<ConsultationAudio> ConsultationAudios { get; set; } = new List<ConsultationAudio>();
}
