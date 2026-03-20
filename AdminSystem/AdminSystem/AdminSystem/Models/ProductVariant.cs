namespace AdminSystem.Models
{
    public class ProductVariant
    {
        public int     ProductVariantId { get; set; }
        public int     ProductId        { get; set; }
        public string  VariantName      { get; set; }
        public decimal AdditionalPrice  { get; set; }
        public int     StockQuantity    { get; set; }
        public int     ReorderThreshold { get; set; }
        public string  SKU              { get; set; }
        public bool    IsActive         { get; set; }
        public System.DateTime? UpdatedAt { get; set; }
    }
}
