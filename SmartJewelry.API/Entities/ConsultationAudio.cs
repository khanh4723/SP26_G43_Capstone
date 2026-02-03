namespace SmartJewelry.API.Entities;

public class ConsultationAudio
{
    public int AudioId { get; set; }
    public int TicketId { get; set; }
    public string AudioUrl { get; set; } = string.Empty;
    public int? AudioDurationSeconds { get; set; }
    public int? UploadedBy { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public string? Transcript { get; set; }
    public string TranscriptionStatus { get; set; } = "pending";
    public DateTime? TranscribedAt { get; set; }
    public string? Extraction { get; set; } // JSON
    public string ExtractionStatus { get; set; } = "pending";
    public DateTime? ExtractedAt { get; set; }
    public bool ReviewedBySales { get; set; } = false;
    public string? SalesConfirmedData { get; set; } // JSON
    public DateTime? ReviewedAt { get; set; }

    // Navigation properties
    public virtual ConsultationTicket Ticket { get; set; } = null!;
    public virtual SalesStaff? Uploader { get; set; }
}
