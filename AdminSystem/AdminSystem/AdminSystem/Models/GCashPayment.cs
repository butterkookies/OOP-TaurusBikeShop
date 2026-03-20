namespace AdminSystem.Models
{
    public class GCashPayment
    {
        public int    PaymentId          { get; set; }
        public string GcashNumber        { get; set; }
        public string GcashTransactionId { get; set; }
        public string ScreenshotUrl      { get; set; }
        public string StorageBucket      { get; set; }
        public string StoragePath        { get; set; }
        public System.DateTime SubmittedAt { get; set; }
    }
}
