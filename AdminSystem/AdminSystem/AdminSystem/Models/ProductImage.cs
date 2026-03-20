namespace AdminSystem.Models
{
    public class ProductImage
    {
        public int    ProductImageId { get; set; }
        public int    ProductId      { get; set; }
        public string ImageUrl       { get; set; }
        public string StorageBucket  { get; set; }
        public string StoragePath    { get; set; }
        public bool   IsPrimary      { get; set; }
        public int    SortOrder      { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
