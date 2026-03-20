namespace WebApplication.Models.ViewModels
{
    public class PaymentDetailDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal GrandTotal { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public bool HasExistingPayment { get; set; }
        public string? ProofImageUrl { get; set; }
        public string? RejectionReason { get; set; }
    }
}
