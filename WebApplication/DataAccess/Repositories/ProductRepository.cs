using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task<IEnumerable<Product>> GetFeaturedProductsAsync();
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<Product?> GetWithCategoryAsync(int productId);
    }

    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
            => await _dbSet.Include(p => p.Category)
                           .Where(p => p.IsActive)
                           .OrderBy(p => p.Name)
                           .ToListAsync();

        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
            => await _dbSet.Include(p => p.Category)
                           .Where(p => p.IsActive && p.IsFeatured)
                           .ToListAsync();

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
            => await _dbSet.Where(p => p.CategoryId == categoryId && p.IsActive)
                           .ToListAsync();

        public async Task<Product?> GetWithCategoryAsync(int productId)
            => await _dbSet.Include(p => p.Category)
                           .FirstOrDefaultAsync(p => p.ProductId == productId);
    }
}
