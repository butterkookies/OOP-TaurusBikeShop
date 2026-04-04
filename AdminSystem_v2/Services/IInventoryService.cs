using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<LowStockVariant>> GetLowStockVariantsAsync();
    }
}
