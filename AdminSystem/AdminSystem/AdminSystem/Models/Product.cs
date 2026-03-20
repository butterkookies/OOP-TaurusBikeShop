// AdminSystem/Models/Product.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdminSystem.Models
{
    public class Product
    {
        public int     ProductId   { get; set; }
        public int?    CategoryId  { get; set; }
        public int?    BrandId     { get; set; }
        public string  Name        { get; set; } = string.Empty;
        public string  Description { get; set; }
        public decimal Price       { get; set; }
        public bool    IsActive    { get; set; }
        public bool    IsFeatured  { get; set; }
        public DateTime  CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string CategoryName { get; set; }
        public string BrandName    { get; set; }
        public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public List<ProductImage>   Images   { get; set; } = new List<ProductImage>();

        public int TotalStock => Variants?.Where(v => v.IsActive).Sum(v => v.StockQuantity) ?? 0;
    }
}
