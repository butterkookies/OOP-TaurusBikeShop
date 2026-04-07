namespace AdminSystem_v2.Models
{
    /// <summary>Customer result row used in the voucher assignment search.</summary>
    public class VoucherUserRow
    {
        public int    UserId   { get; set; }
        public string Name     { get; set; } = string.Empty;
        public string Email    { get; set; } = string.Empty;
        public bool   IsWalkIn { get; set; }

        public string TypeLabel => IsWalkIn ? "Walk-in" : "Online";

        public override string ToString() => $"{Name} ({Email})";
    }
}
