namespace WebApplication.Models.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string OrderStatus { get; set; } = "Pending";
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; } = 0;
        public decimal ShippingFee { get; set; } = 0;
        public decimal TotalAmount { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingCity { get; set; }
        public string? ShippingPostalCode { get; set; }
        public string? ContactPhone { get; set; }
        public string? DeliveryInstructions { get; set; }
        public bool IsWalkIn { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
