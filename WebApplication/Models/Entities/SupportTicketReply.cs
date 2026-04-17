// WebApplication/Models/Entities/SupportTicketReply.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// A single reply message within a support ticket thread.
/// Both customers and admin/staff can reply to tickets.
/// <see cref="IsAdminReply"/> distinguishes between customer and staff messages.
/// <para>
/// <b>Responsibilities by application:</b>
/// <list type="bullet">
///   <item>WebApplication — customers read replies via the ticket detail page.</item>
///   <item>AdminSystem — staff create replies via <c>SupportService.ReplyToTicketAsync</c>,
///     which also updates <c>SupportTicket.TicketStatus</c> to AwaitingResponse
///     and queues a SupportTicketReply notification to the customer.</item>
/// </list>
/// </para>
/// <para>
/// Cascade-deleted when the parent <see cref="SupportTicket"/> is deleted.
/// </para>
/// </summary>
public sealed class SupportTicketReply
{
    /// <summary>Primary key — auto-increment identity (bigint).</summary>
    public long ReplyId { get; set; }

    /// <summary>
    /// FK to the parent support ticket.
    /// Configured with CASCADE DELETE in AppDbContext.
    /// </summary>
    public int TicketId { get; set; }

    /// <summary>FK to the user who authored this reply (customer or staff).</summary>
    public int UserId { get; set; }

    /// <summary>
    /// <c>true</c> when this reply was written by an admin or staff member.
    /// <c>false</c> when written by the customer.
    /// Used by the UI to visually differentiate staff and customer messages
    /// in the reply thread.
    /// </summary>
    public bool IsAdminReply { get; set; } = false;

    /// <summary>The full text body of this reply message.</summary>
    [Required]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// GCS public URL of an optional file attached to this reply.
    /// NULL when no attachment was included.
    /// </summary>
    [MaxLength(1000)]
    public string? AttachmentUrl { get; set; }

    /// <summary>GCS bucket for the attachment. NULL when no attachment.</summary>
    [MaxLength(200)]
    public string? AttachmentBucket { get; set; }

    /// <summary>GCS object path for the attachment. NULL when no attachment.</summary>
    [MaxLength(1000)]
    public string? AttachmentPath { get; set; }

    /// <summary>UTC timestamp when this reply was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The support ticket this reply belongs to.</summary>
    public SupportTicket Ticket { get; set; } = null!;

    /// <summary>The user (customer or staff) who authored this reply.</summary>
    public User User { get; set; } = null!;
}