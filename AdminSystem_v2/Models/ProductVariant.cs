namespace AdminSystem_v2.Models
{
    public class ProductVariant
    {
        public int       ProductVariantId  { get; set; }
        public int       ProductId         { get; set; }
        public string    VariantName       { get; set; } = string.Empty;
        public decimal   AdditionalPrice   { get; set; }
        public int       StockQuantity     { get; set; }
        public int       ReorderThreshold  { get; set; }
        public string    SKU               { get; set; } = string.Empty;
        public bool      IsActive          { get; set; }
        public DateTime? UpdatedAt         { get; set; }
    }
}
