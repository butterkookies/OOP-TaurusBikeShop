using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Repositories;

namespace AdminSystem_v2.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        public ProductService(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        private static string CallerRole => App.CurrentUser?.Role ?? string.Empty;

        public Task<IEnumerable<Product>> GetAllAsync()
            => _productRepo.GetAllAsync();

        public Task<IEnumerable<Product>> SearchAsync(string query)
            => _productRepo.SearchAsync(query);

        public Task<Product?> GetByIdAsync(int productId)
            => _productRepo.GetByIdAsync(productId);

        public Task<int> GetTotalCountAsync()
            => _productRepo.GetTotalCountAsync();

        public Task<int> CreateAsync(Product product)
        {
            RoleGuard.RequireAdminOrManager(CallerRole);
            return _productRepo.InsertAsync(product);
        }

        public Task UpdateAsync(Product product)
        {
            RoleGuard.RequireAdminOrManager(CallerRole);
            return _productRepo.UpdateAsync(product);
        }

        public Task DeactivateAsync(int productId)
        {
            RoleGuard.RequireAdminOrManager(CallerRole);
            return _productRepo.DeleteAsync(productId);
        }

        public Task AdjustStockAsync(int variantId, int qty,
            string changeType, string? notes = null)
        {
            if (qty == 0)
                throw new ArgumentException("Adjustment quantity cannot be zero.");

            int userId = App.CurrentUser?.UserId ?? 0;
            return _productRepo.AdjustStockAsync(variantId, qty, changeType, userId, notes);
        }

        public Task AddVariantAsync(ProductVariant variant)
        {
            RoleGuard.RequireAdminOrManager(CallerRole);
            if (string.IsNullOrWhiteSpace(variant.VariantName))
                throw new ArgumentException("Variant name is required.");
            if (variant.StockQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative.");
            return _productRepo.AddVariantAsync(variant);
        }

        public Task UpdateVariantAsync(ProductVariant variant)
        {
            RoleGuard.RequireAdminOrManager(CallerRole);
            return _productRepo.UpdateVariantAsync(variant);
        }

        public Task<IEnumerable<Category>> GetAllCategoriesAsync()
            => _productRepo.GetAllCategoriesAsync();

        public Task<IEnumerable<Brand>> GetAllBrandsAsync()
            => _productRepo.GetAllBrandsAsync();

        public Task<IEnumerable<ProductImage>> GetImagesAsync(int productId)
            => _productRepo.GetImagesAsync(productId);

        public Task<int> AddImageAsync(int productId, string imageUrl)
        {
            RoleGuard.RequireAdminOrManager(CallerRole);
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("Image URL cannot be empty.");
            return _productRepo.AddImageAsync(productId, imageUrl.Trim());
        }

        public Task DeleteImageAsync(int imageId)
        {
            RoleGuard.RequireAdminOrManager(CallerRole);
            return _productRepo.DeleteImageAsync(imageId);
        }
    }
}
