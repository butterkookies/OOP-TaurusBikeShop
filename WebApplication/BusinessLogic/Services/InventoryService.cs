using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BusinessLogic.Services
{
    public class InventoryService : IInventoryService
    {
        readonly AppDbContext _ctx;
        readonly ILogger<InventoryService> _logger;

        public InventoryService(AppDbContext ctx, ILogger<InventoryService> logger)
        {
            _ctx    = ctx;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllWithStockAsync()
        {
            return await _ctx.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .OrderBy(p => p.StockQuantity)
                .ToListAsync();
        }

        public async Task AdjustStockAsync(int productId, int delta, string reason)
        {
            var product = await _ctx.Products.FindAsync(productId)
                ?? throw new KeyNotFoundException($"Product {productId} not found.");

            var before = product.StockQuantity;
            product.StockQuantity = Math.Max(0, product.StockQuantity + delta);
            product.UpdatedAt     = DateTime.UtcNow;

            await _ctx.SaveChangesAsync();

            _logger.LogInformation(
                "Stock adjusted for {ProductName} (Id={ProductId}): {Before} -> {After}. Reason: {Reason}",
                product.Name, product.ProductId, before, product.StockQuantity, reason);
        }

        public async Task<IEnumerable<Product>> GetLowStockAsync(int threshold = 5)
        {
            return await _ctx.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.StockQuantity <= threshold)
                .OrderBy(p => p.StockQuantity)
                .ToListAsync();
        }
    }
}
