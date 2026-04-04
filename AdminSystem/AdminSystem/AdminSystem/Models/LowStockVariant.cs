namespace AdminSystem.Models
{
    /// <summary>
    /// Represents a product variant whose current stock is at or below its reorder threshold.
    /// Distinct from InventoryLog, which records change events — this represents current state.
    /// </summary>
    public class LowStockVariant
    {
        public int    ProductVariantId { get; set; }
        public int    ProductId        { get; set; }
        public string VariantName      { get; set; }
        public string ProductName      { get; set; }
        public int    StockQuantity    { get; set; }
        public int    ReorderThreshold { get; set; }
    }
}
