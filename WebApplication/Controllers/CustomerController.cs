// WebApplication/Controllers/CustomerController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Handles all customer authentication and account management routes.
/// Flowchart: Part 1 — Customer Registration &amp; Login.
/// </summary>
public sealed class CustomerController : Controller
{
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;
    private readonly IWishlistService _wishlistService;
    private readonly IReviewService _reviewService;
    private readonly ISupportService _supportService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<CustomerController> _logger;

    private const string TempDataRegisterVm  = "RegisterViewModel";
    private const string TempDataRegisterEmail = "PendingOTPEmail";

    /// <inheritdoc/>
    public CustomerController(
        IUserService userService,
        IOrderService orderService,
        IWishlistService wishlistService,
        IReviewService reviewService,
        ISupportService supportService,
        INotificationService notificationService,
        ILogger<CustomerController> logger)
    {
        _userService          = userService          ?? throw new ArgumentNullException(nameof(userService));
        _orderService         = orderService         ?? throw new ArgumentNullException(nameof(orderService));
        _wishlistService      = wishlistService      ?? throw new ArgumentNullException(nameof(wishlistService));
        _reviewService        = reviewService        ?? throw new ArgumentNullException(nameof(reviewService));
        _supportService       = supportService       ?? throw new ArgumentNullException(nameof(supportService));
        _notificationService  = notificationService  ?? throw new ArgumentNullException(nameof(notificationService));
        _logger               = logger               ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // REGISTER
    // =========================================================================

    /// <summary>GET /Customer/Register — renders the registration form.</summary>
    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(Dashboard));

        // After a successful Register POST we redirect here (PRG pattern).
        // If TempData signals the OTP modal should open, pass that through.
        // Use Peek so TempData survives for the subsequent VerifyOTP POST.
        bool showOtp = TempData["ShowOTPModal"] is true;
        string? otpEmail = TempData.Peek(TempDataRegisterEmail) as string;

        var vm = new RegisterViewModel();
        if (showOtp && otpEmail != null)
        {
            // Keep the registration data alive for VerifyOTP / ResendOTP
            TempData.Keep(TempDataRegisterVm);
            TempData.Keep(TempDataRegisterEmail);

            ViewBag.ShowOTPModal = true;
            ViewBag.OTPEmail     = otpEmail;
        }

