namespace AdminSystem_v2.Models
{
    /// <summary>Flat product+variant row returned by POS product search.</summary>
    public class POSProductItem
    {
        public int     ProductId        { get; set; }
        public int     ProductVariantId { get; set; }
        public string  ProductName      { get; set; } = string.Empty;
        public string  VariantName      { get; set; } = string.Empty;
        public decimal UnitPrice        { get; set; }
        public int     StockQuantity    { get; set; }
        public string  SKU              { get; set; } = string.Empty;

        public string DisplayName =>
            string.IsNullOrEmpty(VariantName) || VariantName == "Default"
                ? ProductName
                : $"{ProductName} — {VariantName}";

        public bool   InStock    => StockQuantity > 0;
        public string StockLabel => $"Stock: {StockQuantity}";
    }
}
