namespace WebApplication.Models.Entities
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }
        public string? LalamoveReference { get; set; }
        public string DeliveryStatus { get; set; } = "Pending";
        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Order? Order { get; set; }
    }
}
