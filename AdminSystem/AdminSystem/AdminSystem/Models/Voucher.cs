namespace AdminSystem.Models
{
    public class Voucher
    {
        public int      VoucherId            { get; set; }
        public string   Code                 { get; set; }
        public string   DiscountType         { get; set; }
        public decimal  DiscountValue        { get; set; }
        public decimal? MinimumOrderAmount   { get; set; }
        public int?     MaxUsageCount        { get; set; }
        public int?     MaxUsagePerUser      { get; set; }
        public System.DateTime? ExpiresAt    { get; set; }
        public bool     IsActive             { get; set; }
        public System.DateTime CreatedAt     { get; set; }
    }

    public static class DiscountTypes
    {
        public const string Percentage = "Percentage";
        public const string Fixed      = "Fixed";
    }
}
