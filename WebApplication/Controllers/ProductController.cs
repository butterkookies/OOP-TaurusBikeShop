// WebApplication/Controllers/ProductController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Handles product catalog listing, search, filtering, detail pages,
/// and the AJAX variant price endpoint.
/// Flowchart: Part 2 — Customer Dashboard &amp; Shopping.
/// </summary>
public sealed class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IBrandService   _brandService;
    private readonly IWishlistService _wishlistService;
    private readonly ILogger<ProductController> _logger;

    private const int DefaultPageSize = 12;

    /// <inheritdoc/>
    public ProductController(
        IProductService   productService,
        IBrandService     brandService,
        IWishlistService  wishlistService,
        ILogger<ProductController> logger)
    {
        _productService  = productService  ?? throw new ArgumentNullException(nameof(productService));
        _brandService    = brandService    ?? throw new ArgumentNullException(nameof(brandService));
        _wishlistService = wishlistService ?? throw new ArgumentNullException(nameof(wishlistService));
        _logger          = logger          ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // GET /Product/List
    // =========================================================================

    /// <summary>
    /// Product catalog listing with optional filters, search, and pagination.
    /// Accessible to both authenticated and guest users.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> List(
        int?    categoryId   = null,
        int?    brandId      = null,
        decimal? minPrice    = null,
        decimal? maxPrice    = null,
        string? search       = null,
        string? categoryCode = null,
        bool    featured     = false,
        int     page         = 1,
        CancellationToken cancellationToken = default)
    {
        // Resolve categoryCode to categoryId when provided via URL
        if (!categoryId.HasValue && !string.IsNullOrWhiteSpace(categoryCode))
        {
            Category? cat = await _productService
                .GetCategoryByCodeAsync(categoryCode, cancellationToken);
            if (cat != null) categoryId = cat.CategoryId;
        }

        // Load wishlist product IDs for authenticated users
        IReadOnlyCollection<int> wishlistIds = [];
        int? userId = GetCurrentUserId();
        if (userId.HasValue)
        {
            wishlistIds = (IReadOnlyCollection<int>)
                await _wishlistService.GetProductIdsAsync(userId.Value, cancellationToken);
        }

        try
        {
            (IReadOnlyList<ProductViewModel> products, int totalCount) =
                await _productService.GetFilteredAsync(
                    categoryId, brandId, minPrice, maxPrice, search,
                    page, DefaultPageSize, wishlistIds, cancellationToken);

            IReadOnlyList<Brand> brands =
                await _brandService.GetAllActiveBrandsAsync(cancellationToken);

            IReadOnlyList<Category> categories =
                await _productService.GetActiveCategoriesAsync(cancellationToken);

            int totalPages = (int)Math.Ceiling(totalCount / (double)DefaultPageSize);

            ViewData["Title"]       = "Product Catalog";
            ViewBag.Brands          = brands;
            ViewBag.Categories      = categories;
            ViewBag.CurrentPage     = page;
            ViewBag.TotalPages      = totalPages;
            ViewBag.TotalCount      = totalCount;
            ViewBag.CategoryId      = categoryId;
            ViewBag.BrandId         = brandId;
            ViewBag.MinPrice        = minPrice;
            ViewBag.MaxPrice        = maxPrice;
            ViewBag.Search          = search;

            return View(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading product catalog.");
            TempData["error"] = "Unable to load products. Please try again.";
            return View(Array.Empty<ProductViewModel>());
        }
    }

    // =========================================================================
    // GET /Product/Detail/{id}
    // =========================================================================

    /// <summary>
    /// Single product detail page — loads variants, images, and first
    /// page of reviews.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Detail(
        int id,
        CancellationToken cancellationToken)
    {
        try
        {
            int? userId = GetCurrentUserId();
            ProductDetailViewModel? vm =
                await _productService.GetByIdAsync(id, userId, cancellationToken);

            if (vm is null)
            {
                TempData["error"] = "Product not found.";
                return RedirectToAction(nameof(List));
            }

            ViewData["Title"] = vm.Name;
            return View(vm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading product {ProductId}.", id);
            TempData["error"] = "Unable to load product. Please try again.";
            return RedirectToAction(nameof(List));
        }
    }

    // =========================================================================
    // AJAX GET /Product/GetVariantPrice/{variantId}
    // =========================================================================

    /// <summary>
    /// Returns the total price and stock level for a specific variant.
    /// Called by <c>product-catalog.js</c> when the user selects a variant.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetVariantPrice(
        int variantId,
        CancellationToken cancellationToken)
    {
        try
        {
            (decimal TotalPrice, int StockQuantity)? result =
                await _productService.GetVariantPriceAsync(variantId, cancellationToken);

            if (result is null)
                return Json(ApiResponse.Fail("Variant not found."));

            return Json(ApiResponse.Ok(new
            {
                price    = result.Value.TotalPrice,
                formattedPrice = $"₱{result.Value.TotalPrice:N2}",
                stock    = result.Value.StockQuantity,
                inStock  = result.Value.StockQuantity > 0
            }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching variant price for {VariantId}.", variantId);
            return Json(ApiResponse.Fail("Unable to retrieve variant details."));
        }
    }

    // =========================================================================
    // Private helpers
    // =========================================================================

    private int? GetCurrentUserId()
    {
        string? value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out int id) ? id : null;
    }
}