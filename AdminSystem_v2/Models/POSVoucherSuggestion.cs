namespace AdminSystem_v2.Models
{
    /// <summary>Lightweight voucher entry shown in the POS autocomplete dropdown.</summary>
    public class POSVoucherSuggestion
    {
        public string Code            { get; set; } = string.Empty;
        public string Description     { get; set; } = string.Empty;
        /// <summary>Human-readable discount, e.g. "10% off" or "₱50.00 off".</summary>
        public string DiscountDisplay { get; set; } = string.Empty;
    }
}
