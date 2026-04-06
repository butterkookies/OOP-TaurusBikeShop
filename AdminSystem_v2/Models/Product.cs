namespace AdminSystem_v2.Models
{
    public class Product
    {
        public int       ProductId        { get; set; }
        public int       CategoryId       { get; set; }   // NOT NULL in DB
        public int?      BrandId          { get; set; }
        public string    Name             { get; set; } = string.Empty;
        public string    ShortDescription { get; set; } = string.Empty;
        public string    Description      { get; set; } = string.Empty;
        public decimal   Price            { get; set; }
        public string    Currency         { get; set; } = "PHP";
        public bool      IsActive         { get; set; }
        public bool      IsFeatured       { get; set; }
        public DateTime  CreatedAt        { get; set; }
        public DateTime? UpdatedAt        { get; set; }

        // Joined from Category / Brand tables
        public string CategoryName { get; set; } = string.Empty;
        public string BrandName    { get; set; } = string.Empty;

        public List<ProductVariant>  Variants { get; set; } = new();
        public List<ProductImage>   Images   { get; set; } = new();

        public int TotalStock => Variants
            .Where(v => v.IsActive)
            .Sum(v => v.StockQuantity);
    }
}
