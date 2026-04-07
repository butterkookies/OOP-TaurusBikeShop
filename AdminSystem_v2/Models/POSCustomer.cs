namespace AdminSystem_v2.Models
{
    /// <summary>Customer lookup result for POS customer selector.</summary>
    public class POSCustomer
    {
        public int    UserId { get; set; }
        public string Name   { get; set; } = string.Empty;
        public string Email  { get; set; } = string.Empty;
        public string Phone  { get; set; } = string.Empty;

        public override string ToString() => Name;
    }
}
