// WebApplication/Models/Entities/Notification.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Outbound email/SMS notification queue. Every notification in the system —
/// OTP codes, order confirmations, payment alerts, delivery updates, stock alerts —
/// is written here as a Pending row before dispatch.
/// <para>
/// <b>Queue-only pattern:</b> Application code (services, background jobs) only
/// INSERTS rows via <c>INotificationService.QueueAsync</c>. Rows are never
/// dispatched directly by application code. Dispatch is handled exclusively by
/// <c>DeliveryStatusPollJob</c> (or a dedicated notification dispatcher job)
/// which polls <c>vw_PendingNotifications</c> — filtered to
/// Status = 'Pending' AND RetryCount &lt; 3.
/// </para>
/// <para>
/// <b>Retry policy:</b> The background dispatcher increments
/// <see cref="RetryCount"/> on each failed dispatch attempt.
/// Once <see cref="RetryCount"/> reaches 3, the row is excluded from
/// <c>vw_PendingNotifications</c> and no further dispatch is attempted.
/// <see cref="FailureReason"/> should be set on the final failed attempt.
/// </para>
/// <para>
/// <b>Notification types:</b> Use <see cref="NotifTypes"/> constants instead
/// of magic strings for <see cref="NotifType"/>.
/// </para>
/// </summary>
public sealed class Notification
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int NotificationId { get; set; }

    /// <summary>FK to the recipient user. Required — all notifications target an existing user.</summary>
    public int UserId { get; set; }

    /// <summary>
    /// FK to the order this notification relates to.
    /// NULL for non-order notifications (e.g. OTP, welcome email, stock alerts).
    /// </summary>
    public int? OrderId { get; set; }

    /// <summary>
    /// FK to the support ticket this notification relates to.
    /// NULL for non-support notifications.
    /// </summary>
    public int? TicketId { get; set; }

    /// <summary>
    /// Delivery channel for this notification.
    /// Constrained by CK_Notif_Channel: Email or SMS.
    /// Use <see cref="NotifChannels"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Channel { get; set; } = string.Empty;

    /// <summary>
    /// The type of notification. Constrained by CK_Notif_Type to 17 valid values.
    /// Use <see cref="NotifTypes"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string NotifType { get; set; } = string.Empty;

    /// <summary>
    /// The destination address for this notification.
    /// Email address for Channel = Email; phone number for Channel = SMS.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Recipient { get; set; } = string.Empty;

    /// <summary>
    /// Email subject line. NULL for SMS notifications.
    /// </summary>
    [MaxLength(255)]
    public string? Subject { get; set; }

    /// <summary>The full message body for this notification.</summary>
    public string? Body { get; set; }

    /// <summary>
    /// Current dispatch status of this notification.
    /// Constrained by CK_Notif_Status: Pending, Sent, Failed.
    /// Use <see cref="NotifStatuses"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = NotifStatuses.Pending;

    /// <summary>
    /// Number of dispatch attempts made so far.
    /// The background dispatcher stops retrying when this reaches 3.
    /// Filtered out of <c>vw_PendingNotifications</c> when RetryCount >= 3.
    /// </summary>
    public int RetryCount { get; set; } = 0;

    /// <summary>
    /// UTC timestamp when this notification was successfully dispatched.
    /// NULL until the notification reaches Sent status.
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// Description of why the last dispatch attempt failed.
    /// Set by the background dispatcher on failure.
    /// NULL for successfully sent or not-yet-attempted notifications.
    /// </summary>
    [MaxLength(500)]
    public string? FailureReason { get; set; }

    /// <summary>UTC timestamp when this notification row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Whether the customer has seen this notification in the UI.</summary>
    public bool IsRead { get; set; } = false;

    /// <summary>UTC timestamp when the notification was marked as read. NULL until read.</summary>
    public DateTime? ReadAt { get; set; }

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The recipient user.</summary>
    public User? User { get; set; }

    /// <summary>
    /// The order this notification relates to.
    /// NULL for non-order notifications.
    /// </summary>
    public Order? Order { get; set; }

    /// <summary>
    /// The support ticket this notification relates to.
    /// NULL for non-support notifications.
    /// </summary>
    public SupportTicket? Ticket { get; set; }
}

