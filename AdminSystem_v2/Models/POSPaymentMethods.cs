namespace AdminSystem_v2.Models
{
    /// <summary>
    /// POS-side payment method constants.
    /// References the centralised <see cref="PaymentMethods"/> class for consistency.
    /// Cash, GCash, and BankTransfer are fully active.
    /// Card is a placeholder — stored in the enum/UI but not yet processed.
    /// </summary>
    public static class POSPaymentMethods
    {
        public const string Cash         = PaymentMethods.Cash;
        public const string GCash        = PaymentMethods.GCash;
        public const string BankTransfer = PaymentMethods.BankTransfer;

        /// <summary>Placeholder — UI visible, checkout disabled until gateway is integrated.</summary>
        public const string Card = PaymentMethods.Card;
    }
}
