namespace AdminSystem.Models
{
    public class POS_Session
    {
        public int     POSSessionId  { get; set; }
        public int     CashierId     { get; set; }
        public decimal TotalSales    { get; set; }
        public string  PaymentMethod { get; set; }
        public System.DateTime StartTime  { get; set; }
        public System.DateTime? EndTime   { get; set; }
        public string  CashierName   { get; set; }
    }
}
