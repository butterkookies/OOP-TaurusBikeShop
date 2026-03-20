// WebApplication/Models/ViewModels/DashboardViewModel.cs

namespace WebApplication.Models.ViewModels;

/// <summary>
/// View model for the customer dashboard page.
/// Aggregates data from orders, wishlist, reviews, and support
/// into a single load so the dashboard controller makes one
/// service call per data source.
/// </summary>
public sealed class DashboardViewModel
{
    /// <summary>Customer's first name for the greeting.</summary>
    public string FirstName { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // Stat counters
    // -------------------------------------------------------------------------

    /// <summary>Total lifetime orders placed by this customer.</summary>
    public int TotalOrders { get; set; }

    /// <summary>Orders currently in an active (non-terminal) state.</summary>
    public int ActiveOrders { get; set; }

    /// <summary>Products saved in the wishlist.</summary>
    public int WishlistCount { get; set; }

    /// <summary>Products from delivered orders awaiting a review.</summary>
    public int PendingReviewCount { get; set; }

    /// <summary>Open or in-progress support tickets.</summary>
    public int OpenTicketCount { get; set; }

    // -------------------------------------------------------------------------
    // Recent orders (last 5) for the dashboard list
    // -------------------------------------------------------------------------

    /// <summary>
    /// The 5 most recent orders — used to render the "Recent Orders" section.
    /// Full pagination is available on the Order History page.
    /// </summary>
    public IReadOnlyList<OrderViewModel> RecentOrders { get; set; } = [];

    // -------------------------------------------------------------------------
    // Pending reviews (up to 3) for the dashboard prompt
    // -------------------------------------------------------------------------

    /// <summary>
    /// Up to 3 products awaiting a review — shown as a prompt strip
    /// beneath the stat cards.
    /// </summary>
    public IReadOnlyList<ReviewViewModel> PendingReviews { get; set; } = [];

    // -------------------------------------------------------------------------
    // Computed helpers
    // -------------------------------------------------------------------------

    /// <summary>True when the customer has no orders at all.</summary>
    public bool HasNoOrders => TotalOrders == 0;

    /// <summary>True when there are unreviewed delivered products.</summary>
    public bool HasPendingReviews => PendingReviewCount > 0;

    /// <summary>True when there are open support tickets.</summary>
    public bool HasOpenTickets => OpenTicketCount > 0;
}