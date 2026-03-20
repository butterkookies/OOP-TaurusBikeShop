using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories
{
    public interface ICartRepository : IRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetCartByUserAsync(int userId);
        Task<CartItem?> GetCartItemAsync(int userId, int productId);
        Task ClearCartAsync(int userId);
    }

    public class CartRepository : Repository<CartItem>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<CartItem>> GetCartByUserAsync(int userId)
            => await _dbSet.Include(ci => ci.Product)
                           .Where(ci => ci.UserId == userId)
                           .ToListAsync();

        public async Task<CartItem?> GetCartItemAsync(int userId, int productId)
            => await _dbSet.FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

        public async Task ClearCartAsync(int userId)
        {
            var items = _dbSet.Where(ci => ci.UserId == userId);
            _dbSet.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