        return View(vm);
    }

    /// <summary>
    /// POST /Customer/Register — validates the form, sends OTP, and redirects
    /// back to the GET page (PRG) so a browser refresh doesn't re-POST.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(
        RegisterViewModel vm,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(vm);

        try
        {
            ServiceResult result = await _userService.RegisterAsync(vm, cancellationToken);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                return View(vm);
            }

            // Store the ViewModel in TempData so VerifyOTP can use it to
            // create the account after the code is confirmed.
            TempData[TempDataRegisterVm]    = System.Text.Json.JsonSerializer.Serialize(vm);
            TempData[TempDataRegisterEmail] = vm.Email;
            TempData["ShowOTPModal"]        = true;

            // PRG: redirect to GET so browser refresh won't re-POST the form
            return RedirectToAction(nameof(Register));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Registration failed for email {Email}.", vm.Email);
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
            return View(vm);
        }
    }

    /// <summary>
    /// POST /Customer/VerifyOTP — verifies the submitted OTP code,
    /// creates the user account, signs the user in, and redirects to Dashboard.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyOTP(
        string email,
        string code,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
        {
            TempData["error"] = "Verification code is required.";
            return RedirectToAction(nameof(Register));
        }

        // Retrieve the stored RegisterViewModel
        string? serialisedVm = TempData[TempDataRegisterVm] as string;
        if (serialisedVm is null)
        {
            TempData["error"] = "Registration session expired. Please register again.";
            return RedirectToAction(nameof(Register));
        }

        RegisterViewModel? vm = System.Text.Json.JsonSerializer.Deserialize<RegisterViewModel>(serialisedVm);
        if (vm is null)
        {
            TempData["error"] = "Registration session expired. Please register again.";
            return RedirectToAction(nameof(Register));
        }

        try
        {
            ServiceResult<User> result =
                await _userService.VerifyOTPAndCreateAccountAsync(email, code.Trim(), vm, cancellationToken);

            if (!result.IsSuccess)
            {
                TempData["error"] = result.Error;
                // Keep registration session alive and re-show the OTP modal
                TempData[TempDataRegisterVm]    = serialisedVm;
                TempData[TempDataRegisterEmail] = email;
                TempData["ShowOTPModal"]        = true;

                return RedirectToAction(nameof(Register));
            }

            await SignInUserAsync(result.Value!);
            TempData["success"] = $"Welcome to Taurus Bike Shop, {result.Value!.FirstName}!";
            return RedirectToAction(nameof(Dashboard));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OTP verification failed for email {Email}.", email);
            TempData["error"] = "An unexpected error occurred. Please try again.";
            return RedirectToAction(nameof(Register));
        }
    }

    /// <summary>
    /// POST /Customer/ResendOTP — resends a fresh OTP to the given email
    /// (and via SMS if the phone number can be recovered from the stored
    /// registration session). Returns JSON for the AJAX call in _OTPModal.cshtml.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResendOTP(
        string email,
        CancellationToken cancellationToken)
    {
        try
        {
            // Recover the phone number from the stored registration ViewModel so
            // the SMS OTP is also resent. Peek() reads TempData without consuming
            // it, keeping the ViewModel available for subsequent VerifyOTP and
            // ResendOTP calls in the same registration session.
            string? phone = null;
            if (TempData.Peek(TempDataRegisterVm) is string serialisedVm)
            {
                RegisterViewModel? storedVm =
                    System.Text.Json.JsonSerializer.Deserialize<RegisterViewModel>(serialisedVm);
                phone = storedVm?.PhoneNumber;
            }

            ServiceResult result = await _userService.ResendOTPAsync(email, phone, cancellationToken);
            return Json(result.IsSuccess
                ? ApiResponse.Ok(message: "A new code has been sent to your email and phone.")
                : ApiResponse.Fail(result.Error!));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ResendOTP failed for {Email}.", email);
            return Json(ApiResponse.Fail("Failed to resend code. Please try again."));
        }
    }

    // =========================================================================
    // LOGIN / LOGOUT
    // =========================================================================

    /// <summary>GET /Customer/Login — renders the login form.</summary>
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(Dashboard));

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    /// <summary>
    /// POST /Customer/Login — validates credentials and signs the user in.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginViewModel vm,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(vm);

        try
        {
            ServiceResult<User> result =
                await _userService.LoginAsync(vm.Email, vm.Password, cancellationToken);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                return View(vm);
            }

            await SignInUserAsync(result.Value!);

            string redirect = !string.IsNullOrWhiteSpace(vm.ReturnUrl)
                && Url.IsLocalUrl(vm.ReturnUrl)
                    ? vm.ReturnUrl
                    : Url.Action(nameof(Dashboard))!;

            return Redirect(redirect);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for {Email}.", vm.Email);
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
            return View(vm);
        }
    }

    /// <summary>GET /Customer/Logout — signs the user out and redirects to homepage.</summary>
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["success"] = "You have been signed out.";
        return RedirectToAction("Index", "Home");
    }

    // =========================================================================
    // DASHBOARD
    // =========================================================================

    /// <summary>
    /// GET /Customer/Dashboard — customer dashboard with recent orders,
    /// wishlist count, and quick links. Requires authentication.
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Dashboard(CancellationToken cancellationToken)
    {
        int userId = GetCurrentUserId();
        string firstName = User.Identity?.Name?.Split(' ').FirstOrDefault() ?? "Customer";

        // EF Core DbContext is not thread-safe, so queries must run sequentially.
        (IReadOnlyList<OrderViewModel> recentOrders, int totalOrders) =
            await _orderService.GetOrderHistoryAsync(userId, page: 1, pageSize: 5, cancellationToken);

        int wishlistCount =
            await _wishlistService.GetCountAsync(userId, cancellationToken);

        IReadOnlyList<ReviewViewModel> pendingReviews =
            await _reviewService.GetPendingReviewsAsync(userId, cancellationToken);

        IReadOnlyList<SupportTicketViewModel> tickets =
            await _supportService.GetByUserAsync(userId, cancellationToken);

        int activeOrders = recentOrders
            .Count(o => o.OrderStatus != "Delivered"
                     && o.OrderStatus != "Cancelled"
                     && o.OrderStatus != "Returned");

        int openTickets = tickets
            .Count(t => t.Status != "Resolved" && t.Status != "Closed");

        DashboardViewModel vm = new()
        {
            FirstName          = firstName,
            TotalOrders        = totalOrders,
            ActiveOrders       = activeOrders,
            WishlistCount      = wishlistCount,
            PendingReviewCount = pendingReviews.Count,
            OpenTicketCount    = openTickets,
            RecentOrders       = recentOrders,
            PendingReviews     = pendingReviews.Take(3).ToList().AsReadOnly()
        };

        ViewData["Title"] = "My Dashboard";
        return View(vm);
    }

    // =========================================================================
    // PROFILE
    // =========================================================================

    /// <summary>GET /Customer/Profile — renders the profile view/edit page.</summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile(CancellationToken cancellationToken)
    {
        int userId = GetCurrentUserId();
        ProfileViewModel? vm = await _userService.GetProfileAsync(userId, cancellationToken);

        if (vm is null)
        {
            TempData["error"] = "Unable to load profile.";
            return RedirectToAction(nameof(Dashboard));
        }

        ViewData["Title"] = "My Profile";
        return View(vm);
    }

    /// <summary>POST /Customer/Profile — updates personal info (name, phone).</summary>
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(
        ProfileViewModel vm,
        CancellationToken cancellationToken)
    {
        // Only validate the top-level profile fields, not sub-models.
        // The ChangePassword and NewAddress sub-models live on the same ViewModel
        // but are submitted by separate forms — strip their validation errors.
        foreach (var key in ModelState.Keys
                     .Where(k => k.StartsWith("ChangePassword", StringComparison.OrdinalIgnoreCase)
                              || k.StartsWith("NewAddress", StringComparison.OrdinalIgnoreCase))
                     .ToList())
        {
            ModelState.Remove(key);
        }

        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "My Profile";
            return View(vm);
        }

        int userId = GetCurrentUserId();

        try
        {
            ServiceResult result = await _userService.UpdateProfileAsync(userId, vm, cancellationToken);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                ViewData["Title"] = "My Profile";
                return View(vm);
            }

            // Re-issue the auth cookie so ClaimTypes.Name reflects the new name
            // immediately — without this the navbar still shows the old name until
            // the user logs out and back in.
            User? updatedUser = await _userService.GetUserByIdAsync(userId, cancellationToken);
            if (updatedUser != null)
                await SignInUserAsync(updatedUser);

            TempData["success"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Profile));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Profile update failed for user {UserId}.", userId);
            TempData["error"] = "An unexpected error occurred. Please try again.";
            return RedirectToAction(nameof(Profile));
        }
    }

    /// <summary>POST /Customer/ChangePassword — changes the user's password.</summary>
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(
        [Bind(Prefix = "ChangePassword")] ProfileViewModel.ChangePasswordModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["error"] = "Please correct the password form errors.";
            return RedirectToAction(nameof(Profile));
        }

        int userId = GetCurrentUserId();

        try
        {
            ServiceResult result = await _userService.ChangePasswordAsync(
                userId, model.CurrentPassword, model.NewPassword, cancellationToken);

            if (!result.IsSuccess)
            {
                TempData["error"] = result.Error;
                return RedirectToAction(nameof(Profile));
            }

            TempData["success"] = "Password changed successfully.";
            return RedirectToAction(nameof(Profile));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Password change failed for user {UserId}.", userId);
            TempData["error"] = "An unexpected error occurred. Please try again.";
            return RedirectToAction(nameof(Profile));
        }
    }

    /// <summary>POST /Customer/AddAddress — adds a new saved address.</summary>
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAddress(
        [Bind(Prefix = "NewAddress")] ProfileViewModel.NewAddressModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["error"] = "Please correct the address form errors.";
            TempData["ReopenAddressModal"] = true;
            TempData["AddressForm"] = System.Text.Json.JsonSerializer.Serialize(model);
            return RedirectToAction(nameof(Profile));
        }

        int userId = GetCurrentUserId();

        try
        {
            ServiceResult result = await _userService.AddAddressAsync(userId, model, cancellationToken);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Error;
                TempData["ReopenAddressModal"] = true;
                TempData["AddressForm"] = System.Text.Json.JsonSerializer.Serialize(model);
            }
            else
            {
                TempData["success"] = "Address added successfully.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddAddress failed for user {UserId}.", userId);
            TempData["error"] = "An unexpected error occurred.";
            TempData["ReopenAddressModal"] = true;
            TempData["AddressForm"] = System.Text.Json.JsonSerializer.Serialize(model);
        }

        return RedirectToAction(nameof(Profile));
    }

    /// <summary>POST /Customer/DeleteAddress — removes a saved address.</summary>
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAddress(
        int addressId,
        CancellationToken cancellationToken)
    {
        int userId = GetCurrentUserId();

        try
        {
            ServiceResult result = await _userService.DeleteAddressAsync(userId, addressId, cancellationToken);
            TempData[result.IsSuccess ? "success" : "error"] =
                result.IsSuccess ? "Address removed." : result.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteAddress failed for user {UserId}, address {AddressId}.", userId, addressId);
            TempData["error"] = "An unexpected error occurred.";
        }

        return RedirectToAction(nameof(Profile));
    }

    /// <summary>POST /Customer/SetDefaultAddress — marks an address as default.</summary>
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetDefaultAddress(
        int addressId,
        CancellationToken cancellationToken)
    {
        int userId = GetCurrentUserId();

        try
        {
            ServiceResult result = await _userService.SetDefaultAddressAsync(userId, addressId, cancellationToken);
            TempData[result.IsSuccess ? "success" : "error"] =
                result.IsSuccess ? "Default address updated." : result.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SetDefaultAddress failed for user {UserId}.", userId);
            TempData["error"] = "An unexpected error occurred.";
        }

        return RedirectToAction(nameof(Profile));
    }

    // =========================================================================
    // Notifications
    // =========================================================================

    /// <summary>GET /Customer/Notifications — paginated notification history.</summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Notifications(
        int page = 1, CancellationToken cancellationToken = default)
    {
        int userId = GetCurrentUserId();
        const int pageSize = 20;

        (IReadOnlyList<Models.Entities.Notification> items, int totalCount) =
            await _notificationService.GetNotificationsForUserAsync(
                userId, page, pageSize, cancellationToken);

        int unreadCount = await _notificationService.GetUnreadCountAsync(
            userId, cancellationToken);

        NotificationListViewModel vm = new()
        {
            Notifications = items.Select(n => new NotificationViewModel
            {
                NotificationId = n.NotificationId,
                NotifType      = n.NotifType,
                Subject        = n.Subject,
                Body           = n.Body,
                Status         = n.Status,
                IsRead         = n.IsRead,
                CreatedAt      = n.CreatedAt,
                OrderId        = n.OrderId,
                TicketId       = n.TicketId
            }).ToList().AsReadOnly(),
            TotalCount   = totalCount,
            CurrentPage  = page,
            PageSize     = pageSize,
            UnreadCount  = unreadCount
        };

        ViewData["Title"] = "Notifications";
        return View(vm);
    }

    /// <summary>GET /Customer/NotificationCount — JSON endpoint for AJAX badge.</summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> NotificationCount(CancellationToken cancellationToken)
    {
        int userId = GetCurrentUserId();
        int count = await _notificationService.GetUnreadCountAsync(userId, cancellationToken);
        return Json(new { count });
    }

    /// <summary>POST /Customer/MarkNotificationRead — marks a single notification as read.</summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> MarkNotificationRead(
        int notificationId, CancellationToken cancellationToken)
    {
        int userId = GetCurrentUserId();
        await _notificationService.MarkAsReadAsync(notificationId, userId, cancellationToken);
        return RedirectToAction(nameof(Notifications));
    }

    /// <summary>POST /Customer/MarkAllNotificationsRead — marks all notifications as read.</summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> MarkAllNotificationsRead(CancellationToken cancellationToken)
    {
        int userId = GetCurrentUserId();
        await _notificationService.MarkAllAsReadAsync(userId, cancellationToken);
        return RedirectToAction(nameof(Notifications));
    }

    // =========================================================================
    // Private helpers
    // =========================================================================

    /// <summary>
    /// Creates a cookie-based authentication session for the given user.
    /// Claims: NameIdentifier (UserId), Name (FullName), Email.
    /// </summary>
    private async Task SignInUserAsync(User user)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name,           $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Email,          user.Email ?? string.Empty)
        ];

        ClaimsIdentity identity = new(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        ClaimsPrincipal principal = new(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = false });
    }

    /// <summary>
    /// Reads the current authenticated user's ID from the NameIdentifier claim.
    /// </summary>
    private int GetCurrentUserId()
    {
        string? value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out int id) ? id : 0;
    }
}