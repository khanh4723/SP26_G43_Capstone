namespace SmartJewelry.API.Entities;

public class CustomOrderDetail
{
    public int CustomDetailId { get; set; }
    public int OrderId { get; set; }
    public int? ConsultationTicketId { get; set; }
    public int? SelectedGemstoneId { get; set; }
    public int? SelectedMountingId { get; set; }
    public string? Modifications { get; set; } // JSON
    public string? WorkflowStages { get; set; } // JSON
    public DateTime? EstimatedCompletionDate { get; set; }
    public DateTime? ActualCompletionDate { get; set; }

    // Navigation properties
    public virtual Order Order { get; set; } = null!;
    public virtual ConsultationTicket? ConsultationTicket { get; set; }
    public virtual Gemstone? SelectedGemstone { get; set; }
    public virtual Product? SelectedMounting { get; set; }
}
