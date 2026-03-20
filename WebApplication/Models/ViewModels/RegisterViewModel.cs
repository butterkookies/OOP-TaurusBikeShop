using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact number is required.")]
        [Phone(ErrorMessage = "Enter a valid phone number.")]
        public string ContactNumber { get; set; } = string.Empty;

        public string? OtpCode { get; set; }

        [Required(ErrorMessage = "Street address is required.")]
        public string StreetAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Province is required.")]
        public string Province { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal code is required.")]
        public string PostalCode { get; set; } = string.Empty;
    }
}
