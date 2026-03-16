// WebApplication/Models/Entities/Delivery.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Base delivery record — courier-agnostic. One row per delivery attempt per order.
/// Courier-specific data lives in subtype tables sharing this row's PK:
/// <list type="bullet">
///   <item><see cref="LalamoveDelivery"/> — when <see cref="Courier"/> = Lalamove</item>
///   <item><see cref="LBCDelivery"/> — when <see cref="Courier"/> = LBC</item>
/// </list>
/// <para>
/// <b>Couriers:</b> Lalamove and LBC only. No other courier values are valid.
/// Use <see cref="Couriers"/> constants instead of magic strings.
/// </para>
/// <para>
/// <b>Delivery status flow:</b>
/// Pending → PickedUp → InTransit → Delivered | Failed.
/// Updated by <c>DeliveryStatusPollJob</c> via courier API polling every 5 minutes,
/// and by the AdminSystem when dispatching or marking delays.
/// Use <see cref="DeliveryStatuses"/> constants instead of magic strings.
/// </para>
/// <para>
/// <b>Delay tracking:</b> When a delivery is delayed, the AdminSystem sets
/// <see cref="IsDelayed"/> = true and updates <see cref="DelayedUntil"/> with
/// the new ETA. A DeliveryDelay notification is queued for the customer.
/// </para>
/// </summary>
public sealed class Delivery
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int DeliveryId { get; set; }

    /// <summary>FK to the order being delivered.</summary>
    public int OrderId { get; set; }

    /// <summary>
    /// The courier handling this delivery.
    /// Constrained by CK_Delivery_Courier: Lalamove or LBC only.
    /// Use <see cref="Couriers"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Courier { get; set; } = string.Empty;

    /// <summary>
    /// Current status of the delivery.
    /// Constrained by CK_Delivery_Status: Pending, PickedUp, InTransit,
    /// Delivered, Failed.
    /// Use <see cref="DeliveryStatuses"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string DeliveryStatus { get; set; } = DeliveryStatuses.Pending;

    /// <summary>
    /// <c>true</c> when the AdminSystem has flagged this delivery as delayed.
    /// Set alongside <see cref="DelayedUntil"/> to record the new ETA.
    /// Triggers a DeliveryDelay notification to the customer.
    /// </summary>
    public bool IsDelayed { get; set; } = false;

    /// <summary>
    /// Updated estimated delivery time after a delay has been flagged.
    /// NULL until <see cref="IsDelayed"/> is set to true.
    /// </summary>
    public DateTime? DelayedUntil { get; set; }

    /// <summary>
    /// Original estimated delivery time provided at the time of booking.
    /// NULL until the courier booking is confirmed.
    /// </summary>
    public DateTime? EstimatedDeliveryTime { get; set; }

    /// <summary>
    /// UTC timestamp when the delivery was physically completed.
    /// Set by <c>DeliveryStatusPollJob</c> when status transitions to Delivered.
    /// NULL until delivery is confirmed.
    /// </summary>
    public DateTime? ActualDeliveryTime { get; set; }

    /// <summary>UTC timestamp when this delivery row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The order being delivered.</summary>
    public Order Order { get; set; } = null!;

    /// <summary>
    /// Lalamove-specific delivery data. NULL when Courier != Lalamove.
    /// 1:1 relationship — DeliveryId is both PK and FK on LalamoveDelivery.
    /// Configured via HasOne/WithOne/HasForeignKey in AppDbContext.
    /// </summary>
    public LalamoveDelivery? LalamoveDelivery { get; set; }

    /// <summary>
    /// LBC-specific delivery data. NULL when Courier != LBC.
    /// 1:1 relationship — DeliveryId is both PK and FK on LBCDelivery.
    /// Configured via HasOne/WithOne/HasForeignKey in AppDbContext.
    /// </summary>
    public LBCDelivery? LBCDelivery { get; set; }
}

/// <summary>
/// Compile-time constants for all valid courier values.
/// Mirrors the CK_Delivery_Courier CHECK constraint in the database.
/// Only Lalamove and LBC are supported — no other courier values are valid anywhere.
/// </summary>
public static class Couriers
{
    /// <summary>Lalamove same-day courier service.</summary>
    public const string Lalamove = "Lalamove";

    /// <summary>LBC standard parcel delivery service.</summary>
    public const string LBC = "LBC";
}

/// <summary>
/// Compile-time constants for all valid delivery status values.
/// Mirrors the CK_Delivery_Status CHECK constraint in the database.
/// </summary>
public static class DeliveryStatuses
{
    /// <summary>Delivery has been created but the courier has not yet collected the parcel.</summary>
    public const string Pending = "Pending";

    /// <summary>Courier has collected the parcel from the store.</summary>
    public const string PickedUp = "PickedUp";

    /// <summary>Parcel is en route to the delivery address.</summary>
    public const string InTransit = "InTransit";

    /// <summary>Parcel has been successfully delivered to the customer.</summary>
    public const string Delivered = "Delivered";

    /// <summary>Delivery attempt failed — customer was unavailable or address was incorrect.</summary>
    public const string Failed = "Failed";
}