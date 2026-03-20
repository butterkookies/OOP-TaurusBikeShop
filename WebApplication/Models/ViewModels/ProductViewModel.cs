namespace WebApplication.Models.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = "PHP";
        public int StockQuantity { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryCode { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; }

        public bool InStock => StockQuantity > 0;
    }
}
