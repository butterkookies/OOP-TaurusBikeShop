using System.Collections.Generic;

namespace AdminSystem.Models
{
    public class PurchaseOrder
    {
        public int    PurchaseOrderId  { get; set; }
        public int    SupplierId       { get; set; }
        public int?   CreatedByUserId  { get; set; }
        public string Status           { get; set; }
        public System.DateTime  OrderDate    { get; set; }
        public System.DateTime? ExpectedDate { get; set; }
        public System.DateTime? ReceivedDate { get; set; }
        public string Notes            { get; set; }
        public string SupplierName     { get; set; }
        public List<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    }

    public static class PurchaseOrderStatuses
    {
        public const string Pending   = "Pending";
        public const string Received  = "Received";
        public const string Cancelled = "Cancelled";
    }
}
