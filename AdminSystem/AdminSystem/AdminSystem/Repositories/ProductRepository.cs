using System.Collections.Generic;
using System.Linq;
using AdminSystem.Models;
using Dapper;
using System.Data.SqlClient;

namespace AdminSystem.Repositories
{
    public class ProductRepository : Repository<Product>, IRepository<Product>
    {
        public Product GetById(int id)
        {
            using (SqlConnection conn = GetConnection())
            {
                Product p = conn.QueryFirstOrDefault<Product>(
                    @"SELECT p.*, c.Name AS CategoryName, b.BrandName
                      FROM Product p
                      LEFT JOIN Category c ON p.CategoryId=c.CategoryId
                      LEFT JOIN Brand b ON p.BrandId=b.BrandId
                      WHERE p.ProductId=@Id", new { Id = id });
                if (p == null) return null;
                p.Variants = conn.Query<ProductVariant>(
                    "SELECT * FROM ProductVariant WHERE ProductId=@Id AND IsActive=1 ORDER BY VariantName",
                    new { Id = id }).ToList();
                p.Images = conn.Query<ProductImage>(
                    "SELECT * FROM ProductImage WHERE ProductId=@Id ORDER BY IsPrimary DESC, SortOrder",
                    new { Id = id }).ToList();
                return p;
            }
        }

        public IEnumerable<Product> GetAll()
        {
            using (SqlConnection conn = GetConnection())
                return conn.Query<Product>(
                    @"SELECT p.*, c.Name AS CategoryName, b.BrandName
                      FROM Product p
                      LEFT JOIN Category c ON p.CategoryId=c.CategoryId
                      LEFT JOIN Brand b ON p.BrandId=b.BrandId
                      ORDER BY p.Name");
        }

        public IEnumerable<Product> Search(string searchText)
        {
            using (SqlConnection conn = GetConnection())
                return conn.Query<Product>(
                    @"SELECT p.*, c.Name AS CategoryName, b.BrandName
                      FROM Product p
                      LEFT JOIN Category c ON p.CategoryId=c.CategoryId
                      LEFT JOIN Brand b ON p.BrandId=b.BrandId
                      WHERE p.Name LIKE @Search OR p.Description LIKE @Search
                      ORDER BY p.Name",
                    new { Search = "%" + searchText + "%" });
        }

        public int Insert(Product entity)
            => ExecuteScalar(
                @"INSERT INTO Product (CategoryId,BrandId,Name,Description,Price,
                    IsActive,IsFeatured,CreatedAt,UpdatedAt)
                  VALUES (@CategoryId,@BrandId,@Name,@Description,@Price,
                    @IsActive,@IsFeatured,GETUTCDATE(),GETUTCDATE());
                  SELECT CAST(SCOPE_IDENTITY() AS INT);", entity);

        public void Update(Product entity)
            => Execute(
                @"UPDATE Product SET CategoryId=@CategoryId,BrandId=@BrandId,
                    Name=@Name,Description=@Description,Price=@Price,
                    IsActive=@IsActive,IsFeatured=@IsFeatured,UpdatedAt=GETUTCDATE()
                  WHERE ProductId=@ProductId", entity);

        public void Delete(int id)
            => Execute(
                "UPDATE Product SET IsActive=0,UpdatedAt=GETUTCDATE() WHERE ProductId=@Id",
                new { Id = id });

        public void AdjustStock(int variantId, int qty, string changeType,
            int changedByUserId, string notes = null)
        {
            ExecuteTransaction((conn, tx) =>
            {
                conn.Execute(
                    @"UPDATE ProductVariant SET StockQuantity=StockQuantity+@Qty,
                        UpdatedAt=GETUTCDATE() WHERE ProductVariantId=@VariantId",
                    new { Qty = qty, VariantId = variantId }, tx);
                int productId = conn.ExecuteScalar<int>(
                    "SELECT ProductId FROM ProductVariant WHERE ProductVariantId=@Id",
                    new { Id = variantId }, tx);
                conn.Execute(
                    @"INSERT INTO InventoryLog (ProductId,ProductVariantId,ChangeQuantity,
                        ChangeType,ChangedByUserId,Notes,CreatedAt)
                      VALUES (@ProductId,@VariantId,@Qty,@ChangeType,@UserId,@Notes,GETUTCDATE())",
                    new { ProductId = productId, VariantId = variantId,
                          Qty = qty, ChangeType = changeType,
                          UserId = changedByUserId, Notes = notes }, tx);
            });
        }

        public IEnumerable<Category> GetAllCategories()
        {
            using (SqlConnection conn = GetConnection())
                return conn.Query<Category>("SELECT * FROM Category ORDER BY Name");
        }

        public IEnumerable<Brand> GetAllBrands()
        {
            using (SqlConnection conn = GetConnection())
                return conn.Query<Brand>(
                    "SELECT * FROM Brand WHERE IsActive=1 ORDER BY BrandName");
        }
    }
}
