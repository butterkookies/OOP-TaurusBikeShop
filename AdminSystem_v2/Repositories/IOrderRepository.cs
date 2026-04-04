using AdminSystem_v2.Models;

namespace AdminSystem_v2.Repositories
{
    public interface IOrderRepository
    {
        /// <summary>Returns online orders (IsWalkIn = 0), optionally filtered by status.</summary>
        Task<IEnumerable<Order>> GetOrdersAsync(string? statusFilter = null);

        /// <summary>Returns the full order detail including items, delivery, and pickup records.</summary>
        Task<Order?> GetOrderDetailAsync(int orderId);

        /// <summary>Updates the Order.OrderStatus column.</summary>
        Task UpdateOrderStatusAsync(int orderId, string status);

        /// <summary>
        /// Sets Order status to ReadyForPickup and upserts the PickupOrder row
        /// with PickupReadyAt = now, PickupExpiresAt = now + 7 days.
        /// </summary>
        Task MarkReadyForPickupAsync(int orderId);

        /// <summary>
        /// Sets Order status to PickedUp and stamps PickupOrder.PickupConfirmedAt.
        /// </summary>
        Task ConfirmPickupAsync(int orderId);
    }
}
