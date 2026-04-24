namespace AdminSystem_v2.Models
{
    public class POSSession
    {
        public int      POSSessionId { get; set; }
        public int      UserId       { get; set; }
        public string   TerminalName { get; set; } = string.Empty;
        public DateTime ShiftStart   { get; set; }
        public DateTime? ShiftEnd    { get; set; }
        public decimal  TotalSales   { get; set; }
    }
}
