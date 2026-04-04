using AdminSystem_v2.Models;
using AdminSystem_v2.Repositories;

namespace AdminSystem_v2.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepo;

        public InventoryService(IInventoryRepository inventoryRepo)
        {
            _inventoryRepo = inventoryRepo;
        }

        public Task<IEnumerable<LowStockVariant>> GetLowStockVariantsAsync()
            => _inventoryRepo.GetLowStockVariantsAsync();
    }
}
