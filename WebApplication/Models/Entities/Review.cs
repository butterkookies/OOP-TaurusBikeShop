// WebApplication/Models/Entities/Review.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents a customer product review submitted after confirmed delivery.
/// <para>
/// <b>Verified purchase requirement:</b> A review can only be submitted when
/// <c>ReviewService.HasVerifiedPurchaseAsync</c> confirms that the submitting
/// user has a Delivered order containing the reviewed product.
/// <see cref="IsVerifiedPurchase"/> is set to <c>true</c> by the service
/// after this check passes.
/// </para>
/// <para>
/// <b>Rating range:</b> 1–5 stars, enforced by CK_Review_Rating.
/// </para>
/// </summary>
public sealed class Review
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int ReviewId { get; set; }

    /// <summary>FK to the customer who submitted this review.</summary>
    public int UserId { get; set; }

    /// <summary>FK to the product being reviewed.</summary>
    public int ProductId { get; set; }

    /// <summary>
    /// FK to the order that contains the reviewed product.
    /// Links the review to a specific confirmed purchase for verification.
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Star rating from 1 (worst) to 5 (best).
    /// Enforced by CK_Review_Rating: must be &gt;= 1 AND &lt;= 5.
    /// </summary>
    public int Rating { get; set; }

    /// <summary>Optional free-text review comment from the customer.</summary>
    [MaxLength(1000)]
    public string? Comment { get; set; }

    /// <summary>
    /// <c>true</c> when <c>ReviewService</c> has confirmed that the user has
    /// a Delivered order containing this product.
    /// Always set to <c>true</c> by the service before inserting — the web UI
    /// only shows the review form when this check passes.
    /// </summary>
    public bool IsVerifiedPurchase { get; set; } = false;

    /// <summary>UTC timestamp when this review was submitted.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The customer who submitted this review.</summary>
    public User User { get; set; } = null!;

    /// <summary>The product being reviewed.</summary>
    public Product Product { get; set; } = null!;

    /// <summary>The order that contains the reviewed product.</summary>
    public Order Order { get; set; } = null!;
}