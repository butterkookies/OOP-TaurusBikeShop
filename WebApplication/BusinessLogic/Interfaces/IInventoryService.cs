using WebApplication.Models.Entities;

namespace WebApplication.BusinessLogic.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<Product>> GetAllWithStockAsync();
        Task AdjustStockAsync(int productId, int delta, string reason);
        Task<IEnumerable<Product>> GetLowStockAsync(int threshold = 5);
    }
}
