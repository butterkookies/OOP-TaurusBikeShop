namespace WebApplication.Models.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string? SKU { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = "PHP";
        public int StockQuantity { get; set; } = 0;
        public string? Material { get; set; }
        public string? Color { get; set; }
        public string? WheelSize { get; set; }
        public string? SpeedCompatibility { get; set; }
        public bool? BoostCompatible { get; set; }
        public bool? TubelessReady { get; set; }
        public string? AxleStandard { get; set; }
        public string? SuspensionTravel { get; set; }
        public string? BrakeType { get; set; }
        public string? AdditionalSpecs { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Category? Category { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
