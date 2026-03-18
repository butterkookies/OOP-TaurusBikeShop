// WebApplication/Models/ViewModels/ProfileViewModel.cs

using System.ComponentModel.DataAnnotations;
using WebApplication.Models.Entities;

namespace WebApplication.Models.ViewModels;

/// <summary>
/// ViewModel for the customer profile view and edit form.
/// Contains personal info, saved addresses, and the change-password sub-model.
/// </summary>
public sealed class ProfileViewModel
{
    /// <summary>The user's current first name.</summary>
    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>The user's current last name.</summary>
    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(100)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Email address — displayed read-only.
    /// Cannot be changed after registration.
    /// </summary>
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    /// <summary>Philippine mobile phone number.</summary>
    [MaxLength(20)]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// All saved (non-snapshot) addresses for this user.
    /// Used to render the address management section.
    /// </summary>
    public IReadOnlyList<Address> SavedAddresses { get; set; } = [];

    /// <summary>
    /// New address fields for adding an address from the profile page.
    /// Populated only when the user submits the add-address form.
    /// </summary>
    public NewAddressModel? NewAddress { get; set; }

    /// <summary>
    /// Change-password sub-model. Populated only when the user
    /// submits the change-password form.
    /// </summary>
    public ChangePasswordModel? ChangePassword { get; set; }

    /// <summary>Sub-model for adding a new address from the profile page.</summary>
    public sealed class NewAddressModel
    {
        [Required(ErrorMessage = "Street address is required.")]
        [MaxLength(500)]
        [Display(Name = "Street")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(100)]
        [Display(Name = "City")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal code is required.")]
        [MaxLength(20)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = string.Empty;

        [MaxLength(100)]
        [Display(Name = "Province")]
        public string? Province { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Country")]
        public string Country { get; set; } = "Philippines";

        [Required]
        [MaxLength(50)]
        [Display(Name = "Label")]
        public string Label { get; set; } = AddressLabels.Home;
    }

    /// <summary>Sub-model for the change-password form section.</summary>
    public sealed class ChangePasswordModel
    {
        [Required(ErrorMessage = "Current password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your new password.")]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}