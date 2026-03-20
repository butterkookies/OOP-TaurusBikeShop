using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IUserService    _userService;
        private readonly IProductService _productService;
        private readonly IOrderService   _orderService;
        private readonly IReviewService  _reviewService;
        private readonly ILogger<CustomerController> _logger;

        // Session key for logged-in user ID
        private const string SessionUserId   = "UserId";
        private const string SessionUserName = "UserName";

        // Session keys for OTP verification
        private const string SessionOtpCode  = "OtpCode";
        private const string SessionOtpPhone = "OtpPhone";
        private const string SessionOtpExpiry = "OtpExpiry";

        public CustomerController(IUserService userService, IProductService productService,
            IOrderService orderService, IReviewService reviewService,
            ILogger<CustomerController> logger)
        {
            _userService    = userService;
            _productService = productService;
            _orderService   = orderService;
            _reviewService  = reviewService;
            _logger         = logger;
        }

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
    <a href=""/Customer/Cart"">Cart</a>
    <a href=""/Customer/Checkout"">Checkout</a>
    <a href=""/Customer/OrderHistory"">Order History</a>
    <a href=""/Customer/Profile"">Profile</a>
    <a href=""/Customer/Dashboard"">Dashboard</a>
</body>
</html>", "text/html");
        }

        // ─────────────────────────────────────────
        // GET /Customer/Login
        // ─────────────────────────────────────────
        [HttpGet]
        public IActionResult Login()
        {
            if (IsLoggedIn()) return RedirectToAction("ProductCatalog");
            return View();
        }

        // POST /Customer/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userService.AuthenticateAsync(model.Email, model.Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            HttpContext.Session.SetInt32(SessionUserId, user.UserId);
            HttpContext.Session.SetString(SessionUserName, user.FullName);

            return RedirectToAction("ProductCatalog");
        }

        // ─────────────────────────────────────────
        // GET /Customer/Register
        // ─────────────────────────────────────────
        [HttpGet]
        public IActionResult Register()
        {
            if (IsLoggedIn()) return RedirectToAction("ProductCatalog");
            return View();
        }

        // POST /Customer/SendOtp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendOtp([FromForm] string contactNumber)
        {
            if (string.IsNullOrWhiteSpace(contactNumber))
                return Json(new { success = false, message = "Phone number is required." });

            var phone  = contactNumber.Trim();
            var otp    = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.UtcNow.AddMinutes(5).Ticks.ToString();

            HttpContext.Session.SetString(SessionOtpCode,   otp);
            HttpContext.Session.SetString(SessionOtpPhone,  phone);
            HttpContext.Session.SetString(SessionOtpExpiry, expiry);

            // In production, integrate an SMS gateway here.
            // For dev: OTP is returned in the JSON response and logged to the console.
            _logger.LogInformation("[OTP] Phone={Phone} Code={Code} (expires in 5 min)", phone, otp);

            var isDev = HttpContext.RequestServices
                .GetRequiredService<IWebHostEnvironment>().IsDevelopment();

            return Json(new { success = true, otp = isDev ? otp : (string?)null });
        }

        // POST /Customer/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // ── OTP validation ──────────────────────────────────────
            var sessionOtp   = HttpContext.Session.GetString(SessionOtpCode);
            var sessionPhone = HttpContext.Session.GetString(SessionOtpPhone);
            var expiryStr    = HttpContext.Session.GetString(SessionOtpExpiry);

            if (string.IsNullOrEmpty(sessionOtp))
            {
                ModelState.AddModelError(string.Empty, "Please request and enter an OTP before submitting.");
                return View(model);
            }

            if (long.TryParse(expiryStr, out long expiryTicks) && DateTime.UtcNow.Ticks > expiryTicks)
            {
                ModelState.AddModelError(string.Empty, "OTP has expired. Please request a new one.");
                return View(model);
            }

            if (sessionPhone != model.ContactNumber.Trim())
            {
                ModelState.AddModelError(string.Empty, "OTP was sent to a different phone number.");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.OtpCode) || model.OtpCode.Trim() != sessionOtp)
            {
                ModelState.AddModelError(string.Empty, "Invalid OTP. Please check the code and try again.");
                return View(model);
            }

            // Clear OTP from session after successful validation
            HttpContext.Session.Remove(SessionOtpCode);
            HttpContext.Session.Remove(SessionOtpPhone);
            HttpContext.Session.Remove(SessionOtpExpiry);
            // ───────────────────────────────────────────────────────

            var (success, error) = await _userService.RegisterAsync(model);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error);
                return View(model);
            }

            TempData["SuccessMessage"] = "Account created! Please sign in.";
            return RedirectToAction("Login");
        }

        // ─────────────────────────────────────────
        // GET /Customer/Logout
        // ─────────────────────────────────────────
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ─────────────────────────────────────────
        // GET /Customer/ProductCatalog
        // ─────────────────────────────────────────
        public IActionResult ProductCatalog()
        {
            return View();
        }

        // ─────────────────────────────────────────
        // GET /Customer/ProductDetails/{id}
        // ─────────────────────────────────────────
        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            var vm = new ProductViewModel
            {
                ProductId        = product.ProductId,
                Name             = product.Name,
                ShortDescription = product.ShortDescription,
                Description      = product.Description,
                Price            = product.Price,
                Currency         = product.Currency,
                StockQuantity    = product.StockQuantity,
                CategoryName     = product.Category?.Name,
                CategoryCode     = product.Category?.CategoryCode,
                IsFeatured       = product.IsFeatured,
                IsActive         = product.IsActive
            };

            int? userId = HttpContext.Session.GetInt32(SessionUserId);

            var reviews       = await _reviewService.GetProductReviewsAsync(id);
            bool canReview    = userId != null && await _reviewService.CanReviewAsync(userId.Value, id);
            double avgRating  = await _reviewService.GetAverageRatingAsync(id);

            ViewBag.Reviews       = reviews;
            ViewBag.CanReview     = canReview;
            ViewBag.AverageRating = avgRating;

            return View(vm);
        }

        // ─────────────────────────────────────────
        // GET /Customer/Cart
        // ─────────────────────────────────────────
        public IActionResult Cart()
        {
            return View();
        }

        // ─────────────────────────────────────────
        // GET /Customer/Checkout
        // ─────────────────────────────────────────
        [HttpGet]
        public IActionResult Checkout()
        {
            return View(new CheckoutViewModel());
        }

        // POST /Customer/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null) return RedirectToAction("Login");

            try
            {
                var order = await _orderService.PlaceOrderAsync(userId.Value, model);
                return RedirectToAction("Confirmation", new { orderNumber = order.OrderNumber });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        // ─────────────────────────────────────────
        // GET /Customer/Payment
        // ─────────────────────────────────────────
        public IActionResult Payment()
        {
            return View();
        }

        // ─────────────────────────────────────────
        // GET /Customer/Confirmation
        // ─────────────────────────────────────────
        public async Task<IActionResult> Confirmation(string orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber))
                return RedirectToAction("OrderHistory");

            // Pass orderNumber to the view
            ViewBag.OrderNumber = orderNumber;
            return View();
        }

        // ─────────────────────────────────────────
        // GET /Customer/OrderHistory
        // ─────────────────────────────────────────
        public async Task<IActionResult> OrderHistory()
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null) return RedirectToAction("Login");

            var orders = await _orderService.GetOrderHistoryAsync(userId.Value);
            return View(orders);
        }

        // ─────────────────────────────────────────
        // GET /Customer/Profile
        // ─────────────────────────────────────────
        public async Task<IActionResult> Profile()
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null) return RedirectToAction("Login");

            var user = await _userService.GetByIdAsync(userId.Value);
            if (user == null) return RedirectToAction("Login");

            ViewBag.FullName = user.FullName;
            ViewBag.Email    = user.Email;
            ViewBag.Phone    = user.PhoneNumber;
            ViewBag.Address  = user.Address;
            return View();
        }

        // ─────────────────────────────────────────
        // GET /Customer/Dashboard
        // ─────────────────────────────────────────
        public IActionResult Dashboard()
        {
            return View();
        }

        // ─────────────────────────────────────────
        private bool IsLoggedIn() => HttpContext.Session.GetInt32(SessionUserId) != null;
    }
}
