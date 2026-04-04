namespace AdminSystem_v2.Models
{
    public class Brand
    {
        public int    BrandId   { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public bool   IsActive  { get; set; }
    }
}
