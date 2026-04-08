namespace AdminSystem_v2.Models
{
    /// <summary>Returned by POSRepository.CreatePOSSaleAsync — holds everything needed for the receipt view.</summary>
    public class POSOrderResult
    {
        public int               OrderId        { get; set; }
        public string            OrderNumber    { get; set; } = string.Empty;
        public decimal           Subtotal       { get; set; }
        public decimal           DiscountAmount { get; set; }
        public decimal           GrandTotal     { get; set; }
        public string            PaymentMethod  { get; set; } = string.Empty;
        public decimal           CashReceived   { get; set; }
        public decimal           Change         { get; set; }
        public string            VoucherCode    { get; set; } = string.Empty;
        public string            CustomerName   { get; set; } = string.Empty;
        public string            CashierName    { get; set; } = string.Empty;
        public DateTime          CompletedAt    { get; set; }
        public List<POSCartItem> Items          { get; set; } = new();

        // ── Computed helpers for XAML visibility bindings ─────────────────
        public bool HasDiscount   => DiscountAmount > 0;
        public bool HasVoucher    => !string.IsNullOrEmpty(VoucherCode);
        public bool IsCashMethod  => PaymentMethod  == POSPaymentMethods.Cash;
    }
}
