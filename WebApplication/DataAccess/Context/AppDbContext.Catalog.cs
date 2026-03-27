// WebApplication/DataAccess/Context/AppDbContext.Catalog.cs
// Entity configurations: Category, Brand, Product, ProductVariant, ProductImage, PriceHistory

using Microsoft.EntityFrameworkCore;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Context;

public sealed partial class AppDbContext
{
    private static void ConfigureCategory(ModelBuilder mb)
    {
        mb.Entity<Category>(e =>
        {
            e.ToTable("Category");
            e.HasKey(c => c.CategoryId);

            e.HasIndex(c => c.CategoryCode)
                .IsUnique()
                .HasDatabaseName("UQ_Category_Code");

            e.HasIndex(c => c.IsActive)
                .HasDatabaseName("IX_Category_IsActive");

            e.HasIndex(c => c.DisplayOrder)
                .HasDatabaseName("IX_Category_DisplayOrder");

            e.HasIndex(c => c.ParentCategoryId)
                .HasDatabaseName("IX_Category_ParentCategoryId");

            // Self-referencing hierarchy — no cascade delete to prevent
            // accidental tree destruction
            e.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureBrand(ModelBuilder mb)
    {
        mb.Entity<Brand>(e =>
        {
            e.ToTable("Brand");
            e.HasKey(b => b.BrandId);

            e.HasIndex(b => b.BrandName)
                .IsUnique()
                .HasDatabaseName("UQ_Brand_Name");

            e.HasIndex(b => b.IsActive)
                .HasDatabaseName("IX_Brand_IsActive");
        });
    }

    private static void ConfigureProduct(ModelBuilder mb)
    {
        mb.Entity<Product>(e =>
        {
            e.ToTable("Product");
            e.HasKey(p => p.ProductId);

            e.Property(p => p.Price).HasPrecision(18, 2);

            // Filtered unique SKU — unique when non-NULL
            e.HasIndex(p => p.SKU)
                .IsUnique()
                .HasFilter("[SKU] IS NOT NULL")
                .HasDatabaseName("UX_Product_SKU");

            e.HasIndex(p => p.CategoryId)
                .HasDatabaseName("IX_Product_CategoryId");

            e.HasIndex(p => p.BrandId)
                .HasDatabaseName("IX_Product_BrandId");

            e.HasIndex(p => p.IsActive)
                .HasDatabaseName("IX_Product_IsActive");

            e.HasIndex(p => p.IsFeatured)
                .HasDatabaseName("IX_Product_IsFeatured");

            e.HasIndex(p => p.Name)
                .HasDatabaseName("IX_Product_Name");

            e.HasIndex(p => new { p.Price, p.IsActive })
                .HasDatabaseName("IX_Product_Price");

            e.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureProductVariant(ModelBuilder mb)
    {
        mb.Entity<ProductVariant>(e =>
        {
            e.ToTable("ProductVariant");
            e.HasKey(pv => pv.ProductVariantId);

            e.Property(pv => pv.AdditionalPrice).HasPrecision(18, 2);

            e.HasIndex(pv => pv.ProductId)
                .HasDatabaseName("IX_ProductVariant_ProductId");

            e.HasIndex(pv => pv.IsActive)
                .HasDatabaseName("IX_ProductVariant_IsActive");

            e.HasIndex(pv => pv.ReorderThreshold)
                .HasDatabaseName("IX_ProductVariant_ReorderThreshold");

            e.HasIndex(pv => pv.SKU)
                .HasDatabaseName("IX_ProductVariant_SKU");

            e.HasOne(pv => pv.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureProductImage(ModelBuilder mb)
    {
        mb.Entity<ProductImage>(e =>
        {
            e.ToTable("ProductImage");
            e.HasKey(pi => pi.ProductImageId);

            // Filtered unique index — only one primary image per product
            e.HasIndex(pi => pi.ProductId)
                .IsUnique()
                .HasFilter("[IsPrimary] = 1")
                .HasDatabaseName("UX_ProductImage_Primary");

            e.HasIndex(pi => pi.ProductId)
                .HasDatabaseName("IX_ProductImage_ProductId");

            e.HasIndex(pi => pi.IsPrimary)
                .HasDatabaseName("IX_ProductImage_IsPrimary");

            e.HasIndex(pi => pi.ImageType)
                .HasDatabaseName("IX_ProductImage_ImageType");

            e.HasIndex(pi => pi.DisplayOrder)
                .HasDatabaseName("IX_ProductImage_DisplayOrder");

            e.HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(pi => pi.UploadedBy)
                .WithMany()
                .HasForeignKey(pi => pi.UploadedByUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigurePriceHistory(ModelBuilder mb)
    {
        mb.Entity<PriceHistory>(e =>
        {
            e.ToTable("PriceHistory");
            e.HasKey(ph => ph.PriceHistoryId);

            e.Property(ph => ph.OldPrice).HasPrecision(18, 2);
            e.Property(ph => ph.NewPrice).HasPrecision(18, 2);

            e.HasIndex(ph => new { ph.ProductId, ph.ChangedAt })
                .HasDatabaseName("IX_PriceHistory_ProductId");

            e.HasIndex(ph => ph.ChangedAt)
                .HasDatabaseName("IX_PriceHistory_ChangedAt");

            e.HasOne(ph => ph.Product)
                .WithMany(p => p.PriceHistory)
                .HasForeignKey(ph => ph.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(ph => ph.ChangedBy)
                .WithMany()
                .HasForeignKey(ph => ph.ChangedByUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
