namespace WebApplication.Models.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;

        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
