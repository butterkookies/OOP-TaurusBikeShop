using AdminSystem_v2.Models;

namespace AdminSystem_v2.Repositories
{
    public interface IOrderRepository
    {
        /// <summary>
        /// Returns online orders (IsWalkIn = 0), optionally filtered by status and/or order type.
        /// </summary>
        Task<IEnumerable<Order>> GetOrdersAsync(string? statusFilter = null, string? typeFilter = null);

        /// <summary>Returns the full order detail including items, delivery, and pickup records.</summary>
        Task<Order?> GetOrderDetailAsync(int orderId);

        /// <summary>
        /// Updates the Order.OrderStatus column after validating the transition.
        /// Throws <see cref="InvalidStatusTransitionException"/> if the transition is invalid.
        /// </summary>
        Task UpdateOrderStatusAsync(int orderId, string status, string? expectedCurrentStatus = null);

        /// <summary>
        /// Sets Order status to ReadyForPickup and upserts the PickupOrder row
        /// with PickupReadyAt = now, PickupExpiresAt = now + 7 days.
        /// Throws <see cref="InvalidStatusTransitionException"/> if the transition is invalid.
        /// </summary>
        Task MarkReadyForPickupAsync(int orderId, string? expectedCurrentStatus = null);

        /// <summary>
        /// Sets Order status to PickedUp and stamps PickupOrder.PickupConfirmedAt.
        /// Throws <see cref="InvalidStatusTransitionException"/> if the transition is invalid.
        /// </summary>
        Task ConfirmPickupAsync(int orderId, string? expectedCurrentStatus = null);

        /// <summary>
        /// Returns a dictionary of OrderStatus → count for all online orders.
        /// Used to populate the status badge bar without loading every row.
        /// </summary>
        Task<Dictionary<string, int>> GetStatusCountsAsync();

        /// <summary>
        /// Approves the payment proof for a PendingVerification order:
        /// transitions order to Processing and payment to Completed.
        /// </summary>
        Task ApprovePaymentAsync(int orderId, string? expectedCurrentStatus = null);

        /// <summary>
        /// Places the order on hold when the admin finds a discrepancy in the payment proof:
        /// transitions order from PaymentVerification to OnHold and payment to VerificationRejected.
        /// </summary>
        Task HoldPaymentAsync(int orderId, string? expectedCurrentStatus = null);

        /// <summary>
        /// Auto-cancels any Pending orders older than 24 hours, unlocks their inventory,
        /// and logs the transitions. Returns the number of orders that were cancelled.
        /// Called at the start of each order list load to ensure stale orders are cleaned up.
        /// </summary>
        Task<int> AutoCancelExpiredPendingOrdersAsync();
    }
}
