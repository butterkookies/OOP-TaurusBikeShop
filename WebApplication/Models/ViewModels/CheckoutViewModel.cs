using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.ViewModels
{
    public class CheckoutViewModel
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Zip { get; set; } = string.Empty;

        [Required]
        public string PaymentMethod { get; set; } = "gcash";

        public decimal Subtotal { get; set; }
        public decimal ShippingFee { get; set; } = 150m;
        public decimal Total => Subtotal + ShippingFee;
    }
}
