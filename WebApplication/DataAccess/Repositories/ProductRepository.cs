// WebApplication/DataAccess/Repositories/ProductRepository.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories;

/// <summary>
/// Data access for <see cref="Product"/>, <see cref="ProductVariant"/>,
/// and <see cref="ProductImage"/> entities.
/// Handles catalog listing with dynamic filtering, full product detail loading,
/// and featured product retrieval.
/// </summary>
public sealed class ProductRepository : Repository<Product>
{
    /// <inheritdoc/>
    public ProductRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Returns a paginated, filtered list of active products.
    /// All filter parameters are optional — omitting a parameter removes
    /// that filter from the query. <c>IsActive = true</c> is always applied.
    /// </summary>
    /// <param name="categoryId">Filter by category ID. NULL = all categories.</param>
    /// <param name="brandId">Filter by brand ID. NULL = all brands.</param>
    /// <param name="minPrice">Minimum base price (inclusive). NULL = no lower bound.</param>
    /// <param name="maxPrice">Maximum base price (inclusive). NULL = no upper bound.</param>
    /// <param name="searchText">
    /// Text to match against product Name and ShortDescription (case-insensitive).
    /// NULL or whitespace = no text filter.
    /// </param>
    /// <param name="page">1-based page number.</param>
    /// <param name="pageSize">Number of products per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A page of active products matching all supplied filters,
    /// ordered by <c>IsFeatured DESC</c> then <c>Name ASC</c>.
    /// Each product includes its primary image and active variants.
    /// </returns>
    public async Task<IReadOnlyList<Product>> GetFilteredAsync(
        int? categoryId,
        int? brandId,
        decimal? minPrice,
        decimal? maxPrice,
        string? searchText,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = Context.Products
            .AsNoTracking()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images.Where(i => i.IsPrimary))
            .Include(p => p.Variants.Where(v => v.IsActive))
            .Where(p => p.IsActive && p.Category.IsActive);

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (brandId.HasValue)
            query = query.Where(p => p.BrandId == brandId.Value);

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            string term = searchText.Trim();
            query = query.Where(p =>
                p.Name.Contains(term) ||
                (p.ShortDescription != null && p.ShortDescription.Contains(term)));
        }

        return await query
            .OrderByDescending(p => p.IsFeatured)
            .ThenBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns the total count of active products matching the given filters.
    /// Used to calculate pagination page count alongside <see cref="GetFilteredAsync"/>.
    /// Parameters mirror those of <see cref="GetFilteredAsync"/> exactly.
    /// </summary>
    public async Task<int> GetFilteredCountAsync(
        int? categoryId,
        int? brandId,
        decimal? minPrice,
        decimal? maxPrice,
        string? searchText,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = Context.Products
            .Where(p => p.IsActive && p.Category.IsActive);

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (brandId.HasValue)
            query = query.Where(p => p.BrandId == brandId.Value);

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            string term = searchText.Trim();
            query = query.Where(p =>
                p.Name.Contains(term) ||
                (p.ShortDescription != null && p.ShortDescription.Contains(term)));
        }

        return await query.CountAsync(cancellationToken);
    }

    /// <summary>
    /// Returns a single product with full detail loaded for the product detail page.
    /// Includes: all active variants, all images (primary first then by DisplayOrder),
    /// Brand, Category, and all reviews with their authors.
    /// Returns <c>null</c> if the product does not exist or is inactive.
    /// </summary>
    /// <param name="productId">The product ID to load.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Fully loaded active product, or <c>null</c>.</returns>
    public async Task<Product?> GetWithFullDetailsAsync(
        int productId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Products
            .AsNoTracking()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Variants.Where(v => v.IsActive))
            .Include(p => p.Images.OrderByDescending(i => i.IsPrimary)
                                  .ThenBy(i => i.DisplayOrder))
            .Include(p => p.PriceHistory.OrderByDescending(ph => ph.ChangedAt).Take(10))
            .FirstOrDefaultAsync(
                p => p.ProductId == productId && p.IsActive,
                cancellationToken);
    }

    /// <summary>
    /// Returns a product by its SKU for POS barcode scanning.
    /// Returns <c>null</c> when no active product with that SKU exists.
    /// </summary>
    /// <param name="sku">The SKU string to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching active product, or <c>null</c>.</returns>
    public async Task<Product?> GetBySKUAsync(
        string sku,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sku))
            return null;

        return await Context.Products
            .AsNoTracking()
            .Include(p => p.Variants.Where(v => v.IsActive))
            .Include(p => p.Images.Where(i => i.IsPrimary))
            .FirstOrDefaultAsync(
                p => p.SKU == sku && p.IsActive,
                cancellationToken);
    }

    /// <summary>
    /// Returns the top <paramref name="count"/> featured active products.
    /// Used by the homepage hero section.
    /// </summary>
    /// <param name="count">Maximum number of featured products to return.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Featured active products with primary image included.</returns>
    public async Task<IReadOnlyList<Product>> GetFeaturedAsync(
        int count,
        CancellationToken cancellationToken = default)
    {
        return await Context.Products
            .AsNoTracking()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images.Where(i => i.IsPrimary))
            .Include(p => p.Variants.Where(v => v.IsActive))
            .Where(p => p.IsActive && p.IsFeatured && p.Category.IsActive)
            .OrderBy(p => p.Name)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns all active categories ordered by <c>DisplayOrder</c>.
    /// Used to populate the category filter sidebar.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All active categories ordered for display.</returns>
    public async Task<IReadOnlyList<Category>> GetActiveCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        return await Context.Categories
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ThenBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }
}