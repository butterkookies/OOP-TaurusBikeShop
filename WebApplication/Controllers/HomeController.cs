// WebApplication/Controllers/HomeController.cs

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Handles the landing page and privacy page.
/// No authentication required for any action in this controller.
/// </summary>
public sealed class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ILogger<HomeController> _logger;

    private const int FeaturedProductCount = 8;

    /// <inheritdoc/>
    public HomeController(
        IProductService productService,
        ILogger<HomeController> logger)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _logger         = logger         ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // GET /
    // GET /Home/Index
    // =========================================================================

    /// <summary>
    /// Renders the homepage with featured products.
    /// Unauthenticated access is permitted.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        try
        {
            IReadOnlyList<ProductViewModel> featured =
                await _productService.GetFeaturedAsync(FeaturedProductCount, cancellationToken);

            return View(featured);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load featured products for homepage.");
            return View(Array.Empty<ProductViewModel>());
        }
    }

    // =========================================================================
    // GET /Home/Privacy
    // =========================================================================

    /// <summary>Renders the static privacy policy page.</summary>
    [HttpGet]
    public IActionResult Privacy() => View();

    // =========================================================================
    // GET /Home/Error
    // =========================================================================

    /// <summary>
    /// Unhandled exception fallback. Retrieves the exception context from
    /// the <see cref="IExceptionHandlerPathFeature"/> and renders the error view.
    /// </summary>
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        IExceptionHandlerPathFeature? exceptionFeature =
            HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionFeature?.Error != null)
        {
            _logger.LogError(
                exceptionFeature.Error,
                "Unhandled exception on path: {Path}",
                exceptionFeature.Path);
        }

        return View(new ErrorViewModel
        {
            RequestId = HttpContext.TraceIdentifier
        });
    }
}