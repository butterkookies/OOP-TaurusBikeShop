namespace AdminSystem_v2.Models
{
    /// <summary>Result of validating a voucher code against the current POS cart.</summary>
    public class POSVoucherResult
    {
        public bool    IsValid           { get; set; }
        public string  Error             { get; set; } = string.Empty;
        public int     VoucherId         { get; set; }
        public string  VoucherCode       { get; set; } = string.Empty;
        public string  DiscountType      { get; set; } = string.Empty;
        public decimal DiscountValue     { get; set; }
        public decimal DiscountAmount    { get; set; }
        public string  FormattedDiscount { get; set; } = string.Empty;
    }
}
