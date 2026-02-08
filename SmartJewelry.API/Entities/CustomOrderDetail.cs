using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class CustomOrderDetail
{
    public int CustomDetailId { get; set; }

    public int OrderId { get; set; }

    public int? ConsultationTicketId { get; set; }

    public int? SelectedGemstoneId { get; set; }

    public int? SelectedMountingId { get; set; }

    public string? Modifications { get; set; }

    public string? WorkflowStages { get; set; }

    public DateOnly? EstimatedCompletionDate { get; set; }

    public DateOnly? ActualCompletionDate { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Gemstone? SelectedGemstone { get; set; }

    public virtual Product? SelectedMounting { get; set; }
}
