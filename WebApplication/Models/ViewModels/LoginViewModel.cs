// WebApplication/Models/ViewModels/LoginViewModel.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.ViewModels;

/// <summary>
/// ViewModel for the customer login form.
/// Validated by data annotations at the ViewModel level;
/// credential verification is performed by <c>UserService.LoginAsync</c>.
/// </summary>
public sealed class LoginViewModel
{
    /// <summary>
    /// The customer's registered email address.
    /// </summary>
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [MaxLength(255)]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The customer's account password.
    /// Never stored or logged — passed directly to BCrypt verification.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// URL to redirect to after a successful login.
    /// Set by ASP.NET Core when an unauthenticated user attempts to
    /// access a protected route.
    /// </summary>
    public string? ReturnUrl { get; set; }
}