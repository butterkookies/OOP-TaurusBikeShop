using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class CustomerController : Controller
    {
        // ─────────────────────────────────────────
        // TEMP: Dev Navigation Page
        // Visit /Customer/DevNav to see all pages
        // Remove this action before going to production
        // ─────────────────────────────────────────
        public IActionResult DevNav()
        {
            return Content(@"
<!DOCTYPE html>
<html>
<head>
    <title>Taurus Dev Nav</title>
    <style>
        body { background:#0B0B0B; font-family:sans-serif; display:flex;
               flex-direction:column; align-items:center; justify-content:center;
               min-height:100vh; margin:0; gap:16px; }
        h2   { color:#E10600; letter-spacing:0.2em; font-size:13px;
               text-transform:uppercase; margin-bottom:8px; }
        a    { display:block; width:260px; padding:14px 24px;
               background:#141414; border:1px solid #2a2a2a;
               color:#f0f0f0; text-decoration:none; border-radius:4px;
               font-size:14px; transition:border-color .2s, color .2s; }
        a:hover { border-color:#E10600; color:#E10600; }
        .badge { font-size:10px; color:#888; margin-left:8px; }
    </style>
</head>
<body>
    <h2>&#9650; Taurus — Dev Navigation</h2>
    <a href=""/Customer/Login"">Login <span class=""badge"">✓ Layout=null</span></a>
    <a href=""/Customer/Register"">Register <span class=""badge"">✓ Layout=null</span></a>
    <a href=""/Customer/ProductCatalog"">Product Catalog <span class=""badge"">Uses _Layout</span></a>
</body>
</html>", "text/html");
        }

        // ─────────────────────────────────────────
        // Customer Pages
        // ─────────────────────────────────────────

        // GET /Customer/Register
        public IActionResult Register()
        {
            return View(); // Views/Customer/Register.cshtml
        }

        // GET /Customer/Login
        public IActionResult Login()
        {
            return View(); // Views/Customer/Login.cshtml
        }

        // GET /Customer/ProductCatalog
        public IActionResult ProductCatalog()
        {
            return View(); // Views/Customer/ProductCatalog.cshtml
        }

        // Add other pages like Cart, Checkout, etc. as needed
    }
}
