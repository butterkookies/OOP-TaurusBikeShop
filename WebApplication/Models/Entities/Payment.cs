namespace WebApplication.Models.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; } = "Cash";
        public string PaymentStatus { get; set; } = "Pending";
        public decimal Amount { get; set; }
        public string? TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ProofImageUrl { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime? VerifiedAt { get; set; }

        public Order? Order { get; set; }
    }
}
