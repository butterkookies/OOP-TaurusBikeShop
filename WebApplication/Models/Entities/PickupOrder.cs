// WebApplication/Models/Entities/PickupOrder.cs

namespace WebApplication.Models.Entities;

/// <summary>
/// Stores store-pickup metadata for orders where the customer chose to collect
/// in person rather than have the order delivered by a courier.
/// <para>
/// <b>1:1 relationship with Order:</b> This table only has rows for pickup orders.
/// The <see cref="OrderId"/> column is both the PK and a unique FK to
/// <c>Order.OrderId</c>, enforcing a strict 1:1 relationship.
/// Configured in AppDbContext via HasOne/WithOne/HasForeignKey.
/// </para>
/// <para>
/// <b>Pickup expiry:</b> <see cref="PickupExpiresAt"/> is set to
/// <c>PickupReadyAt + 3 days</c> by the AdminSystem when the order is marked
/// ReadyForPickup. <c>PendingOrderMonitorJob</c> polls this column and
/// auto-holds orders that have not been collected by the expiry time.
/// </para>
/// </summary>
public sealed class PickupOrder
{
    /// <summary>
    /// Primary key — also the FK to <c>Order.OrderId</c>.
    /// Not an auto-increment identity — the parent Order's PK value is used directly.
    /// Configured in AppDbContext with HasOne/WithOne/HasForeignKey.
    /// </summary>
    public int PickupOrderId { get; set; }

    /// <summary>
    /// FK to the parent order. UNIQUE constraint enforces the strict 1:1 relationship.
    /// Configured with CASCADE DELETE — removing an order removes its pickup record.
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// UTC timestamp when the AdminSystem marked the order as ready for collection.
    /// NULL until the admin sets the order status to ReadyForPickup.
    /// Triggers the ReadyForPickup notification to the customer.
    /// </summary>
    public DateTime? PickupReadyAt { get; set; }

    /// <summary>
    /// UTC timestamp after which the uncollected order will be auto-held.
    /// Set to <c>PickupReadyAt + 3 days</c> by the AdminSystem.
    /// Polled by <c>PendingOrderMonitorJob</c>.
    /// NULL until <see cref="PickupReadyAt"/> is set.
    /// </summary>
    public DateTime? PickupExpiresAt { get; set; }

    /// <summary>
    /// UTC timestamp when the customer physically collected the order.
    /// Set by the AdminSystem when pickup is confirmed.
    /// NULL until the customer collects.
    /// </summary>
    public DateTime? PickupConfirmedAt { get; set; }

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The order this pickup record belongs to.</summary>
    public Order Order { get; set; } = null!;
}