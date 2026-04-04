using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> SearchAsync(string query);
        Task<Product?>             GetByIdAsync(int productId);
        Task<int>                  GetTotalCountAsync();
        Task<int>                  CreateAsync(Product product);
        Task                       UpdateAsync(Product product);
        Task                       DeactivateAsync(int productId);
        Task                       AdjustStockAsync(int variantId, int qty,
                                       string changeType, string? notes = null);
        Task                       AddVariantAsync(ProductVariant variant);
        Task                       UpdateVariantAsync(ProductVariant variant);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Brand>>   GetAllBrandsAsync();
    }
}
