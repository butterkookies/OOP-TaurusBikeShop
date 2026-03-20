// AdminSystem/Models/InventoryLog.cs
using System;

namespace AdminSystem.Models
{
    public class InventoryLog
    {
        public int LogId { get; set; }
        public int ProductId { get; set; }
        public int? ProductVariantId { get; set; }
        public int? OrderId { get; set; }
        public int? PurchaseOrderId { get; set; }
        public int? ChangedByUserId { get; set; }
        public int ChangeQuantity { get; set; }
        public string ChangeType { get; set; } = string.Empty;
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        public string ProductName { get; set; }
        public string VariantName { get; set; }
    }

    public static class InventoryChangeTypes
    {
        public const string Purchase = "Purchase";
        public const string Sale = "Sale";
        public const string Return = "Return";
        public const string Adjustment = "Adjustment";
        public const string Damage = "Damage";
        public const string Loss = "Loss";
        public const string Lock = "Lock";
        public const string Unlock = "Unlock";
    }
}