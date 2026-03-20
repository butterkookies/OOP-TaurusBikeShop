namespace WebApplication.Models.ViewModels
{
    public class OrderItemViewModel
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal => Quantity * UnitPrice;
    }

    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new();
    }
}
