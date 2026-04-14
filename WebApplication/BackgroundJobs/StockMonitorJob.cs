// WebApplication/BackgroundJobs/StockMonitorJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

/// <summary>
/// Background job that runs every 15 minutes to check whether any active
/// <see cref="ProductVariant"/> has fallen below its <c>ReorderThreshold</c>.
/// <para>
/// When a variant is detected as low-stock, two notification paths are triggered:
/// <list type="bullet">
///   <item><description>
///     <b>Admin/Manager alert (<see cref="NotifTypes.LowStockAlert"/>):</b>
///     All active users holding the Admin or Manager role receive an email
///     prompting them to raise a purchase order.
///   </description></item>
///   <item><description>
///     <b>Wishlist customer nudge (<see cref="NotifTypes.WishlistRestock"/>):</b>
///     Any active customer who has the product in their wishlist and where
///     <c>StockQuantity &gt; 0</c> receives a "last chance" email urging them
///     to order before stock runs out.
///   </description></item>
/// </list>
/// </para>
/// <para>
/// <b>Cooldown / deduplication:</b> A 24-hour cooldown is enforced per variant
/// using the <c>SystemLog</c> table. If a <see cref="SystemLogEvents.LowStockTriggered"/>
/// entry already exists for the variant within the cooldown window, both
/// notifications are skipped to prevent spam on every 15-minute cycle.
/// The <c>LowStockTriggered</c> SystemLog row is written BEFORE notifications are
/// queued so that the cooldown record survives even if individual notification
/// inserts fail.
/// </para>
/// <para>
/// Flowchart references: Part 12 (INV7–INV9A), Part 14 (JOB 4 / J4A–J4B).
/// </para>
/// </summary>
public sealed class StockMonitorJob : BackgroundService
{
    // How often the job wakes and scans all active variants.
    private static readonly TimeSpan CycleInterval = TimeSpan.FromMinutes(15);

    // Minimum gap between repeated low-stock notifications for the same variant.
    // Prevents spamming admins and customers every 15 minutes when stock stays low.
    private static readonly TimeSpan AlertCooldown = TimeSpan.FromHours(24);

    private readonly IServiceScopeFactory     _scopeFactory;
    private readonly ILogger<StockMonitorJob> _logger;

    // Exponential backoff: tracks consecutive failures to avoid log flooding
    // when the database is unavailable.
    private int _consecutiveFailures;
    private static readonly TimeSpan MaxBackoff = TimeSpan.FromMinutes(2);

    public StockMonitorJob(
        IServiceScopeFactory     scopeFactory,
        ILogger<StockMonitorJob> logger)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // ExecuteAsync — hosted service entry point
    // =========================================================================

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("StockMonitorJob started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunCycleAsync(stoppingToken);
                _consecutiveFailures = 0;
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                _consecutiveFailures++;
                _logger.LogError(ex, "{Job} unhandled exception (failure #{Count}) — will retry next cycle.",
                    GetType().Name, _consecutiveFailures);
            }

