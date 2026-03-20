namespace AdminSystem.Models
{
    public class PurchaseOrderItem
    {
        public int     PurchaseOrderItemId { get; set; }
        public int     PurchaseOrderId     { get; set; }
        public int     ProductId           { get; set; }
        public int?    ProductVariantId    { get; set; }
        public int     Quantity            { get; set; }
        public decimal UnitPrice           { get; set; }
        public string  ProductName         { get; set; }
        public string  VariantName         { get; set; }
        public decimal LineTotal           => Quantity * UnitPrice;
    }
}
