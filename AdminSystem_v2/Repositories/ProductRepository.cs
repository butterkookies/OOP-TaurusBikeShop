using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using Dapper;

namespace AdminSystem_v2.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public async Task<Product?> GetByIdAsync(int id)
        {
            await using var conn = GetConnection();

            var product = await conn.QueryFirstOrDefaultAsync<Product>(
                @"SELECT p.*, c.Name AS CategoryName, b.BrandName
                  FROM Product p
                  LEFT JOIN Category c ON p.CategoryId = c.CategoryId
                  LEFT JOIN Brand    b ON p.BrandId    = b.BrandId
                  WHERE p.ProductId = @Id", new { Id = id });

            if (product == null) return null;

            product.Variants = (await conn.QueryAsync<ProductVariant>(
                "SELECT * FROM ProductVariant WHERE ProductId = @Id AND IsActive = 1 ORDER BY VariantName",
                new { Id = id })).ToList();

            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            await using var conn = GetConnection();

            var products = (await conn.QueryAsync<Product>(
                @"SELECT p.*, c.Name AS CategoryName, b.BrandName
                  FROM Product p
                  LEFT JOIN Category c ON p.CategoryId = c.CategoryId
                  LEFT JOIN Brand    b ON p.BrandId    = b.BrandId
                  WHERE p.IsActive = 1
                  ORDER BY p.Name")).ToList();

            await PopulateVariantsAsync(conn, products);
            return products;
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchText)
        {
            await using var conn = GetConnection();

            var products = (await conn.QueryAsync<Product>(
                @"SELECT p.*, c.Name AS CategoryName, b.BrandName
                  FROM Product p
                  LEFT JOIN Category c ON p.CategoryId = c.CategoryId
                  LEFT JOIN Brand    b ON p.BrandId    = b.BrandId
                  WHERE p.IsActive = 1
                    AND (p.Name LIKE @Search OR p.Description LIKE @Search)
                  ORDER BY p.Name",
                new { Search = $"%{searchText}%" })).ToList();

            await PopulateVariantsAsync(conn, products);
            return products;
        }

        public async Task<int> GetTotalCountAsync()
            => await ExecuteScalarAsync(
                "SELECT COUNT(*) FROM Product WHERE IsActive = 1");

        public async Task<int> InsertAsync(Product entity)
            => await ExecuteScalarAsync(
                @"INSERT INTO Product
                    (CategoryId, BrandId, Name, Description, Price,
                     IsActive, IsFeatured, CreatedAt, UpdatedAt)
                  VALUES
                    (@CategoryId, @BrandId, @Name, @Description, @Price,
                     @IsActive, @IsFeatured, GETUTCDATE(), GETUTCDATE());
                  SELECT CAST(SCOPE_IDENTITY() AS INT);", entity);

        public async Task UpdateAsync(Product entity)
            => await ExecuteAsync(
                @"UPDATE Product
                  SET CategoryId  = @CategoryId,
                      BrandId     = @BrandId,
                      Name        = @Name,
                      Description = @Description,
                      Price       = @Price,
                      IsActive    = @IsActive,
                      IsFeatured  = @IsFeatured,
                      UpdatedAt   = GETUTCDATE()
                  WHERE ProductId = @ProductId", entity);

        public async Task DeleteAsync(int id)
            => await ExecuteAsync(
                "UPDATE Product SET IsActive = 0, UpdatedAt = GETUTCDATE() WHERE ProductId = @Id",
                new { Id = id });

        public async Task AdjustStockAsync(int variantId, int qty,
            string changeType, int changedByUserId, string? notes = null)
        {
            await using var conn = GetConnection();
            await using var tx   = await conn.BeginTransactionAsync();
            try
            {
                await conn.ExecuteAsync(
                    @"UPDATE ProductVariant
                      SET StockQuantity = StockQuantity + @Qty,
                          UpdatedAt     = GETUTCDATE()
                      WHERE ProductVariantId = @VariantId",
                    new { Qty = qty, VariantId = variantId }, tx);

                int productId = await conn.ExecuteScalarAsync<int>(
                    "SELECT ProductId FROM ProductVariant WHERE ProductVariantId = @Id",
                    new { Id = variantId }, tx);

                await conn.ExecuteAsync(
                    @"INSERT INTO InventoryLog
                        (ProductId, ProductVariantId, ChangeQuantity,
                         ChangeType, ChangedByUserId, Notes, CreatedAt)
                      VALUES
                        (@ProductId, @VariantId, @Qty,
                         @ChangeType, @UserId, @Notes, GETUTCDATE())",
                    new { ProductId  = productId,
                          VariantId  = variantId,
                          Qty        = qty,
                          ChangeType = changeType,
                          UserId     = changedByUserId,
                          Notes      = notes }, tx);

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task AddVariantAsync(ProductVariant variant)
        {
            variant.IsActive = true;
            await ExecuteAsync(
                @"INSERT INTO ProductVariant
                    (ProductId, VariantName, SKU, StockQuantity,
                     ReorderThreshold, IsActive, UpdatedAt)
                  VALUES
                    (@ProductId, @VariantName, @SKU, @StockQuantity,
                     @ReorderThreshold, @IsActive, GETUTCDATE())",
                variant);
        }

        public async Task UpdateVariantAsync(ProductVariant variant)
            => await ExecuteAsync(
                @"UPDATE ProductVariant
                  SET VariantName       = @VariantName,
                      SKU               = @SKU,
                      StockQuantity     = @StockQuantity,
                      ReorderThreshold  = @ReorderThreshold,
                      IsActive          = @IsActive,
                      UpdatedAt         = GETUTCDATE()
                  WHERE ProductVariantId = @ProductVariantId",
                variant);

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
            => await QueryAsync<Category>(
                "SELECT * FROM Category ORDER BY Name");

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
            => await QueryAsync<Brand>(
                "SELECT * FROM Brand WHERE IsActive = 1 ORDER BY BrandName");

        // ── Private helpers ───────────────────────────────────────────────

        private static async Task PopulateVariantsAsync(
            Microsoft.Data.SqlClient.SqlConnection conn,
            List<Product> products)
        {
            if (products.Count == 0) return;

            var ids      = products.Select(p => p.ProductId).ToList();
            var variants = (await conn.QueryAsync<ProductVariant>(
                "SELECT * FROM ProductVariant WHERE ProductId IN @Ids AND IsActive = 1 ORDER BY VariantName",
                new { Ids = ids })).ToList();

            var map = variants
                .GroupBy(v => v.ProductId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var p in products)
                p.Variants = map.TryGetValue(p.ProductId, out var list) ? list : new();
        }
    }
}
