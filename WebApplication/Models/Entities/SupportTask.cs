// WebApplication/Models/Entities/SupportTask.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// An actionable follow-up task linked to a support ticket (added in v7.1).
/// Tasks are distinct from replies — they have their own assignee, status
/// lifecycle, and due date. Created and managed exclusively by AdminSystem staff.
/// <para>
/// <b>Task type rule:</b> Valid TaskType values are: ShipReplacement, ArrangeReturn,
/// ContactSupplier, Other. The 'IssueRefund' value was removed in the v7.1 patch
/// because Taurus does not offer refunds. Never add IssueRefund back.
/// Use <see cref="SupportTaskTypes"/> constants instead of magic strings.
/// </para>
/// <para>
/// <b>Status lifecycle:</b> Pending → InProgress → Done | Cancelled.
/// Use <see cref="SupportTaskStatuses"/> constants instead of magic strings.
/// <see cref="CompletedAt"/> is set when status transitions to Done.
/// </para>
/// <para>
/// Cascade-deleted when the parent <see cref="SupportTicket"/> is deleted.
/// </para>
/// </summary>
public sealed class SupportTask
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int TaskId { get; set; }

    /// <summary>
    /// FK to the parent support ticket.
    /// Configured with CASCADE DELETE in AppDbContext.
    /// </summary>
    public int TicketId { get; set; }

    /// <summary>
    /// FK to the admin or staff member assigned to complete this task.
    /// NULL until an admin assigns it.
    /// </summary>
    public int? AssignedToUserId { get; set; }

    /// <summary>
    /// The type of follow-up action required.
    /// Constrained by CK_SupportTask_Type: ShipReplacement, ArrangeReturn,
    /// ContactSupplier, Other.
    /// IssueRefund is NOT a valid value — Taurus does not offer refunds.
    /// Use <see cref="SupportTaskTypes"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string TaskType { get; set; } = string.Empty;

    /// <summary>
    /// Current status of this task.
    /// Constrained by CK_SupportTask_Status: Pending, InProgress, Done, Cancelled.
    /// Use <see cref="SupportTaskStatuses"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string TaskStatus { get; set; } = SupportTaskStatuses.Pending;

    /// <summary>
    /// Optional due date for completing this task.
    /// NULL when no deadline has been set.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Optional notes describing what needs to be done or
    /// what was done to complete the task.
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>UTC timestamp when this task was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// UTC timestamp when this task was completed.
    /// Set by AdminSystem when <see cref="TaskStatus"/> transitions to Done.
    /// NULL until the task is completed.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The support ticket this task belongs to.</summary>
    public SupportTicket Ticket { get; set; } = null!;

    /// <summary>
    /// The admin or staff member assigned to this task.
    /// NULL until assigned.
    /// </summary>
    public User? AssignedTo { get; set; }
}

/// <summary>
/// Compile-time constants for all valid support task type values.
/// Mirrors the CK_SupportTask_Type CHECK constraint in the database.
/// Note: IssueRefund is NOT included — it was removed in the v7.1 patch
/// because Taurus does not offer refunds. Never add it back.
/// </summary>
public static class SupportTaskTypes
{
    /// <summary>Ship a replacement item to the customer.</summary>
    public const string ShipReplacement = "ShipReplacement";

    /// <summary>Arrange for the customer to return the item.</summary>
    public const string ArrangeReturn = "ArrangeReturn";

    /// <summary>Contact the supplier about a product or delivery issue.</summary>
    public const string ContactSupplier = "ContactSupplier";

    /// <summary>Any other follow-up action not covered by the above types.</summary>
    public const string Other = "Other";
}

/// <summary>
/// Compile-time constants for all valid support task status values.
/// Mirrors the CK_SupportTask_Status CHECK constraint in the database.
/// </summary>
public static class SupportTaskStatuses
{
    /// <summary>Task has been created and is awaiting action.</summary>
    public const string Pending = "Pending";

    /// <summary>Task is currently being worked on.</summary>
    public const string InProgress = "InProgress";

    /// <summary>Task has been completed. CompletedAt timestamp is set.</summary>
    public const string Done = "Done";

    /// <summary>Task was cancelled before completion.</summary>
    public const string Cancelled = "Cancelled";
}