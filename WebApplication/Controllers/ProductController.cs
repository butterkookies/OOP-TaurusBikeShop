using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;

namespace WebApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET /Product  (all active products — API-style for AJAX)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllActiveProductsAsync();
            return Json(products.Select(p => new
            {
                p.ProductId,
                p.Name,
                p.ShortDescription,
                p.Price,
                p.Currency,
                p.StockQuantity,
                p.IsFeatured
            }));
        }

        // GET /Product/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // GET /Product/Search?query=...
        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var results = await _productService.SearchProductsAsync(query);
            return Json(results.Select(p => new
            {
                p.ProductId,
                p.Name,
                p.Price,
                p.ShortDescription
            }));
        }
    }
}
