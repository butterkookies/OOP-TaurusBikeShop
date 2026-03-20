// AdminSystem/Models/Payment.cs
using System;

namespace AdminSystem.Models
{
    public class Payment
    {
        public int     PaymentId      { get; set; }
        public int     OrderId        { get; set; }
        public string  PaymentMethod  { get; set; } = string.Empty;
        public string  PaymentStage   { get; set; } = string.Empty;
        public string  PaymentStatus  { get; set; } = string.Empty;
        public decimal Amount         { get; set; }
        public DateTime CreatedAt     { get; set; }

        public GCashPayment       GCash       { get; set; }
        public BankTransferPayment BankTransfer { get; set; }
    }

    public static class PaymentMethods
    {
        public const string GCash        = "GCash";
        public const string BankTransfer = "BankTransfer";
        public const string Cash         = "Cash";
    }

    public static class PaymentStatuses
    {
        public const string Pending                = "Pending";
        public const string VerificationPending    = "VerificationPending";
        public const string VerificationRejected   = "VerificationRejected";
        public const string Completed              = "Completed";
        public const string Failed                 = "Failed";
    }

    public static class PaymentStages
    {
        public const string Upfront      = "Upfront";
        public const string Confirmation = "Confirmation";
    }
}
