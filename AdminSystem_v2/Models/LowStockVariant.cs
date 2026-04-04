namespace AdminSystem_v2.Models
{
    /// <summary>
    /// A product variant whose current stock is at or below its reorder threshold.
    /// Read-only — used only for display, never written back to the database.
    /// </summary>
    public class LowStockVariant
    {
        public int    ProductVariantId  { get; set; }
        public int    ProductId         { get; set; }
        public string ProductName       { get; set; } = string.Empty;
        public string VariantName       { get; set; } = string.Empty;
        public int    StockQuantity     { get; set; }
        public int    ReorderThreshold  { get; set; }

        public string StockDisplay => $"{StockQuantity} / {ReorderThreshold}";
    }
}
