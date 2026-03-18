// WebApplication/BusinessLogic/Interfaces/IOrderService.cs

using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Interfaces;

/// <summary>
/// Contract for order creation and lifecycle management.
/// Flowchart: Part 3 — Cart &amp; Checkout, Part 4 — Order Tracking.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Creates a new online order from the customer's active cart.
    /// Executes atomically in a single DB transaction:
    /// <list type="number">
    ///   <item>Validates all cart items still have sufficient stock.</item>
    ///   <item>Creates the <c>Order</c> row.</item>
    ///   <item>Creates all <c>OrderItem</c> rows.</item>
    ///   <item>Locks stock via <c>InventoryLog</c> Lock entries per item.</item>
    ///   <item>Creates the shipping address snapshot.</item>
    ///   <item>Creates a <c>PickupOrder</c> row if delivery method is Pickup.</item>
    ///   <item>Redeems the voucher (writes <c>VoucherUsage</c>) if one was applied.</item>
    ///   <item>Marks the cart as checked out.</item>
    ///   <item>Queues an OrderConfirmation notification.</item>
    ///   <item>Writes a SystemLog OrderStatusChange entry.</item>
    /// </list>
    /// </summary>
    /// <param name="userId">The authenticated user placing the order.</param>
    /// <param name="vm">The validated checkout form data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="ServiceResult{T}"/> containing the new <c>OrderId</c> on success,
    /// or a descriptive error on failure.
    /// </returns>
    Task<ServiceResult<int>> CreateOrderAsync(
        int userId,
        CheckoutViewModel vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the order view model for the order confirmation page.
    /// Validates that the order belongs to the requesting user.
    /// </summary>
    Task<OrderViewModel?> GetOrderConfirmationAsync(
        int orderId,
        int userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a paginated list of orders for the customer's order history page.
    /// </summary>
    Task<(IReadOnlyList<OrderViewModel> Orders, int TotalCount)> GetOrderHistoryAsync(
        int userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the full order detail view model for the order tracking page.
    /// Validates that the order belongs to the requesting user.
    /// </summary>
    Task<OrderViewModel?> GetOrderDetailAsync(
        int orderId,
        int userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels a customer's own order if it is still in a cancellable state
    /// (Pending or PendingVerification).
    /// Unlocks inventory via InventoryLog Unlock entries.
    /// </summary>
    Task<ServiceResult> CancelOrderAsync(
        int orderId,
        int userId,
        CancellationToken cancellationToken = default);
}