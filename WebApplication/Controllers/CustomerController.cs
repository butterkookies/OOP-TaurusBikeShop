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
    private readonly ILogger<CustomerController> _logger;

    private const string TempDataRegisterVm  = "RegisterViewModel";
    private const string TempDataRegisterEmail = "PendingOTPEmail";

    /// <inheritdoc/>
    public CustomerController(
        IUserService userService,
        ILogger<CustomerController> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger      = logger      ?? throw new ArgumentNullException(nameof(logger));
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

        return View(new RegisterViewModel());
    }

    /// <summary>
    /// POST /Customer/Register — validates the form, sends OTP, and displays
    /// the OTP verification modal on success.
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

            ViewBag.ShowOTPModal = true;
            ViewBag.OTPEmail     = vm.Email;
            return View(vm);
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
                // Re-expose the modal
                TempData[TempDataRegisterVm]    = serialisedVm;
                TempData[TempDataRegisterEmail] = email;

                RegisterViewModel returnVm = vm;
                ViewBag.ShowOTPModal = true;
                ViewBag.OTPEmail     = email;
                return View("Register", returnVm);
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
    /// POST /Customer/ResendOTP — resends a fresh OTP to the given email.
    /// Returns JSON for the AJAX call in _OTPModal.cshtml.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResendOTP(
        string email,
        CancellationToken cancellationToken)
    {
        try
        {
            ServiceResult result = await _userService.ResendOTPAsync(email, cancellationToken);
            return Json(result.IsSuccess
                ? ApiResponse.Ok(message: "A new code has been sent to your email.")
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
    /// Full DashboardViewModel is populated in Step 21.
    /// </summary>
    [HttpGet]
    [Authorize]
    public IActionResult Dashboard()
    {
        // DashboardViewModel will be fully wired in Step 21.
        // For now, pass the user's name through ViewBag.
        ViewData["Title"] = "My Dashboard";
        ViewBag.CustomerName = User.Identity?.Name ?? "Customer";
        return View();
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
        // Only validate the top-level profile fields, not sub-models
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
        ProfileViewModel.ChangePasswordModel model,
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
        ProfileViewModel.NewAddressModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["error"] = "Please correct the address form errors.";
            return RedirectToAction(nameof(Profile));
        }

        int userId = GetCurrentUserId();

        try
        {
            ServiceResult result = await _userService.AddAddressAsync(userId, model, cancellationToken);
            TempData[result.IsSuccess ? "success" : "error"] =
                result.IsSuccess ? "Address added successfully." : result.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddAddress failed for user {UserId}.", userId);
            TempData["error"] = "An unexpected error occurred.";
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