            TimeSpan delay = _consecutiveFailures > 0
                ? TimeSpan.FromSeconds(Math.Min(MaxBackoff.TotalSeconds,
                    CycleInterval.TotalSeconds * Math.Pow(2, _consecutiveFailures - 1)))
                : CycleInterval;
            await Task.Delay(delay, stoppingToken).ConfigureAwait(false);
        }
        _logger.LogInformation("StockMonitorJob stopping.");
    }

    // =========================================================================
    // RunCycleAsync — one full detection + notification pass
    // =========================================================================

    private async Task RunCycleAsync(CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();

        // Both services share the same scoped lifetime so they share
        // the same AppDbContext instance — important for change tracking.
        AppDbContext         context       = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        INotificationService notifications = scope.ServiceProvider.GetRequiredService<INotificationService>();

        // ── Cycle start log (best-effort — DB may be unavailable on boot) ──────
        try
        {
            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobStart,
                EventDescription = $"{nameof(StockMonitorJob)} cycle started.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "{Job} could not write start log — DB may be unavailable.",
                nameof(StockMonitorJob));
        }

        try
        {
            // ── 1. Detect variants below their reorder threshold ───────────────
            // Only active variants whose parent product is also active are checked.
            // EF Core translates the v.Product.IsActive navigation access to a JOIN.
            var lowStockVariants = await context.ProductVariants
                .AsNoTracking()
                .Where(v => v.IsActive
                         && v.Product.IsActive
                         && v.StockQuantity < v.ReorderThreshold)
                .Select(v => new
                {
                    v.ProductVariantId,
                    v.VariantName,
                    v.StockQuantity,
                    v.ReorderThreshold,
                    v.ProductId,
                    ProductName = v.Product.Name
                })
                .ToListAsync(cancellationToken);

            // ── 2. Load admin/manager recipients once for the entire cycle ──────
            // Loaded once to avoid repeating the same query per variant.
            // Filters out walk-in placeholder users (no email) and inactive accounts.
            var adminRecipients = await context.Users
                .AsNoTracking()
                .Where(u => u.IsActive
                         && !u.IsWalkIn
                         && u.Email != null
                         && u.UserRoles.Any(ur =>
                             ur.Role.RoleName == RoleNames.Admin ||
                             ur.Role.RoleName == RoleNames.Manager))
                .Select(u => new { u.UserId, u.Email, u.FirstName })
                .ToListAsync(cancellationToken);

            DateTime cooldownStart    = DateTime.UtcNow.Subtract(AlertCooldown);
            int      notifiedVariants = 0;

            foreach (var variant in lowStockVariants)
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogWarning(
                    "StockMonitorJob: low stock — variant {VariantId} ({ProductName} / {VariantName}): " +
                    "{Stock} remaining (threshold {Threshold}).",
                    variant.ProductVariantId, variant.ProductName, variant.VariantName,
                    variant.StockQuantity, variant.ReorderThreshold);

                // ── 3. Cooldown check ─────────────────────────────────────────
                // The variantMarker is embedded in EventDescription by step 4 below.
                // Using EF.Functions.Like produces a SQL LIKE '%marker%' predicate.
                string variantMarker   = $"(VariantId {variant.ProductVariantId})";
                bool   alreadyAlerted  = await context.SystemLogs
                    .AnyAsync(l =>
                        l.EventType  == SystemLogEvents.LowStockTriggered &&
                        l.CreatedAt  > cooldownStart &&
                        EF.Functions.Like(l.EventDescription, $"%{variantMarker}%"),
                        cancellationToken);

                if (alreadyAlerted)
                {
                    _logger.LogDebug(
                        "StockMonitorJob: variant {VariantId} already alerted within {Hours}h — skipping.",
                        variant.ProductVariantId, AlertCooldown.TotalHours);
                    continue;
                }

                // ── 4. Write detection record to SystemLog ────────────────────
                // Committed BEFORE queuing notifications.  If a notification fails
                // to insert, the cooldown record still prevents spam next cycle.
                // Notifications that do insert will be retried by the dispatcher job.
                await context.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.LowStockTriggered,
                    EventDescription =
                        $"Low stock: {variant.ProductName} / {variant.VariantName} " +
                        $"{variantMarker} — {variant.StockQuantity} unit(s) remaining " +
                        $"(threshold {variant.ReorderThreshold}).",
                    CreatedAt = DateTime.UtcNow
                }, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                // Human-readable label: omit "(Default)" for single-variant products.
                string variantLabel = variant.VariantName.Equals(
                    "Default", StringComparison.OrdinalIgnoreCase)
                    ? variant.ProductName
                    : $"{variant.ProductName} ({variant.VariantName})";

                // ── 5. Queue LowStockAlert to all admin / manager users ────────
                foreach (var admin in adminRecipients)
                {
                    try
                    {
                        await notifications.QueueAsync(
                            channel:   NotifChannels.Email,
                            notifType: NotifTypes.LowStockAlert,
                            recipient: admin.Email!,
                            subject:   $"Low Stock Alert: {variantLabel}",
                            body:
                                $"Hi {admin.FirstName},\n\n" +
                                $"Stock is running low for the following product:\n\n" +
                                $"  Product           : {variantLabel}\n" +
                                $"  Current stock     : {variant.StockQuantity} unit(s)\n" +
                                $"  Reorder threshold : {variant.ReorderThreshold} unit(s)\n\n" +
                                $"Please raise a purchase order or adjust stock as soon as possible.\n\n" +
                                $"— Taurus Bike Shop System",
                            userId:            admin.UserId,
                            cancellationToken: cancellationToken);
                    }
                    catch (Exception ex) when (ex is not OperationCanceledException)
                    {
                        // One admin's failure must not block the rest.
                        _logger.LogError(ex,
                            "StockMonitorJob: failed to queue LowStockAlert " +
                            "for admin {UserId} (variant {VariantId}).",
                            admin.UserId, variant.ProductVariantId);
                    }
                }

                // ── 6. Queue WishlistRestock nudge to wishlisting customers ────
                // Only sent while stock is still > 0 — a product with zero units
                // available should not prompt customers to buy.
                if (variant.StockQuantity > 0)
                {
                    // Batch-load all eligible wishlist customers for this product
                    // in one query to avoid N+1 lookups inside the loop.
                    var wishlistCustomers = await context.Users
                        .AsNoTracking()
                        .Where(u => u.IsActive
                                 && !u.IsWalkIn
                                 && u.Email != null
                                 && u.Wishlist.Any(w => w.ProductId == variant.ProductId))
                        .Select(u => new { u.UserId, u.Email, u.FirstName })
                        .ToListAsync(cancellationToken);

                    foreach (var customer in wishlistCustomers)
                    {
                        try
                        {
                            await notifications.QueueAsync(
                                channel:   NotifChannels.Email,
                                notifType: NotifTypes.WishlistRestock,
                                recipient: customer.Email!,
                                subject:   $"Hurry — {variantLabel} is almost sold out!",
                                body:
                                    $"Hi {customer.FirstName},\n\n" +
                                    $"A product you saved to your wishlist is still available, " +
                                    $"but stock is running low:\n\n" +
                                    $"  Product    : {variantLabel}\n" +
                                    $"  Units left : {variant.StockQuantity}\n\n" +
                                    $"Order soon before it sells out!\n\n" +
                                    $"— Taurus Bike Shop",
                                userId:            customer.UserId,
                                cancellationToken: cancellationToken);
                        }
                        catch (Exception ex) when (ex is not OperationCanceledException)
                        {
                            // One customer's failure must not block the rest.
                            _logger.LogError(ex,
                                "StockMonitorJob: failed to queue WishlistRestock " +
                                "for customer {UserId} (variant {VariantId}).",
                                customer.UserId, variant.ProductVariantId);
                        }
                    }
                }

                notifiedVariants++;
            }

            // ── Cycle complete log ─────────────────────────────────────────────
            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobComplete,
                EventDescription =
                    $"{nameof(StockMonitorJob)} cycle completed. " +
                    $"{lowStockVariants.Count} low-stock variant(s) detected, " +
                    $"{notifiedVariants} newly alerted (others within cooldown).",
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "StockMonitorJob cycle failed.");

            bool isDbError = ex is Microsoft.EntityFrameworkCore.Storage.RetryLimitExceededException
                          || ex.InnerException is Microsoft.Data.SqlClient.SqlException;
            if (!isDbError)
            {
                try
                {
                    await using AsyncServiceScope err = _scopeFactory.CreateAsyncScope();
                    AppDbContext ec = err.ServiceProvider.GetRequiredService<AppDbContext>();
                    await ec.SystemLogs.AddAsync(new SystemLog
                    {
                        EventType        = SystemLogEvents.BackgroundJobError,
                        EventDescription =
                            $"{nameof(StockMonitorJob)} error: " +
                            ex.Message[..Math.Min(ex.Message.Length, 200)],
                        CreatedAt = DateTime.UtcNow
                    }, cancellationToken);
                    await ec.SaveChangesAsync(cancellationToken);
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "Failed to write error log for StockMonitorJob.");
                }
            }
        }
    }
}
