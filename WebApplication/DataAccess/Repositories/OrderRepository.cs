// WebApplication/DataAccess/Repositories/OrderRepository.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories;

/// <summary>
/// Data access for <see cref="Order"/> and <see cref="OrderItem"/> entities.
/// Handles order history retrieval, full order detail loading, status updates,
/// and pending order monitoring.
/// </summary>
public sealed class OrderRepository : Repository<Order>
{
    /// <inheritdoc/>
    public OrderRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Returns a paginated list of orders for a specific user,
    /// ordered by most recent first.
    /// Used by the Order History page.
    /// </summary>
    /// <param name="userId">The user whose orders to retrieve.</param>
    /// <param name="pageNum">1-based page number.</param>
    /// <param name="pageSize">Number of orders per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A page of orders for the user, most recent first.</returns>
    public async Task<IReadOnlyList<Order>> GetOrdersByUserAsync(
        int userId,
        int pageNum,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await Context.Orders
            .AsNoTracking()
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns a single order with all related data loaded for full detail display.
    /// Includes: OrderItems with Product and Variant, Payments with GCash and
    /// BankTransfer subtypes, Deliveries with Lalamove and LBC subtypes,
    /// PickupOrder, and ShippingAddress.
    /// </summary>
    /// <param name="orderId">The order ID to load.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The fully loaded order, or <c>null</c> if not found.</returns>
    public async Task<Order?> GetOrderWithDetailsAsync(
        int orderId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Items)
                .ThenInclude(oi => oi.Variant)
            .Include(o => o.Payments)
                .ThenInclude(p => p.GCashPayment)
            .Include(o => o.Payments)
                .ThenInclude(p => p.BankTransferPayment)
            .Include(o => o.Deliveries)
                .ThenInclude(d => d.LalamoveDelivery)
            .Include(o => o.Deliveries)
                .ThenInclude(d => d.LBCDelivery)
            .Include(o => o.PickupOrder)
            .Include(o => o.ShippingAddress)
            .FirstOrDefaultAsync(o => o.OrderId == orderId, cancellationToken);
    }

    /// <summary>
    /// Updates the <c>OrderStatus</c> of a single order and persists the change.
    /// Does not perform any business rule validation — callers (services) are
    /// responsible for ensuring the transition is valid.
    /// </summary>
    /// <param name="orderId">The order to update.</param>
    /// <param name="status">
    /// The new status value. Use <see cref="OrderStatuses"/> constants.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no order with <paramref name="orderId"/> exists.
    /// </exception>
    public async Task UpdateStatusAsync(
        int orderId,
        string status,
        CancellationToken cancellationToken = default)
    {
        Order? order = await DbSet.FindAsync(new object[] { orderId }, cancellationToken);

        if (order is null)
            throw new InvalidOperationException(
                $"Cannot update status: Order {orderId} not found.");

        order.OrderStatus = status;
        await Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Returns all orders that are still in <c>Pending</c> status and were
    /// created more than <paramref name="cutoffHours"/> hours ago.
    /// Used by <c>PendingOrderMonitorJob</c> to identify stale unprocessed orders.
    /// </summary>
    /// <param name="cutoffHours">
    /// The number of hours after which a Pending order is considered stale.
    /// Typically 24.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All stale pending orders.</returns>
    public async Task<IReadOnlyList<Order>> GetPendingUnprocessedAsync(
        int cutoffHours,
        CancellationToken cancellationToken = default)
    {
        DateTime cutoff = DateTime.UtcNow.AddHours(-cutoffHours);

        return await Context.Orders
            .AsNoTracking()
            .Where(o => o.OrderStatus == OrderStatuses.Pending
                     && o.CreatedAt < cutoff)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns the total number of orders placed by the specified user.
    /// Used to calculate total page count for the Order History pagination.
    /// </summary>
    /// <param name="userId">The user ID to count orders for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Total order count for the user.</returns>
    public async Task<int> GetOrderCountByUserAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Orders
            .CountAsync(o => o.UserId == userId, cancellationToken);
    }
}