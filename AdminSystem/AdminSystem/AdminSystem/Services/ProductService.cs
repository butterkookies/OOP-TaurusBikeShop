using System;
using System.Collections.Generic;
using AdminSystem.Helpers;
using AdminSystem.Models;
using AdminSystem.Repositories;
using Dapper;

namespace AdminSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepo;

        public ProductService(ProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public IEnumerable<Product> GetAllProducts()
            => _productRepo.GetAll();

        public IEnumerable<Product> SearchProducts(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return _productRepo.GetAll();
            return _productRepo.Search(query.Trim());
        }

        public Product GetProductById(int productId)
            => _productRepo.GetById(productId);

        public int CreateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name is required.");
            if (product.Price < 0)
                throw new ArgumentException("Price cannot be negative.");

            product.IsActive = true;
            return _productRepo.Insert(product);
        }

        public void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name is required.");
            if (product.Price < 0)
                throw new ArgumentException("Price cannot be negative.");

            _productRepo.Update(product);
        }

        public void DeactivateProduct(int productId)
            => _productRepo.Delete(productId);

        public void AddVariant(ProductVariant variant)
        {
            if (variant == null)
                throw new ArgumentNullException("variant");
            if (string.IsNullOrWhiteSpace(variant.VariantName))
                throw new ArgumentException("Variant name is required.");
            if (variant.StockQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative.");

            variant.IsActive = true;
            using (System.Data.SqlClient.SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Execute(
                    @"INSERT INTO ProductVariant
                        (ProductId, VariantName, SKU, StockQuantity,
                         ReorderThreshold, IsActive, CreatedAt, UpdatedAt)
                      VALUES
                        (@ProductId, @VariantName, @SKU, @StockQuantity,
                         @ReorderThreshold, @IsActive, GETUTCDATE(), GETUTCDATE())",
                    variant);
            }
        }

        public void UpdateVariant(ProductVariant variant)
        {
            if (variant == null)
                throw new ArgumentNullException("variant");

            using (System.Data.SqlClient.SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Execute(
                    @"UPDATE ProductVariant
                      SET VariantName      = @VariantName,
                          SKU              = @SKU,
                          StockQuantity    = @StockQuantity,
                          ReorderThreshold = @ReorderThreshold,
                          IsActive         = @IsActive,
                          UpdatedAt        = GETUTCDATE()
                      WHERE ProductVariantId = @ProductVariantId",
                    variant);
            }
        }

        public void AddProductImage(ProductImage image)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (string.IsNullOrWhiteSpace(image.ImageUrl))
                throw new ArgumentException("Image URL is required.");

            using (System.Data.SqlClient.SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Execute(
                    @"INSERT INTO ProductImage
                        (ProductId, ImageUrl, IsPrimary, SortOrder, CreatedAt)
                      VALUES
                        (@ProductId, @ImageUrl, @IsPrimary, @SortOrder, GETUTCDATE())",
                    image);
            }
        }

        public void DeleteProductImage(int imageId)
        {
            using (System.Data.SqlClient.SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Execute(
                    "DELETE FROM ProductImage WHERE ProductImageId = @Id",
                    new { Id = imageId });
            }
        }

        public IEnumerable<Category> GetAllCategories()
            => _productRepo.GetAllCategories();

        public IEnumerable<Brand> GetAllBrands()
            => _productRepo.GetAllBrands();
    }
}
