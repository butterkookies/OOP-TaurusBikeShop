using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    public interface IOrderService
    {
        /// <summary>
        /// Returns online orders, optionally filtered by status and/or order type (Delivery/Pickup).
        /// </summary>
        Task<IEnumerable<Order>> GetOrdersAsync(string? statusFilter = null, string? typeFilter = null);

        Task<Order?> GetOrderDetailAsync(int orderId);

        /// <summary>
        /// Updates order status after validating the transition.
        /// Throws <see cref="InvalidStatusTransitionException"/> on invalid transitions.
        /// </summary>
        Task UpdateOrderStatusAsync(int orderId, string status, string? expectedCurrentStatus = null);

        Task MarkReadyForPickupAsync(int orderId, string? expectedCurrentStatus = null);
        Task ConfirmPickupAsync(int orderId, string? expectedCurrentStatus = null);

        /// <summary>Returns order counts per status for the badge bar (single cheap query).</summary>
        Task<Dictionary<string, int>> GetStatusCountsAsync();

        /// <summary>Approves the payment proof: transitions order to Processing, payment to Completed.</summary>
        Task ApprovePaymentAsync(int orderId, string? expectedCurrentStatus = null);

        /// <summary>Holds the payment on discrepancy: moves order to OnHold, payment to VerificationRejected.</summary>
        Task HoldPaymentAsync(int orderId, string? expectedCurrentStatus = null);
    }
}
