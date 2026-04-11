// WebApplication/BusinessLogic/Services/NotificationService.cs

using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BusinessLogic.Services;

/// <summary>
/// Implements <see cref="INotificationService"/> by inserting rows into the
/// <c>Notification</c> table with <c>Status = Pending</c>.
/// <para>
/// <b>This service never dispatches email or SMS directly.</b>
/// It is a pure write-to-queue operation. The background job that polls
/// <c>vw_PendingNotifications</c> (Status = 'Pending' AND RetryCount &lt; 3)
/// handles all actual dispatch to the email/SMS gateway.
/// </para>
/// <para>
/// <b>Retry policy:</b> The background dispatcher increments
/// <c>Notification.RetryCount</c> on each failed attempt and sets
/// <c>Status = Failed</c> once <c>RetryCount</c> reaches 3.
/// This service always inserts with <c>RetryCount = 0</c>.
/// </para>
/// </summary>
public sealed class NotificationService : INotificationService
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initialises the service with the EF Core database context.
    /// </summary>
    /// <param name="context">The application's EF Core database context.</param>
    public NotificationService(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="channel"/>, <paramref name="notifType"/>,
    /// or <paramref name="recipient"/> are null or whitespace.
    /// </exception>
    public async Task QueueAsync(
        string channel,
        string notifType,
        string recipient,
        string? subject,
        string? body,
        int userId,
        int? orderId = null,
        int? ticketId = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(channel))
            throw new ArgumentException("Channel must not be null or whitespace.", nameof(channel));

        if (string.IsNullOrWhiteSpace(notifType))
            throw new ArgumentException("NotifType must not be null or whitespace.", nameof(notifType));

        if (string.IsNullOrWhiteSpace(recipient))
            throw new ArgumentException("Recipient must not be null or whitespace.", nameof(recipient));

        Notification notification = new()
        {
            UserId = userId,
            OrderId = orderId,
            TicketId = ticketId,
            Channel = channel,
            NotifType = notifType,
            Recipient = recipient.Trim(),
            Subject = subject,
            Body = body,
            Status = NotifStatuses.Pending,
            RetryCount = 0,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}