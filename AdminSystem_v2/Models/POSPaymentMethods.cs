namespace AdminSystem_v2.Models
{
    /// <summary>
    /// POS-side payment method constants.
    /// Cash, GCash, and BankTransfer are fully active.
    /// Card is a placeholder — stored in the enum/UI but not yet processed.
    /// </summary>
    public static class POSPaymentMethods
    {
        public const string Cash         = "Cash";
        public const string GCash        = "GCash";
        public const string BankTransfer = "BankTransfer";

        /// <summary>Placeholder — UI visible, checkout disabled until gateway is integrated.</summary>
        public const string Card = "Card";
    }
}
