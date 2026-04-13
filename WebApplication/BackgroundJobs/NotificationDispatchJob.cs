// WebApplication/BackgroundJobs/NotificationDispatchJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

/// <summary>
/// Background job that polls the Notification table for pending email notifications
/// and dispatches them via <see cref="IEmailSender"/> (Gmail SMTP).
/// <para>
/// <b>Poll interval:</b> Every 30 seconds.
/// <b>Retry policy:</b> Up to 3 attempts per notification. On the 3rd failure,
/// the notification is marked as Failed with a reason.
/// </para>
/// </summary>
public sealed class NotificationDispatchJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NotificationDispatchJob> _logger;

    private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(30);
    private const int MaxRetryCount = 3;
    private const int BatchSize = 10;

    public NotificationDispatchJob(
        IServiceScopeFactory scopeFactory,
        ILogger<NotificationDispatchJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NotificationDispatchJob started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DispatchPendingAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "NotificationDispatchJob encountered an error.");
            }

            await Task.Delay(PollInterval, stoppingToken);
        }
    }

    private async Task DispatchPendingAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        IEmailSender emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

        List<Notification> pending = await context.Notifications
            .Where(n => n.Status == NotifStatuses.Pending
                     && n.RetryCount < MaxRetryCount
                     && n.Channel == NotifChannels.Email)
            .OrderBy(n => n.CreatedAt)
            .Take(BatchSize)
            .ToListAsync(cancellationToken);

        foreach (Notification notification in pending)
        {
            try
            {
                await emailSender.SendAsync(
                    notification.Recipient,
                    notification.Subject ?? "(No Subject)",
                    notification.Body ?? string.Empty,
                    cancellationToken);

                notification.Status = NotifStatuses.Sent;
                notification.SentAt = DateTime.UtcNow;

                _logger.LogInformation(
                    "Notification {Id} sent to {Recipient} ({Type}).",
                    notification.NotificationId, notification.Recipient, notification.NotifType);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                notification.RetryCount++;
                notification.FailureReason = ex.Message.Length > 500
                    ? ex.Message[..500]
                    : ex.Message;

                if (notification.RetryCount >= MaxRetryCount)
                {
                    notification.Status = NotifStatuses.Failed;
                    _logger.LogWarning(
                        "Notification {Id} failed permanently after {Retries} attempts: {Error}",
                        notification.NotificationId, MaxRetryCount, ex.Message);
                }
                else
                {
                    _logger.LogWarning(
                        "Notification {Id} attempt {Attempt} failed: {Error}",
                        notification.NotificationId, notification.RetryCount, ex.Message);
                }
            }
        }

        if (pending.Count > 0)
            await context.SaveChangesAsync(cancellationToken);
    }
}
