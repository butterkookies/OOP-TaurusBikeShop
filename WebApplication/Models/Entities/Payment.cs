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

        public Order? Order { get; set; }
    }
}
