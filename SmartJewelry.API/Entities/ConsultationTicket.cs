namespace SmartJewelry.API.Entities;

public class ConsultationTicket
{
    public int TicketId { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int? SalesStaffId { get; set; }
    public string Category { get; set; } = "general";
    public string Status { get; set; } = "open";
    public string Priority { get; set; } = "medium";
    public string? Subject { get; set; }
    public string? Description { get; set; }
    public DateTime? AssignedAt { get; set; }
    public bool AutoAssigned { get; set; } = false;
    public string? Notes { get; set; }
    public string? AssignmentHistory { get; set; } // JSON
    public DateTime CreationTime { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }

    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
    public virtual SalesStaff? SalesStaff { get; set; }
    public virtual ICollection<ConsultationAudio> ConsultationAudios { get; set; } = new List<ConsultationAudio>();
    public virtual ICollection<CustomOrderDetail> CustomOrderDetails { get; set; } = new List<CustomOrderDetail>();
}
