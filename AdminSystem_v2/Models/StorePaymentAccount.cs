namespace AdminSystem_v2.Models
{
    /// <summary>
    /// A single row of the <c>StorePaymentAccount</c> table — the store-owned
    /// GCash / BPI account the customer website displays on the payment page.
    /// </summary>
    public class StorePaymentAccount
    {
        public int     StorePaymentAccountId { get; set; }
        public string  PaymentMethod         { get; set; } = string.Empty;
        public string  AccountName           { get; set; } = string.Empty;
        public string  AccountNumber         { get; set; } = string.Empty;
        public string? BankName              { get; set; }
        public string? QrImageUrl            { get; set; }
        public string? Instructions          { get; set; }
        public bool    IsActive              { get; set; }
        public int     DisplayOrder          { get; set; }
        public DateTime CreatedAt            { get; set; }
        public DateTime UpdatedAt            { get; set; }

        public string MethodDisplay => PaymentMethod switch
        {
            "GCash"        => "GCash",
            "BankTransfer" => "Bank Transfer",
            _              => PaymentMethod
        };

        public string StatusDisplay => IsActive ? "Active" : "Inactive";
    }
}
