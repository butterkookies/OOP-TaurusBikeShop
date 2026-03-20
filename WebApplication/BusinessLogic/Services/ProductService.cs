using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;

namespace WebApplication.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        public ProductService(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IEnumerable<Product>> GetAllActiveProductsAsync()
            => await _productRepo.GetActiveProductsAsync();

        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
            => await _productRepo.GetFeaturedProductsAsync();

        public async Task<Product?> GetProductByIdAsync(int id)
            => await _productRepo.GetWithCategoryAsync(id);

        public async Task<IEnumerable<Product>> SearchProductsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return await _productRepo.GetActiveProductsAsync();

            var q = query.Trim().ToLower();
            return await _productRepo.FindAsync(p =>
                p.IsActive &&
                (p.Name.ToLower().Contains(q) ||
                 (p.ShortDescription != null && p.ShortDescription.ToLower().Contains(q))));
        }
    }
}
