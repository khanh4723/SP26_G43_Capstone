using System;
using System.Collections.Generic;

namespace SmartJewelry.API.Entities;

public partial class ConsultationAudio
{
    public int AudioId { get; set; }

    public int TicketId { get; set; }

    public string AudioUrl { get; set; } = null!;

    public int? AudioDurationSeconds { get; set; }

    public int? UploadedBy { get; set; }

    public DateTime? UploadedAt { get; set; }

    public string? Transcript { get; set; }

    public string? TranscriptionStatus { get; set; }

    public DateTime? TranscribedAt { get; set; }

    public string? Extraction { get; set; }

    public string? ExtractionStatus { get; set; }

    public DateTime? ExtractedAt { get; set; }

    public bool? ReviewedBySales { get; set; }

    public string? SalesConfirmedData { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public virtual ConsultationTicket Ticket { get; set; } = null!;

    public virtual SalesStaff? UploadedByNavigation { get; set; }
}
