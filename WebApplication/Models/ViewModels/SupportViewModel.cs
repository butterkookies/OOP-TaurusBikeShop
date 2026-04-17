// WebApplication/Models/ViewModels/SupportViewModel.cs

using System.ComponentModel.DataAnnotations;
using WebApplication.Models.Entities;

namespace WebApplication.Models.ViewModels;

// ─────────────────────────────────────────────────────────────────────────────
// List page
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// A single row in the customer's support ticket list page.
/// </summary>
public sealed class SupportTicketListItemViewModel
{
    /// <summary>Ticket primary key.</summary>
    public int TicketId { get; set; }

    /// <summary>Short category display label.</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>Brief subject line.</summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>Current ticket status (Open, InProgress, AwaitingResponse, Resolved, Closed).</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Related order number, or null for general inquiries.</summary>
    public string? OrderNumber { get; set; }

    /// <summary>UTC timestamp when the ticket was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>UTC timestamp of the last update. Null when no update has occurred.</summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// View model for the support ticket list page (SupportList.cshtml).
/// </summary>
public sealed class SupportListViewModel
{
    /// <summary>All tickets for the authenticated user, most recent first.</summary>
    public IReadOnlyList<SupportTicketListItemViewModel> Tickets { get; set; } = [];
}

// ─────────────────────────────────────────────────────────────────────────────
// Detail page
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// A single reply entry in the ticket thread display.
/// </summary>
public sealed class SupportReplyViewModel
{
    /// <summary>Reply primary key.</summary>
    public long ReplyId { get; set; }

    /// <summary>
    /// Display name of the reply author.
    /// Shown as "Support Team" for admin replies, or the customer's first name otherwise.
    /// </summary>
    public string AuthorName { get; set; } = string.Empty;

    /// <summary><c>true</c> when the reply was written by an admin or staff member.</summary>
    public bool IsAdminReply { get; set; }

    /// <summary>Alias for <see cref="IsAdminReply"/> used by Razor views.</summary>
    public bool IsStaff => IsAdminReply;

    /// <summary>The full text body of this reply.</summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>Public GCS URL of any attachment on this reply. Null when none.</summary>
    public string? AttachmentUrl { get; set; }

    /// <summary>UTC timestamp when this reply was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Formatted created-at string for display.</summary>
    public string FormattedCreatedAt => CreatedAt.ToString("MMM dd, yyyy h:mm tt");
}

/// <summary>
/// View model for the support ticket detail page (SupportDetail.cshtml).
/// </summary>
public sealed class SupportTicketDetailViewModel
{
    /// <summary>Ticket primary key.</summary>
    public int TicketId { get; set; }

    /// <summary>Category name from <c>TicketCategories</c>.</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>Brief subject line.</summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>Full description supplied by the customer at creation time.</summary>
    public string? Description { get; set; }

    /// <summary>Public GCS URL of the original attachment. Null when none.</summary>
    public string? AttachmentUrl { get; set; }

    /// <summary>Current ticket status.</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Related order ID, or null for general inquiries.</summary>
    public int? OrderId { get; set; }

    /// <summary>Human-readable order number (e.g. TBS-2026-00001), or null.</summary>
    public string? OrderNumber { get; set; }

    /// <summary>UTC timestamp when the ticket was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>UTC timestamp when the ticket was resolved. Null until resolved.</summary>
    public DateTime? ResolvedAt { get; set; }

    /// <summary>All replies in chronological order (oldest first).</summary>
    public IReadOnlyList<SupportReplyViewModel> Replies { get; set; } = [];
}

// ─────────────────────────────────────────────────────────────────────────────
// Unified ticket view model — used by list, card, and detail views
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Unified view model for a support ticket. Used by SupportList.cshtml,
/// _SupportTicketCard.cshtml, and SupportDetail.cshtml.
/// Detail-only properties (<see cref="Replies"/>, <see cref="Description"/>,
/// <see cref="AttachmentUrl"/>) are left at defaults when used in the list context.
/// </summary>
public sealed class SupportTicketViewModel
{
    /// <summary>Ticket primary key.</summary>
    public int TicketId { get; set; }

    /// <summary>Short category display label.</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>Brief subject line.</summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>Full description supplied by the customer at creation time.</summary>
    public string? Description { get; set; }

    /// <summary>Public GCS URL of the original attachment. Null when none.</summary>
    public string? AttachmentUrl { get; set; }

    /// <summary>Current ticket status (Open, InProgress, AwaitingResponse, Resolved, Closed).</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Related order ID, or null for general inquiries.</summary>
    public int? OrderId { get; set; }

    /// <summary>Human-readable order number (e.g. TBS-2026-00001), or null.</summary>
    public string? OrderNumber { get; set; }

    /// <summary>UTC timestamp when the ticket was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>UTC timestamp of the last update. Null when no update has occurred.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>UTC timestamp when the ticket was resolved. Null until resolved.</summary>
    public DateTime? ResolvedAt { get; set; }

    /// <summary>Formatted created-at string for display.</summary>
    public string FormattedCreatedAt => CreatedAt.ToString("MMM dd, yyyy");

    /// <summary><c>true</c> when the ticket can still accept customer replies.</summary>
    public bool CanReply => Status is not (TicketStatuses.Resolved or TicketStatuses.Closed);

    /// <summary>All replies in chronological order. Empty for list context.</summary>
    public IReadOnlyList<SupportReplyViewModel> Replies { get; set; } = [];
}

// ─────────────────────────────────────────────────────────────────────────────
// Create page
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// View model for the Create Support Ticket page (SupportCreate.cshtml).
/// Carries both the form field values (POST) and reference data for the form (GET).
/// </summary>
public sealed class SupportCreateViewModel
{
    [Required(ErrorMessage = "Please select a category.")]
    public string? Category { get; set; }

    [Required(ErrorMessage = "Please provide a subject.")]
    [MaxLength(200, ErrorMessage = "Subject must be 200 characters or fewer.")]
    public string? Subject { get; set; }

    [MaxLength(5000, ErrorMessage = "Description must be 5 000 characters or fewer.")]
    public string? Description { get; set; }

    public int? OrderId { get; set; }

    /// <summary>All valid ticket categories for the category drop-down.</summary>
    public IReadOnlyList<string> AvailableCategories { get; set; } =
    [
        TicketCategories.DamagedItem,
        TicketCategories.WrongItem,
        TicketCategories.DeliveryIssue,
        TicketCategories.PaymentIssue,
        TicketCategories.ProductInquiry,
        TicketCategories.ReturnRefund,
        TicketCategories.General
    ];

    public string? OrderNumber { get; set; }
}
