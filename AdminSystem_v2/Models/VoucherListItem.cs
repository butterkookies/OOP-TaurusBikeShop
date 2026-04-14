namespace AdminSystem_v2.Models
{
    /// <summary>Represents one row in the voucher management grid.</summary>
    public class VoucherListItem
    {
        public int      VoucherId          { get; set; }
        public string   Code               { get; set; } = string.Empty;
        public string   Description        { get; set; } = string.Empty;
        public string   DiscountType       { get; set; } = string.Empty;
        public decimal  DiscountValue      { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
        public int?     MaxUses            { get; set; }
        public int?     MaxUsesPerUser     { get; set; }
        public DateTime StartDate          { get; set; }
        public DateTime? EndDate           { get; set; }
        public bool     IsActive           { get; set; }
        public DateTime CreatedAt          { get; set; }
        public int      TimesUsed          { get; set; }
        public int      AssignedCount      { get; set; }

        // ── Computed display helpers ──────────────────────────────────────

        public string DiscountDisplay => DiscountType == "Percentage"
            ? $"{DiscountValue:N0}% off"
            : $"₱{DiscountValue:N2} off";

        public string ValidPeriod
        {
            get
            {
                string start = StartDate.ToString("MMM d, yyyy");
                string end   = EndDate.HasValue ? EndDate.Value.ToString("MMM d, yyyy") : "No expiry";
                return $"{start} – {end}";
            }
        }

        public string MaxUsesDisplay    => MaxUses.HasValue    ? MaxUses.ToString()!           : "Unlimited";
        public string PerUserDisplay    => MaxUsesPerUser.HasValue ? MaxUsesPerUser.ToString()! : "Unlimited";
        public string MinOrderDisplay   => MinimumOrderAmount.HasValue ? $"₱{MinimumOrderAmount:N2}" : "None";

        public string StatusDisplay
        {
            get
            {
                if (!IsActive) return "Inactive";
                if (EndDate.HasValue && EndDate.Value < DateTime.Now) return "Expired";
                if (StartDate > DateTime.Now) return "Scheduled";
                return "Active";
            }
        }

        /// <summary>True when end date is in the past regardless of IsActive.</summary>
        public bool IsExpired => EndDate.HasValue && EndDate.Value < DateTime.Now;

        public override string ToString()
            => $"{Code}  —  {DiscountDisplay}  |  Used: {TimesUsed}/{MaxUsesDisplay}  |  {StatusDisplay}";
    }
}
