using AdminSystem_v2.Models;

namespace AdminSystem_v2.Repositories
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<LowStockVariant>> GetLowStockVariantsAsync();
    }
}
