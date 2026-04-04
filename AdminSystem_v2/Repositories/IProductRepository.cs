using AdminSystem_v2.Models;

namespace AdminSystem_v2.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<int>                      GetTotalCountAsync();
        Task<IEnumerable<Product>>     SearchAsync(string searchText);
        Task                           AdjustStockAsync(int variantId, int qty,
                                           string changeType, int changedByUserId,
                                           string? notes = null);
        Task                           AddVariantAsync(ProductVariant variant);
        Task                           UpdateVariantAsync(ProductVariant variant);
        Task<IEnumerable<Category>>    GetAllCategoriesAsync();
        Task<IEnumerable<Brand>>       GetAllBrandsAsync();
    }
}
