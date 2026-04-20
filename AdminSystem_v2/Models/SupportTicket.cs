namespace AdminSystem_v2.Models
{
    public class SupportTicket
    {
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public int? OrderId { get; set; }
        public string TicketSource { get; set; } = string.Empty;
        public string TicketCategory { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? AttachmentUrl { get; set; }
        public string? AttachmentBucket { get; set; }
        public string? AttachmentPath { get; set; }
        public string TicketStatus { get; set; } = string.Empty;
        public int? AssignedToUserId { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        // Added properties to hold joined data for the UI
        public string? UserFullName { get; set; }
        public string? UserEmail { get; set; }
        public string? AssignedToFullName { get; set; }
    }

    public class SupportTicketReply
    {
        public long ReplyId { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public bool IsAdminReply { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? AttachmentUrl { get; set; }
        public string? AttachmentBucket { get; set; }
        public string? AttachmentPath { get; set; }
        public DateTime CreatedAt { get; set; }

        // UI Helpers
        public string? UserFullName { get; set; }
    }
}
