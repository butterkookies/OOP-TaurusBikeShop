namespace AdminSystem_v2.Models
{
    /// <summary>
    /// Append-only record of every stock movement.
    /// Never updated or deleted — each change adds a new row.
    /// </summary>
    public class InventoryLog
    {
        public int      InventoryLogId    { get; set; }
        public int      ProductId         { get; set; }
        public int?     ProductVariantId  { get; set; }
        public int?     OrderId           { get; set; }
        public int?     PurchaseOrderId   { get; set; }
        public int?     ChangedByUserId   { get; set; }
        public int      ChangeQuantity    { get; set; }
        public string   ChangeType        { get; set; } = string.Empty;
        public string   Notes             { get; set; } = string.Empty;
        public DateTime CreatedAt         { get; set; }

        // Joined display fields
        public string ProductName { get; set; } = string.Empty;
        public string VariantName { get; set; } = string.Empty;
    }

    public static class InventoryChangeTypes
    {
        public const string Purchase   = "Purchase";
        public const string Sale       = "Sale";
        public const string Return     = "Return";
        public const string Adjustment = "Adjustment";
        public const string Damage     = "Damage";
        public const string Loss       = "Loss";
        public const string Lock       = "Lock";
        public const string Unlock     = "Unlock";
    }
}
