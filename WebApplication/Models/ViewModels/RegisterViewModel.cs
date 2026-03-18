// WebApplication/Models/ViewModels/RegisterViewModel.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.ViewModels;

/// <summary>
/// ViewModel for the customer registration form.
/// Validated by data annotations at the ViewModel level;
/// OTP sending and account creation are performed by <c>UserService</c>.
/// </summary>
public sealed class RegisterViewModel
{
    /// <summary>The customer's first name.</summary>
    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>The customer's last name.</summary>
    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(100)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// The customer's email address.
    /// Used as the login identifier and for OTP delivery.
    /// </summary>
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [MaxLength(255)]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Philippine mobile phone number.
    /// Validated by <c>ValidationHelper.IsValidPhilippinePhone</c>
    /// in <c>CustomerController</c>.
    /// </summary>
    [Required(ErrorMessage = "Phone number is required.")]
    [MaxLength(20)]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Account password. Must be at least 8 characters.
    /// Hashed with BCrypt work factor 12 by <c>PasswordHelper.Hash</c>.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Confirm password — must match <see cref="Password"/>.
    /// </summary>
    [Required(ErrorMessage = "Please confirm your password.")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // Address fields — mapped to an Address entity at account creation
    // -------------------------------------------------------------------------

    /// <summary>Street address line (house number, street name, barangay).</summary>
    [Required(ErrorMessage = "Street address is required.")]
    [MaxLength(500)]
    [Display(Name = "Street Address")]
    public string Street { get; set; } = string.Empty;

    /// <summary>City or municipality.</summary>
    [Required(ErrorMessage = "City is required.")]
    [MaxLength(100)]
    [Display(Name = "City")]
    public string City { get; set; } = string.Empty;

    /// <summary>4-digit Philippine postal code.</summary>
    [Required(ErrorMessage = "Postal code is required.")]
    [MaxLength(20)]
    [Display(Name = "Postal Code")]
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>Province or region. Optional for Metro Manila addresses.</summary>
    [MaxLength(100)]
    [Display(Name = "Province")]
    public string? Province { get; set; }

    /// <summary>Country. Defaults to Philippines.</summary>
    [MaxLength(100)]
    [Display(Name = "Country")]
    public string Country { get; set; } = "Philippines";
}