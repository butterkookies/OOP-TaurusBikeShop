using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class CustomerController : Controller
    {
        // Render Register page
        public IActionResult Register()
        {
            return View(); // Serves Views/Customer/Register.cshtml
        }

        // Render Login page
        public IActionResult Login()
        {
            return View(); // Serves Views/Customer/Login.cshtml
        }

        // Example: Product Catalog
        public IActionResult ProductCatalog()
        {
            return View(); // Serves Views/Customer/ProductCatalog.cshtml
        }

        // Add other pages like Cart, Checkout, etc. as needed
    }
}