// WebApplication/Models/Entities/LalamoveDelivery.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Lalamove-specific delivery data. Exists only when the parent
/// <see cref="Delivery.Courier"/> = <c>Lalamove</c>.
/// <para>
/// <b>Shared PK pattern:</b> <see cref="DeliveryId"/> is simultaneously the primary
/// key of this table AND a foreign key to <c>Delivery.DeliveryId</c>, enforcing
/// a strict 1:1 relationship. Configured in AppDbContext via
/// HasOne/WithOne/HasForeignKey.
/// </para>
/// <para>
/// <b>Field population timeline:</b>
/// <list type="bullet">
///   <item><see cref="BookingRef"/> — populated when the AdminSystem
///     successfully books via the Lalamove API.</item>
///   <item><see cref="DriverName"/> and <see cref="DriverPhone"/> — populated
///     when Lalamove assigns a driver to the booking.
///     Polled by <c>DeliveryStatusPollJob</c> every 5 minutes.</item>
/// </list>
/// All three fields are NULL until the respective events occur.
/// </para>
/// </summary>
public sealed class LalamoveDelivery
{
    /// <summary>
    /// Primary key AND foreign key to <c>Delivery.DeliveryId</c>.
    /// Not auto-increment — value is set equal to the parent Delivery's PK.
    /// Configured via HasOne/WithOne/HasForeignKey in AppDbContext.
    /// </summary>
    public int DeliveryId { get; set; }

    /// <summary>
    /// Lalamove booking reference number returned by the Lalamove API
    /// after a successful booking.
    /// NULL until the booking is confirmed via the API.
    /// </summary>
    [MaxLength(255)]
    public string? BookingRef { get; set; }

    /// <summary>
    /// Name of the Lalamove driver assigned to this delivery.
    /// NULL until Lalamove assigns a driver to the booking.
    /// Populated by <c>DeliveryStatusPollJob</c> when the API returns driver info.
    /// </summary>
    [MaxLength(100)]
    public string? DriverName { get; set; }

    /// <summary>
    /// Phone number of the Lalamove driver assigned to this delivery.
    /// NULL until a driver is assigned.
    /// Displayed to the customer on the order tracking page.
    /// </summary>
    [MaxLength(20)]
    public string? DriverPhone { get; set; }

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The parent delivery record this Lalamove data belongs to.</summary>
    public Delivery Delivery { get; set; } = null!;
}