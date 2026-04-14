// WebApplication/BusinessLogic/Services/NotificationService.cs

using Microsoft.EntityFrameworkCore;
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

    /// <inheritdoc/>
    public async Task<(IReadOnlyList<Notification> Items, int TotalCount)>
        GetNotificationsForUserAsync(
            int userId, int page, int pageSize,
            CancellationToken cancellationToken = default)
    {
        IQueryable<Notification> query = _context.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt);

        int totalCount = await query.CountAsync(cancellationToken);

        IReadOnlyList<Notification> items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    /// <inheritdoc/>
    public async Task<int> GetUnreadCountAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .AsNoTracking()
            .CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task MarkAsReadAsync(
        int notificationId, int userId,
        CancellationToken cancellationToken = default)
    {
        Notification? notification = await _context.Notifications
            .FirstOrDefaultAsync(
                n => n.NotificationId == notificationId && n.UserId == userId,
                cancellationToken);

        if (notification is null || notification.IsRead) return;

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task MarkAllAsReadAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(n => n.IsRead, true)
                    .SetProperty(n => n.ReadAt, DateTime.UtcNow),
                cancellationToken);
    }
}
