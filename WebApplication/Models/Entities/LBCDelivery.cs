// WebApplication/Models/Entities/LBCDelivery.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// LBC-specific delivery data. Exists only when the parent
/// <see cref="Delivery.Courier"/> = <c>LBC</c>.
/// <para>
/// <b>Shared PK pattern:</b> <see cref="DeliveryId"/> is simultaneously the primary
/// key of this table AND a foreign key to <c>Delivery.DeliveryId</c>, enforcing
/// a strict 1:1 relationship. Configured in AppDbContext via
/// HasOne/WithOne/HasForeignKey.
/// </para>
/// <para>
/// <b>No driver fields:</b> Unlike <see cref="LalamoveDelivery"/>, this table has
/// no driver name or phone columns. LBC does not expose driver information
/// via its API — tracking is waybill-number based only.
/// </para>
/// <para>
/// <b>Tracking number:</b> <see cref="TrackingNumber"/> is the LBC waybill number
/// generated when the parcel is booked. NULL until the AdminSystem receives
/// confirmation from LBC. Polled by <c>DeliveryStatusPollJob</c>.
/// </para>
/// </summary>
public sealed class LBCDelivery
{
    /// <summary>
    /// Primary key AND foreign key to <c>Delivery.DeliveryId</c>.
    /// Not auto-increment — value is set equal to the parent Delivery's PK.
    /// Configured via HasOne/WithOne/HasForeignKey in AppDbContext.
    /// </summary>
    public int DeliveryId { get; set; }

    /// <summary>
    /// LBC waybill tracking number for this shipment.
    /// NULL until LBC generates the waybill after booking confirmation.
    /// Used by the customer to track their parcel on the LBC website.
    /// </summary>
    [MaxLength(255)]
    public string? TrackingNumber { get; set; }

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The parent delivery record this LBC data belongs to.</summary>
    public Delivery Delivery { get; set; } = null!;
}