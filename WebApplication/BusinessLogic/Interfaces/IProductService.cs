using WebApplication.Models.Entities;

namespace WebApplication.BusinessLogic.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllActiveProductsAsync();
        Task<IEnumerable<Product>> GetFeaturedProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> SearchProductsAsync(string query);
    }
}
