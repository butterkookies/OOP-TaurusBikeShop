namespace AdminSystem_v2.Models
{
    public class ProductImage
    {
        public int      ProductImageId { get; set; }
        public int      ProductId      { get; set; }
        public string   ImageUrl       { get; set; } = string.Empty;
        public string   ImageType      { get; set; } = string.Empty;
        public string   StorageBucket  { get; set; } = string.Empty;
        public string   StoragePath    { get; set; } = string.Empty;
        public bool     IsPrimary      { get; set; }
        public int      DisplayOrder   { get; set; }
        public DateTime CreatedAt      { get; set; }
    }
}
