// WebApplication/BusinessLogic/Services/ProductService.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Services;

/// <summary>
/// Implements <see cref="IProductService"/> — product listing, filtering,
/// detail loading, and variant price/stock queries.
/// Flowchart: Part 2 — Customer Dashboard &amp; Shopping.
/// </summary>
public sealed class ProductService : IProductService
{
    private readonly ProductRepository  _productRepo;
    private readonly ReviewRepository   _reviewRepo;
    private readonly WishlistRepository _wishlistRepo;

    private const int ReviewPageSize = 5;

    /// <inheritdoc/>
    public ProductService(
        ProductRepository  productRepo,
        ReviewRepository   reviewRepo,
        WishlistRepository wishlistRepo)
    {
        _productRepo  = productRepo  ?? throw new ArgumentNullException(nameof(productRepo));
        _reviewRepo   = reviewRepo   ?? throw new ArgumentNullException(nameof(reviewRepo));
        _wishlistRepo = wishlistRepo ?? throw new ArgumentNullException(nameof(wishlistRepo));
    }

    /// <inheritdoc/>
    public async Task<(IReadOnlyList<ProductViewModel> Products, int TotalCount)> GetFilteredAsync(
        int? categoryId,
        int? brandId,
        decimal? minPrice,
        decimal? maxPrice,
        string? searchText,
        int page,
        int pageSize,
        IReadOnlyCollection<int> wishlistProductIds,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Product> products = await _productRepo.GetFilteredAsync(
            categoryId, brandId, minPrice, maxPrice, searchText,
            page, pageSize, cancellationToken);

        int totalCount = await _productRepo.GetFilteredCountAsync(
            categoryId, brandId, minPrice, maxPrice, searchText, cancellationToken);

        IReadOnlyList<ProductViewModel> viewModels = products
            .Select(p => MapToViewModel(p, wishlistProductIds))
            .ToList()
            .AsReadOnly();

        return (viewModels, totalCount);
    }

    /// <inheritdoc/>
    public async Task<ProductDetailViewModel?> GetByIdAsync(
        int productId,
        int? userId,
        CancellationToken cancellationToken = default)
    {
        Product? product = await _productRepo.GetWithFullDetailsAsync(productId, cancellationToken);
        if (product is null) return null;

        // Load first page of reviews
        IReadOnlyList<Review> reviews =
            await _reviewRepo.GetByProductAsync(productId, 1, ReviewPageSize, cancellationToken);

        decimal avgRating   = await _reviewRepo.GetAverageRatingAsync(productId, cancellationToken);
        int     reviewCount = await _reviewRepo.GetReviewCountAsync(productId, cancellationToken);

        bool isInWishlist = false;
        bool canReview    = false;

        if (userId.HasValue)
        {
            IReadOnlyList<int> wishlistIds =
                await _wishlistRepo.GetProductIdsForUserAsync(userId.Value, cancellationToken);
            isInWishlist = wishlistIds.Contains(productId);
            canReview    = await _reviewRepo.HasVerifiedPurchaseAsync(userId.Value, productId, cancellationToken);
        }

        return new ProductDetailViewModel
        {
            ProductId          = product.ProductId,
            Name               = product.Name,
            Description        = product.Description,
            ShortDescription   = product.ShortDescription,
            Price              = product.Price,
            Brand              = product.Brand?.BrandName,
            Category           = product.Category.Name,
            CategoryCode       = product.Category.CategoryCode,
            Material           = product.Material,
            Color              = product.Color,
            WheelSize          = product.WheelSize,
            SpeedCompatibility = product.SpeedCompatibility,
            BoostCompatible    = product.BoostCompatible,
            TubelessReady      = product.TubelessReady,
            AxleStandard       = product.AxleStandard,
            SuspensionTravel   = product.SuspensionTravel,
            BrakeType          = product.BrakeType,
            AdditionalSpecs    = product.AdditionalSpecs,
            Variants           = product.Variants.Where(v => v.IsActive).ToList().AsReadOnly(),
            Images             = product.Images
                .OrderByDescending(i => i.IsPrimary)
                .ThenBy(i => i.DisplayOrder)
                .ToList().AsReadOnly(),
            Reviews            = reviews.Select(MapReviewToViewModel).ToList().AsReadOnly(),
            AverageRating      = avgRating,
            ReviewCount        = reviewCount,
            SelectedVariantId  = product.Variants.FirstOrDefault(v => v.IsActive)?.ProductVariantId,
            IsInWishlist       = isInWishlist,
            CanReview          = canReview
        };
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ProductViewModel>> GetFeaturedAsync(
        int count,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Product> products =
            await _productRepo.GetFeaturedAsync(count, cancellationToken);

        return products
            .Select(p => MapToViewModel(p, []))
            .ToList()
            .AsReadOnly();
    }

    /// <inheritdoc/>
    public async Task<(decimal TotalPrice, int StockQuantity)?> GetVariantPriceAsync(
        int variantId,
        CancellationToken cancellationToken = default)
    {
        ProductVariant? variant = await _productRepo.Context.ProductVariants
            .AsNoTracking()
            .Include(v => v.Product)
            .FirstOrDefaultAsync(v => v.ProductVariantId == variantId && v.IsActive, cancellationToken);

        if (variant is null) return null;

        decimal totalPrice = variant.Product.Price + variant.AdditionalPrice;
        return (totalPrice, variant.StockQuantity);
    }

    /// <inheritdoc/>
    public async Task<bool> CheckStockAsync(
        int variantId,
        int requestedQty,
        CancellationToken cancellationToken = default)
    {
        ProductVariant? variant = await _productRepo.Context.ProductVariants
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.ProductVariantId == variantId && v.IsActive, cancellationToken);

        return variant != null && variant.StockQuantity >= requestedQty;
    }


