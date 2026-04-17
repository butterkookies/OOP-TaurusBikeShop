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
    /// <param name="selectedCartItemIds">
    /// Optional filter — when non-null, only cart items whose <c>CartItemId</c>
    /// appears in the set are included in the order. Unselected items remain in
    /// the cart for a later checkout. When null, the entire cart is consumed.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="ServiceResult{T}"/> containing the new <c>OrderId</c> on success,
    /// or a descriptive error on failure.
    /// </returns>
    Task<ServiceResult<int>> CreateOrderAsync(
        int userId,
        CheckoutViewModel vm,
        IReadOnlyCollection<int>? selectedCartItemIds = null,
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

    /// <summary>
    /// Confirms that an out-for-delivery order has been physically received
    /// by the customer. Transitions the order from <c>OutForDelivery</c> to
    /// <c>Delivered</c> and finalises inventory by writing paired
    /// <c>Unlock</c> + <c>Sale</c> InventoryLog entries per line item.
    /// <para>
    /// Only valid for orders in <c>OutForDelivery</c> status that are not walk-in
    /// POS orders and not store-pickup orders.
    /// </para>
    /// <para>
    /// Executes atomically in a single DB transaction:
    /// <list type="number">
    ///   <item>Sets OrderStatus to Delivered.</item>
    ///   <item>Per item: writes Unlock entry (+qty, releases the lock in the ledger)
    ///     and Sale entry (-qty, records actual consumption). Net StockQuantity
    ///     change is zero — stock was already decremented at order placement.</item>
    ///   <item>Writes a SystemLog OrderStatusChange entry.</item>
    /// </list>
    /// A DeliveryConfirmation notification is queued outside the transaction.
    /// </para>
    /// Flowchart reference: Part 6 — D28A (Update Order Status: Delivered)
    /// and D28B (Unlock Pending Stock in InventoryLog).
    /// </summary>
    /// <param name="orderId">The order to confirm.</param>
    /// <param name="userId">The authenticated customer confirming receipt.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="ServiceResult"/> indicating success or a descriptive error.
    /// </returns>
    Task<ServiceResult> ConfirmDeliveryAsync(
        int orderId,
        int userId,
        CancellationToken cancellationToken = default);
}