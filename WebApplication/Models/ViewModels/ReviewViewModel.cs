// WebApplication/Models/ViewModels/ReviewViewModel.cs

namespace WebApplication.Models.ViewModels;

/// <summary>
/// Represents a single review entry for display in the reviews feed.
/// </summary>
public sealed class ReviewItemViewModel
{
    /// <summary>Display name of the reviewer (first name only for privacy).</summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>Star rating 1–5.</summary>
    public int Rating { get; set; }

    /// <summary>Optional free-text comment.</summary>
    public string? Comment { get; set; }

    /// <summary>UTC timestamp when the review was submitted.</summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// View model for the Write a Review page (GET) and the review submission form (POST).
/// Also carries the existing reviews feed shown alongside the form.
/// </summary>
public sealed class ReviewViewModel
{
    // ── Hidden form fields ────────────────────────────────────────────────────

    /// <summary>The product being reviewed.</summary>
    public int ProductId { get; set; }

    /// <summary>The delivered order that contains the product.</summary>
    public int OrderId { get; set; }

    // ── Display data (GET only) ───────────────────────────────────────────────

    /// <summary>Human-readable product name shown on the brand panel card.</summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>Category name shown on the brand panel card.</summary>
    public string ProductCategory { get; set; } = string.Empty;

    /// <summary>Brand name shown on the brand panel card.</summary>
    public string BrandName { get; set; } = string.Empty;

    /// <summary>Product SKU shown on the brand panel card (optional).</summary>
    public string? SKU { get; set; }

    /// <summary>Always true in this flow — set by ReviewService after verified-purchase check.</summary>
    public bool IsVerifiedPurchase { get; set; } = true;

    /// <summary>Existing reviews for the same product, shown in the reviews feed below the form.</summary>
    public IEnumerable<ReviewItemViewModel> ExistingReviews { get; set; } = [];

    // ── POST fields ───────────────────────────────────────────────────────────

    /// <summary>Star rating chosen by the customer (1–5). Required on POST.</summary>
    public int Rating { get; set; }

    /// <summary>Optional review comment text (max 1 000 chars). May be null on POST.</summary>
    public string? Comment { get; set; }
}

/// <summary>
/// View model for the paginated product reviews list page (Reviews.cshtml).
/// </summary>
public sealed class ProductReviewsViewModel
{
    /// <summary>The product whose reviews are being displayed.</summary>
    public int ProductId { get; set; }

    /// <summary>Human-readable product name for the page heading.</summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>Average star rating across all verified reviews (0 when no reviews exist).</summary>
    public double AverageRating { get; set; }

    /// <summary>Total number of reviews for this product across all pages.</summary>
    public int TotalCount { get; set; }

    /// <summary>Current 1-based page number.</summary>
    public int CurrentPage { get; set; } = 1;

    /// <summary>Total number of pages given TotalCount and the page size.</summary>
    public int TotalPages { get; set; } = 1;

    /// <summary>Reviews for the current page.</summary>
    public IReadOnlyList<ReviewItemViewModel> Reviews { get; set; } = [];
}
