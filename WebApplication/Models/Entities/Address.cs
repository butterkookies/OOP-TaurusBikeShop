// WebApplication/Models/Entities/Address.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents a user's shipping or billing address.
/// <para>
/// Two types of rows exist in this table:
/// <list type="bullet">
///   <item>
///     <b>Live addresses</b> (<see cref="IsSnapshot"/> = false) — editable addresses
///     saved to the user's profile. One may be marked as default via
///     <see cref="IsDefault"/>.
///   </item>
///   <item>
///     <b>Snapshot addresses</b> (<see cref="IsSnapshot"/> = true) — immutable copies
///     created at checkout time and linked to an order via
///     <c>Order.ShippingAddressId</c>. These are never modified after creation,
///     preserving the exact address at the time of the order permanently.
///   </item>
/// </list>
/// </para>
/// <para>
/// Part of the circular FK pair: <c>User.DefaultAddressId</c> → <c>Address.AddressId</c>.
/// The reverse side (<c>Address.UserId</c> → <c>User.UserId</c>) uses CASCADE DELETE
/// so that deleting a user removes all their addresses. The circular FK on
/// <c>User.DefaultAddressId</c> is configured with no cascade delete in AppDbContext
/// to break the cycle.
/// </para>
/// </summary>
public sealed class Address
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int AddressId { get; set; }

    /// <summary>
    /// FK to the user who owns this address.
    /// Configured with CASCADE DELETE — removing a user removes all their addresses.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Display label for this address.
    /// Constrained by CK_Address_Label: Home, Work, or Other.
    /// Use <see cref="AddressLabels"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Label { get; set; } = AddressLabels.Home;

    /// <summary>Street line — house number, street name, barangay.</summary>
    [Required]
    [MaxLength(500)]
    public string Street { get; set; } = string.Empty;

    /// <summary>City or municipality.</summary>
    [Required]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    /// <summary>4-digit Philippine postal code.</summary>
    [Required]
    [MaxLength(20)]
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>Province or region. Optional for Metro Manila addresses.</summary>
    [MaxLength(100)]
    public string? Province { get; set; }

    /// <summary>Country name. Defaults to Philippines.</summary>
    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = "Philippines";

    /// <summary>
    /// <c>true</c> if this is the user's currently selected default shipping address.
    /// Only one live address per user should have this set to <c>true</c>.
    /// Snapshot rows always have <see cref="IsDefault"/> = false.
    /// </summary>
    public bool IsDefault { get; set; } = false;

    /// <summary>
    /// <c>true</c> for immutable checkout snapshot copies of this address.
    /// Snapshot rows are created at order placement and must never be modified
    /// after creation — they are the permanent record of where an order was shipped.
    /// </summary>
    public bool IsSnapshot { get; set; } = false;

    /// <summary>UTC timestamp when this address row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The user who owns this address.</summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Orders that used this address as the shipping destination.
    /// Populated only for snapshot rows (<see cref="IsSnapshot"/> = true).
    /// </summary>
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

/// <summary>
/// Compile-time constants for all valid address label values.
/// Mirrors the CK_Address_Label CHECK constraint in the database.
/// </summary>
public static class AddressLabels
{
    /// <summary>Home address.</summary>
    public const string Home = "Home";

    /// <summary>Work or office address.</summary>
    public const string Work = "Work";

    /// <summary>Any other address type.</summary>
    public const string Other = "Other";
}