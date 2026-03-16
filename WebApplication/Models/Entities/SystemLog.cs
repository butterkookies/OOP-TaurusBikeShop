// WebApplication/Models/Entities/SystemLog.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Immutable audit trail for admin actions and background job lifecycle events.
/// Every significant state change in the system — user actions, payment events,
/// inventory changes, background job cycles — writes a row here.
/// <para>
/// <see cref="UserId"/> is NULL for automated or system-generated events
/// (e.g. background job runs, trigger-fired actions).
/// </para>
/// <para>
/// Rows in this table must never be updated or deleted — they are an append-only
/// audit log. Use <see cref="SystemLogEvents"/> constants for the
/// <see cref="EventType"/> value instead of magic strings.
/// </para>
/// </summary>
public sealed class SystemLog
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int SystemLogId { get; set; }

    /// <summary>
    /// FK to the user who triggered this event.
    /// NULL for automated events such as background job runs or
    /// database trigger-fired actions.
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// Categorised event type. Constrained by CK_SystemLog_Event to 22 valid values.
    /// Use <see cref="SystemLogEvents"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string EventType { get; set; } = string.Empty;

    /// <summary>
    /// Free-text detail describing what happened. Optional but strongly recommended
    /// for non-trivial events — include relevant IDs and state transitions.
    /// </summary>
    [MaxLength(1000)]
    public string? EventDescription { get; set; }

    /// <summary>UTC timestamp when this log entry was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>
    /// The user who triggered this event.
    /// NULL for automated/system events.
    /// </summary>
    public User? User { get; set; }
}

/// <summary>
/// Compile-time constants for all valid system log event type values.
/// Mirrors the CK_SystemLog_Event CHECK constraint in the database (22 values).
/// Use these instead of magic strings when writing to SystemLog.
/// </summary>
public static class SystemLogEvents
{
    // Authentication
    /// <summary>User successfully logged in.</summary>
    public const string Login = "Login";

    /// <summary>User logged out.</summary>
    public const string Logout = "Logout";

    /// <summary>Unauthorized access attempt was blocked.</summary>
    public const string AccessDenied = "AccessDenied";

    // User management
    /// <summary>A new user account was created.</summary>
    public const string UserCreated = "UserCreated";

    // Product
    /// <summary>A product record was created or updated.</summary>
    public const string ProductUpdate = "ProductUpdate";

    // Vouchers
    /// <summary>A new voucher was created.</summary>
    public const string VoucherCreated = "VoucherCreated";

    // Orders
    /// <summary>An order's status was changed.</summary>
    public const string OrderStatusChange = "OrderStatusChange";

    // Payments
    /// <summary>A payment was initiated or recorded.</summary>
    public const string PaymentProcessed = "PaymentProcessed";

    /// <summary>A bank transfer payment was approved by an admin.</summary>
    public const string PaymentVerified = "PaymentVerified";

    /// <summary>A bank transfer payment was rejected by an admin.</summary>
    public const string PaymentRejected = "PaymentRejected";

    /// <summary>A payment verification deadline was exceeded — order placed on hold.</summary>
    public const string PaymentTimeout = "PaymentTimeout";

    // Inventory
    /// <summary>A manual stock adjustment was made.</summary>
    public const string InventoryAdjustment = "InventoryAdjustment";

    /// <summary>Inventory was synchronised between POS and online channels.</summary>
    public const string InventorySync = "InventorySync";

    /// <summary>A product variant's stock dropped below its reorder threshold.</summary>
    public const string LowStockTriggered = "LowStockTriggered";

    // Delivery
    /// <summary>A delivery status poll was completed by the background job.</summary>
    public const string DeliveryStatusPoll = "DeliveryStatusPoll";

    /// <summary>A delivery was flagged as delayed.</summary>
    public const string DeliveryDelayed = "DeliveryDelayed";

    /// <summary>A delivery attempt failed.</summary>
    public const string DeliveryFailed = "DeliveryFailed";

    // Background jobs
    /// <summary>A background job cycle started.</summary>
    public const string BackgroundJobStart = "BackgroundJobStart";

    /// <summary>A background job cycle completed successfully.</summary>
    public const string BackgroundJobComplete = "BackgroundJobComplete";

    /// <summary>A background job cycle encountered an error.</summary>
    public const string BackgroundJobError = "BackgroundJobError";

    // Support
    /// <summary>A support ticket was created.</summary>
    public const string SupportTicketCreated = "SupportTicketCreated";

    /// <summary>A support ticket was resolved.</summary>
    public const string SupportTicketResolved = "SupportTicketResolved";
}