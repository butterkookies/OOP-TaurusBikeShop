using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId);
        Task<Order?> GetOrderWithItemsAsync(int orderId);
        Task<Order?> GetByOrderNumberAsync(string orderNumber);
    }

    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId)
            => await _dbSet.Where(o => o.UserId == userId)
                           .OrderByDescending(o => o.OrderDate)
                           .ToListAsync();

        public async Task<Order?> GetOrderWithItemsAsync(int orderId)
            => await _dbSet.Include(o => o.OrderItems)
                               .ThenInclude(oi => oi.Product)
                           .Include(o => o.Payments)
                           .Include(o => o.Deliveries)
                           .FirstOrDefaultAsync(o => o.OrderId == orderId);

        public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
            => await _dbSet.Include(o => o.OrderItems)
                               .ThenInclude(oi => oi.Product)
                           .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
    }
}
