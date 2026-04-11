// WebApplication/BusinessLogic/Interfaces/INotificationService.cs

namespace WebApplication.BusinessLogic.Interfaces;

/// <summary>
/// Contract for queuing outbound email and SMS notifications.
/// <para>
/// <b>Queue-only pattern:</b> Implementations of this interface insert a row
/// into the <c>Notification</c> table with <c>Status = Pending</c>.
/// They never dispatch directly to an email or SMS gateway.
/// Dispatch is handled exclusively by the background job that polls
/// <c>vw_PendingNotifications</c> (rows where RetryCount &lt; 3).
/// </para>
/// <para>
/// This interface is injected into every service and background job that
/// needs to send a notification. Use <see cref="Models.Entities.NotifTypes"/>
/// constants for the notification type and
/// <see cref="Models.Entities.NotifChannels"/> constants for
/// the channel — never pass magic strings.
/// <para>
/// <b>OTP codes are not sent through this service.</b> OTP lifecycle
/// is handled exclusively via the <c>OTPVerification</c> table.
/// </para>
/// </para>
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Queues a single outbound notification by inserting a
    /// <c>Notification</c> row with <c>Status = Pending</c> and
    /// <c>RetryCount = 0</c>.
    /// </summary>
    /// <param name="channel">
    /// Delivery channel. Use <c>NotifChannels.Email</c> or
    /// <c>NotifChannels.SMS</c> — never a raw string.
    /// </param>
    /// <param name="notifType">
    /// Notification type. Must be one of the 16 values defined in
    /// <c>NotifTypes</c> — enforced by the database CHECK constraint.
    /// </param>
    /// <param name="recipient">
    /// Destination address — email address for Email channel,
    /// phone number for SMS channel.
    /// </param>
    /// <param name="subject">
    /// Email subject line. Pass <c>null</c> for SMS notifications.
    /// </param>
    /// <param name="body">
    /// Full message body. May be plain text or HTML for emails.
    /// </param>
    /// <param name="userId">
    /// The recipient's <c>User.UserId</c>. Required — all notifications
    /// must target an existing user.
    /// </param>
    /// <param name="orderId">
    /// Optional FK to the order this notification relates to.
    /// Pass <c>null</c> for notifications not tied to an order.
    /// </param>
    /// <param name="ticketId">
    /// Optional FK to the support ticket this notification relates to.
    /// Pass <c>null</c> for notifications not tied to a ticket.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task QueueAsync(
        string channel,
        string notifType,
        string recipient,
        string? subject,
        string? body,
        int userId,
        int? orderId = null,
        int? ticketId = null,
        CancellationToken cancellationToken = default);
}