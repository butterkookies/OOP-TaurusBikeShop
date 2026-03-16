// WebApplication/Models/Entities/SupportTicket.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents a customer support ticket. Can be raised by a customer via the
/// WebApplication, by an admin via AdminSystem, or automatically by the system.
/// <para>
/// <b>Web vs Admin responsibilities:</b>
/// The WebApplication creates tickets and allows customers to read their own.
/// The AdminSystem handles replies, task assignment, status updates, and resolution.
/// </para>
/// <para>
/// <b>Ticket status flow:</b>
/// Open → InProgress → AwaitingResponse → Resolved | Closed.
/// Use <see cref="TicketStatuses"/> constants instead of magic strings.
/// </para>
/// <para>
/// <b>Attachments:</b> File attachments are stored in Google Cloud Storage.
/// Only the GCS URL and path metadata are stored here — the binary file lives in GCS.
/// </para>
/// </summary>
public sealed class SupportTicket
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int TicketId { get; set; }

    /// <summary>FK to the customer who raised this ticket.</summary>
    public int UserId { get; set; }

    /// <summary>
    /// FK to the order this ticket relates to.
    /// NULL for general inquiries not tied to a specific order.
    /// </summary>
    public int? OrderId { get; set; }

    /// <summary>
    /// How this ticket was created. Constrained by CK_Ticket_Source:
    /// Customer, Admin, or System.
    /// Use <see cref="TicketSources"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string TicketSource { get; set; } = TicketSources.Customer;

    /// <summary>
    /// Category of the issue. Constrained by CK_Ticket_Category to 7 values.
    /// Use <see cref="TicketCategories"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string TicketCategory { get; set; } = string.Empty;

    /// <summary>Brief subject line for the ticket.</summary>
    [Required]
    [MaxLength(200)]
    public string Subject { get; set; } = string.Empty;

    /// <summary>Full description of the issue provided by the customer.</summary>
    public string? Description { get; set; }

    /// <summary>
    /// GCS public URL of an attached file (e.g. photo of damaged item).
    /// NULL when no attachment was provided.
    /// </summary>
    [MaxLength(1000)]
    public string? AttachmentUrl { get; set; }

    /// <summary>GCS bucket name for the attachment. NULL when no attachment.</summary>
    [MaxLength(200)]
    public string? AttachmentBucket { get; set; }

    /// <summary>GCS object path for the attachment. NULL when no attachment.</summary>
    [MaxLength(1000)]
    public string? AttachmentPath { get; set; }

    /// <summary>
    /// Current status of this ticket.
    /// Constrained by CK_Ticket_Status to 5 values.
    /// Use <see cref="TicketStatuses"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string TicketStatus { get; set; } = TicketStatuses.Open;

    /// <summary>
    /// FK to the admin or staff member assigned to handle this ticket.
    /// NULL until an admin claims or is assigned the ticket.
    /// </summary>
    public int? AssignedToUserId { get; set; }

    /// <summary>
    /// UTC timestamp when this ticket was resolved.
    /// Set by the AdminSystem when <see cref="TicketStatus"/> transitions to Resolved.
    /// NULL until resolved.
    /// </summary>
    public DateTime? ResolvedAt { get; set; }

    /// <summary>UTC timestamp when this ticket was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// UTC timestamp of the last update to this ticket record.
    /// Updated on every status change or reply.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The customer who raised this ticket.</summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// The admin or staff member assigned to this ticket.
    /// NULL until assigned.
    /// </summary>
    public User? AssignedTo { get; set; }

    /// <summary>
    /// The order this ticket relates to.
    /// NULL for general inquiries.
    /// </summary>
    public Order? Order { get; set; }

    /// <summary>
    /// All reply messages in this ticket thread, ordered by CreatedAt ascending.
    /// Cascade-deleted when the ticket is deleted.
    /// </summary>
    public ICollection<SupportTicketReply> Replies { get; set; } = new List<SupportTicketReply>();

    /// <summary>
    /// Actionable follow-up tasks created by admins for this ticket (v7.1).
    /// Cascade-deleted when the ticket is deleted.
    /// </summary>
    public ICollection<SupportTask> Tasks { get; set; } = new List<SupportTask>();

    /// <summary>
    /// Notification rows queued for this ticket
    /// (e.g. SupportTicketCreated, SupportTicketReply, SupportTicketResolved).
    /// </summary>
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}

/// <summary>
/// Compile-time constants for all valid ticket source values.
/// Mirrors the CK_Ticket_Source CHECK constraint in the database.
/// </summary>
public static class TicketSources
{
    /// <summary>Ticket was created by the customer via the WebApplication.</summary>
    public const string Customer = "Customer";

    /// <summary>Ticket was created by an admin via the AdminSystem.</summary>
    public const string Admin = "Admin";

    /// <summary>Ticket was created automatically by the system (e.g. failed delivery).</summary>
    public const string System = "System";
}

/// <summary>
/// Compile-time constants for all valid ticket category values.
/// Mirrors the CK_Ticket_Category CHECK constraint in the database.
/// </summary>
public static class TicketCategories
{
    /// <summary>Item arrived damaged.</summary>
    public const string DamagedItem = "DamagedItem";

    /// <summary>Customer received the wrong item.</summary>
    public const string WrongItem = "WrongItem";

    /// <summary>Issue with the delivery or courier.</summary>
    public const string DeliveryIssue = "DeliveryIssue";

    /// <summary>Issue with payment processing or verification.</summary>
    public const string PaymentIssue = "PaymentIssue";

    /// <summary>Question about a product.</summary>
    public const string ProductInquiry = "ProductInquiry";

    /// <summary>Request related to returns.</summary>
    public const string ReturnRefund = "ReturnRefund";

    /// <summary>General inquiry not covered by other categories.</summary>
    public const string General = "General";
}

/// <summary>
/// Compile-time constants for all valid ticket status values.
/// Mirrors the CK_Ticket_Status CHECK constraint in the database.
/// </summary>
public static class TicketStatuses
{
    /// <summary>Ticket has been created and is awaiting admin attention.</summary>
    public const string Open = "Open";

    /// <summary>An admin has started working on this ticket.</summary>
    public const string InProgress = "InProgress";

    /// <summary>Admin has replied and is waiting for the customer to respond.</summary>
    public const string AwaitingResponse = "AwaitingResponse";

    /// <summary>Issue has been resolved. ResolvedAt timestamp is set.</summary>
    public const string Resolved = "Resolved";

    /// <summary>Ticket has been closed without further action required.</summary>
    public const string Closed = "Closed";
}