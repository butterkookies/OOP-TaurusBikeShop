namespace AdminSystem.Models
{
    public class BankTransferPayment
    {
        public int    PaymentId            { get; set; }
        public string DepositorName        { get; set; }
        public string BpiReferenceNumber   { get; set; }
        public string DepositSlipUrl       { get; set; }
        public string StorageBucket        { get; set; }
        public string StoragePath          { get; set; }
        public int?   VerifiedByUserId     { get; set; }
        public string VerificationNotes    { get; set; }
        public System.DateTime? VerifiedAt            { get; set; }
        public System.DateTime? VerificationDeadline  { get; set; }
        public System.DateTime SubmittedAt            { get; set; }
    }
}