/// <summary>
/// Compile-time constants for all valid notification channel values.
/// Mirrors the CK_Notif_Channel CHECK constraint in the database.
/// </summary>
public static class NotifChannels
{
    /// <summary>Email notification.</summary>
    public const string Email = "Email";

    /// <summary>SMS text message notification.</summary>
    public const string SMS = "SMS";

    /// <summary>In-app website notification.</summary>
    public const string InApp = "InApp";
}

/// <summary>
/// Compile-time constants for all valid notification status values.
/// Mirrors the CK_Notif_Status CHECK constraint in the database.
/// </summary>
public static class NotifStatuses
{
    /// <summary>Notification is queued and awaiting dispatch.</summary>
    public const string Pending = "Pending";

    /// <summary>Notification was successfully dispatched.</summary>
    public const string Sent = "Sent";

    /// <summary>All dispatch attempts failed. RetryCount has reached 3.</summary>
    public const string Failed = "Failed";
}

/// <summary>
/// Compile-time constants for all valid notification type values.
/// Mirrors the CK_Notif_Type CHECK constraint in the database (17 values).
/// Use these instead of magic strings in all calls to
/// <c>INotificationService.QueueAsync</c>.
/// <para>
/// <b>OTP codes are not handled here.</b> OTP lifecycle is managed
/// exclusively through the <c>OTPVerification</c> table.
/// </para>
/// </summary>
public static class NotifTypes
{
    /// <summary>Welcome email sent after successful registration.</summary>
    public const string WelcomeEmail = "WelcomeEmail";

    /// <summary>Order confirmation sent after a successful order is placed.</summary>
    public const string OrderConfirmation = "OrderConfirmation";

    /// <summary>Payment received confirmation sent to the customer.</summary>
    public const string PaymentReceived = "PaymentReceived";

    /// <summary>Bank transfer payment proof was rejected by admin.</summary>
    public const string PaymentRejected = "PaymentRejected";

    /// <summary>Order placed on hold due to payment verification timeout.</summary>
    public const string PaymentHeld = "PaymentHeld";

    /// <summary>Delivery tracking update — status or ETA changed.</summary>
    public const string TrackingUpdate = "TrackingUpdate";

    /// <summary>Store pickup order is ready for collection.</summary>
    public const string ReadyForPickup = "ReadyForPickup";

    /// <summary>Pickup window is about to expire or has expired.</summary>
    public const string PickupExpiry = "PickupExpiry";

    /// <summary>Delivery has been flagged as delayed with a new ETA.</summary>
    public const string DeliveryDelay = "DeliveryDelay";

    /// <summary>Order has been successfully delivered.</summary>
    public const string DeliveryConfirmation = "DeliveryConfirmation";

    /// <summary>A wishlisted product is back in stock.</summary>
    public const string WishlistRestock = "WishlistRestock";

    /// <summary>Support ticket has been created.</summary>
    public const string SupportTicketCreated = "SupportTicketCreated";

    /// <summary>A reply has been posted on a support ticket.</summary>
    public const string SupportTicketReply = "SupportTicketReply";

    /// <summary>Support ticket has been resolved.</summary>
    public const string SupportTicketResolved = "SupportTicketResolved";

    /// <summary>A product variant's stock has dropped below its reorder threshold.</summary>
    public const string LowStockAlert = "LowStockAlert";

    /// <summary>An online order has been pending for more than 24 hours.</summary>
    public const string PendingOrderAlert = "PendingOrderAlert";

    /// <summary>A voucher has been assigned to a customer by an admin.</summary>
    public const string VoucherAssigned = "VoucherAssigned";
}