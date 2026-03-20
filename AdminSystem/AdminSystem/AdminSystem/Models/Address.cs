namespace AdminSystem.Models
{
    public class Address
    {
        public int    AddressId  { get; set; }
        public int    UserId     { get; set; }
        public string Label      { get; set; }
        public string Street     { get; set; }
        public string City       { get; set; }
        public string Province   { get; set; }
        public string PostalCode { get; set; }
        public string Country    { get; set; }
        public bool   IsDefault  { get; set; }
        public bool   IsSnapshot { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