    /// <inheritdoc/>
    public async Task<Models.Entities.Category?> GetCategoryByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return await _productRepo.Context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CategoryCode == code && c.IsActive, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Models.Entities.Category>> GetActiveCategoriesAsync(
        CancellationToken cancellationToken = default)
        => await _productRepo.GetActiveCategoriesAsync(cancellationToken);

    // =========================================================================
    // Private mapping helpers
    // =========================================================================

    private static ProductViewModel MapToViewModel(
        Product product,
        IReadOnlyCollection<int> wishlistProductIds)
    {
        int totalStock = product.Variants
            .Where(v => v.IsActive)
            .Sum(v => v.StockQuantity);

        string? primaryImageUrl = product.Images
            .FirstOrDefault(i => i.IsPrimary)?.ImageUrl
            ?? product.Images.FirstOrDefault()?.ImageUrl;

        return new ProductViewModel
        {
            ProductId       = product.ProductId,
            Name            = product.Name,
            ShortDescription= product.ShortDescription,
            Price           = product.Price,
            Brand           = product.Brand?.BrandName,
            Category        = product.Category.Name,
            CategoryCode    = product.Category.CategoryCode,
            PrimaryImageUrl = primaryImageUrl,
            StockQuantity   = totalStock,
            IsInWishlist    = wishlistProductIds.Contains(product.ProductId),
            IsFeatured      = product.IsFeatured
        };
    }

    private static ReviewViewModel MapReviewToViewModel(Review review) => new()
    {
        ReviewId           = review.ReviewId,
        ProductId          = review.ProductId,
        ProductName        = review.Product?.Name ?? string.Empty,
        OrderId            = review.OrderId,
        Rating             = review.Rating,
        Comment            = review.Comment,
        IsVerifiedPurchase = review.IsVerifiedPurchase,
        ReviewerName       = review.User != null
            ? $"{review.User.FirstName} {review.User.LastName[0]}."
            : "Customer",
        CreatedAt          = review.CreatedAt.ToString("MMMM d, yyyy")
    };
}