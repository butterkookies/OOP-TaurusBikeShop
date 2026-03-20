// WebApplication/Controllers/HomeController.cs

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Handles the homepage and shared static pages.
/// No authentication required.
/// </summary>
public sealed class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ILogger<HomeController> _logger;

    private const int FeaturedProductCount = 8;

    public HomeController(
        IProductService productService,
        ILogger<HomeController> logger)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _logger         = logger         ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // GET /
    // =========================================================================

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

    [HttpGet]
    public IActionResult Privacy() => View();

    // =========================================================================
    // GET /Home/Error
    // =========================================================================

    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        IExceptionHandlerPathFeature? feature =
            HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (feature?.Error != null)
            _logger.LogError(feature.Error,
                "Unhandled exception on path: {Path}", feature.Path);

        return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
    }
}