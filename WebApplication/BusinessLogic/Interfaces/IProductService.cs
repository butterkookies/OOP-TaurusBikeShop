// WebApplication/BusinessLogic/Interfaces/IProductService.cs

using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Interfaces;

/// <summary>
/// Contract for product catalog operations.
/// Flowchart: Part 2 — Customer Dashboard &amp; Shopping.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Returns a paginated, filtered list of active products as
    /// <see cref="ProductViewModel"/> cards for the catalog grid.
    /// All filter parameters are optional.
    /// </summary>
    /// <param name="categoryId">Filter by category. Null = all categories.</param>
    /// <param name="brandId">Filter by brand. Null = all brands.</param>
    /// <param name="minPrice">Minimum base price. Null = no lower bound.</param>
    /// <param name="maxPrice">Maximum base price. Null = no upper bound.</param>
    /// <param name="searchText">Text search against Name and ShortDescription.</param>
    /// <param name="page">1-based page number.</param>
    /// <param name="pageSize">Items per page.</param>
    /// <param name="wishlistProductIds">
    /// Set of product IDs in the authenticated user's wishlist,
    /// used to set <c>IsInWishlist</c> on each card.
    /// Pass an empty set for unauthenticated users.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<(IReadOnlyList<ProductViewModel> Products, int TotalCount)> GetFilteredAsync(
        int? categoryId,
        int? brandId,
        decimal? minPrice,
        decimal? maxPrice,
        string? searchText,
        int page,
        int pageSize,
        IReadOnlyCollection<int> wishlistProductIds,
        bool featuredOnly = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the full detail view model for a single product,
    /// including variants, images, and the first page of reviews.
    /// Returns <c>null</c> when the product does not exist or is inactive.
    /// </summary>
    /// <param name="productId">The product to load.</param>
    /// <param name="userId">
    /// The authenticated user's ID for wishlist and review eligibility checks.
    /// Null for unauthenticated users.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<ProductDetailViewModel?> GetByIdAsync(
        int productId,
        int? userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the top <paramref name="count"/> featured active products
    /// for the homepage hero section.
    /// </summary>
    Task<IReadOnlyList<ProductViewModel>> GetFeaturedAsync(
        int count,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the current price and stock level for a specific variant.
    /// Used by the AJAX variant selector on the product detail page.
    /// </summary>
    /// <param name="variantId">The variant to query.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A tuple of (TotalPrice, StockQuantity), or null if the variant
    /// does not exist or is inactive.
    /// </returns>
    Task<(decimal TotalPrice, int StockQuantity)?> GetVariantPriceAsync(
        int variantId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a specific variant has sufficient stock for the
    /// requested quantity. Used before adding to cart.
    /// </summary>
    Task<bool> CheckStockAsync(
        int variantId,
        int requestedQty,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// Returns a category by its unique code (e.g. UNIT, FRAME).
    /// Used by <c>ProductController</c> to resolve categoryCode query params.
    /// </summary>
    Task<WebApplication.Models.Entities.Category?> GetCategoryByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all active categories ordered by DisplayOrder.
    /// Used to populate the category filter sidebar.
    /// </summary>
    Task<IReadOnlyList<WebApplication.Models.Entities.Category>> GetActiveCategoriesAsync(
        CancellationToken cancellationToken = default);
}