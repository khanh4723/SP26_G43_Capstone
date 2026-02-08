using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class ConsultationTicket
{
    public int TicketId { get; set; }

    public string TicketNumber { get; set; } = null!;

    public int CustomerId { get; set; }

    public int? SalesStaffId { get; set; }

    public string? Category { get; set; }

    public string? Status { get; set; }

    public string? Priority { get; set; }

    public string? Subject { get; set; }

    public string? Description { get; set; }

    public DateTime? AssignedAt { get; set; }

    public bool? AutoAssigned { get; set; }

    public string? Notes { get; set; }

    public string? AssignmentHistory { get; set; }

    public DateTime? CreationTime { get; set; }

    public DateTime? LastUpdated { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public virtual ICollection<ConsultationAudio> ConsultationAudios { get; set; } = new List<ConsultationAudio>();

    public virtual Customer Customer { get; set; } = null!;

    public virtual SalesStaff? SalesStaff { get; set; }
}
