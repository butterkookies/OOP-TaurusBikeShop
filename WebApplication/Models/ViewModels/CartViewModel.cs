namespace WebApplication.Models.ViewModels
{
    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
    }

    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();
        public decimal Subtotal => Items.Sum(i => i.LineTotal);
        public decimal ShippingFee => Items.Count > 0 ? 150m : 0m;
        public decimal Total => Subtotal + ShippingFee;
        public int ItemCount => Items.Sum(i => i.Quantity);
    }
}
