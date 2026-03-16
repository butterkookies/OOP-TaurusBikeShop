// WebApplication/Models/Entities/Cart.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents an active or checked-out shopping cart session.
/// Supports both authenticated users and anonymous guests.
/// <para>
/// <b>Ownership rule:</b> Exactly one of <see cref="UserId"/> or
/// <see cref="GuestSessionId"/> must be non-NULL — never both, never neither.
/// This is enforced by CK_Cart_Owner in the database. Application code in
/// <c>CartService</c> must validate this before inserting a cart row.
/// </para>
/// <para>
/// <b>Active cart rule:</b> Each user and each guest session may have at most one
/// active cart (<see cref="IsCheckedOut"/> = false) at a time. Enforced by
/// UX_Cart_User_Active and UX_Cart_Guest_Active filtered unique indexes.
/// </para>
/// <para>
/// <b>Guest-to-user merge:</b> When a guest logs in or registers,
/// <c>CartService.MergeGuestCartAsync</c> copies all items from the guest cart
/// to the user's cart, then marks the guest cart as checked out.
/// </para>
/// </summary>
public sealed class Cart
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int CartId { get; set; }

    /// <summary>
    /// FK to the registered user who owns this cart.
    /// NULL for guest carts. Mutually exclusive with <see cref="GuestSessionId"/>.
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// FK to the guest session that owns this cart.
    /// NULL for authenticated user carts. Mutually exclusive with <see cref="UserId"/>.
    /// </summary>
    public int? GuestSessionId { get; set; }

    /// <summary>UTC timestamp when this cart was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// UTC timestamp of the last item add, remove, or quantity update.
    /// NULL until the first modification after creation.
    /// </summary>
    public DateTime? LastUpdatedAt { get; set; }

    /// <summary>
    /// <c>false</c> while the cart is active and items can be added or removed.
    /// Flipped to <c>true</c> when the cart is converted to an order during checkout,
    /// or when a guest cart is consumed during a guest-to-user merge.
    /// Once checked out, a cart is immutable.
    /// </summary>
    public bool IsCheckedOut { get; set; } = false;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>
    /// The registered user who owns this cart.
    /// NULL for guest carts.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// The guest session that owns this cart.
    /// NULL for authenticated user carts.
    /// </summary>
    public GuestSession? GuestSession { get; set; }

    /// <summary>
    /// All line items currently in this cart.
    /// Cascade-deleted when the cart is deleted.
    /// </summary>
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}