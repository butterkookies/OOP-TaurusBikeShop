namespace AdminSystem.Models
{
    public class Review
    {
        public int    ReviewId           { get; set; }
        public int    UserId             { get; set; }
        public int    ProductId          { get; set; }
        public int    OrderId            { get; set; }
        public int    Rating             { get; set; }
        public string Comment            { get; set; }
        public bool   IsVerifiedPurchase { get; set; }
        public bool   IsVisible          { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string CustomerName       { get; set; }
        public string ProductName        { get; set; }
    }
}
