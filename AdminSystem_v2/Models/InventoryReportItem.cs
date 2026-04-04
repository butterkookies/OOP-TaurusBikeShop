namespace AdminSystem_v2.Models
{
    /// <summary>
    /// One row per active product variant — used by the Inventory Report tab.
    /// StockStatus is a computed string: "InStock", "LowStock", or "OutOfStock".
    /// </summary>
    public class InventoryReportItem
    {
        public int     ProductId        { get; set; }
        public string  ProductName      { get; set; } = string.Empty;
        public string  CategoryName     { get; set; } = string.Empty;
        public string? BrandName        { get; set; }
        public string  VariantName      { get; set; } = string.Empty;
        public string? SKU              { get; set; }
        public int     StockQuantity    { get; set; }
        public int     ReorderThreshold { get; set; }
        public decimal Price            { get; set; }
        public decimal StockValue       { get; set; }
        public string  StockStatus      { get; set; } = string.Empty;
    }

    public static class StockStatuses
    {
        public const string InStock    = "InStock";
        public const string LowStock   = "LowStock";
        public const string OutOfStock = "OutOfStock";
    }
}
