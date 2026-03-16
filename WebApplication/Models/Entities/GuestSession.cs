// WebApplication/Models/Entities/GuestSession.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents an anonymous browsing and checkout session for a guest user.
/// A guest session is created when an unauthenticated visitor adds an item to
/// the cart. The session token is stored in a browser cookie to identify the
/// guest across requests.
/// <para>
/// When a guest registers or logs in, <see cref="ConvertedToUserId"/> is set and
/// <c>CartService.MergeGuestCartAsync</c> moves all cart items to the user's cart.
/// Expired sessions and their associated carts can be purged by a maintenance job.
/// </para>
/// </summary>
public sealed class GuestSession
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int GuestSessionId { get; set; }

    /// <summary>
    /// Client-generated UUID stored in the guest's browser cookie.
    /// Used to identify the guest across requests without authentication.
    /// Enforced unique via UX_GuestSession_Token in the database.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string SessionToken { get; set; } = string.Empty;

    /// <summary>
    /// Email address optionally collected from the guest during checkout.
    /// Used to pre-fill the registration form if the guest decides to register.
    /// </summary>
    [MaxLength(255)]
    public string? Email { get; set; }

    /// <summary>
    /// Phone number optionally collected from the guest during checkout.
    /// </summary>
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// FK to the registered user account this guest session was converted to.
    /// Set when the guest registers or logs in during or after checkout.
    /// NULL while the session remains anonymous.
    /// </summary>
    public int? ConvertedToUserId { get; set; }

    /// <summary>
    /// UTC timestamp after which this session is considered expired.
    /// Typically set to <c>CreatedAt + 7 days</c> by <c>CartService</c>.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>UTC timestamp when this session row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>
    /// The registered user this guest session was converted to, if any.
    /// NULL while the session is still anonymous.
    /// </summary>
    public User? ConvertedToUser { get; set; }

    /// <summary>
    /// The guest's shopping cart associated with this session.
    /// A guest has at most one active (IsCheckedOut = false) cart at a time,
    /// enforced by the UX_Cart_Guest_Active filtered unique index.
    /// </summary>
    public ICollection<Cart> Carts { get; set; } = new List<Cart>();
}