using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;

        public CartService(ICartRepository cartRepo, IProductRepository productRepo)
        {
            _cartRepo    = cartRepo;
            _productRepo = productRepo;
        }

        public async Task<CartViewModel> GetCartAsync(int userId)
        {
            var items = await _cartRepo.GetCartByUserAsync(userId);
            return new CartViewModel
            {
                Items = items.Select(ci => new CartItemViewModel
                {
                    CartItemId  = ci.CartItemId,
                    ProductId   = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "Unknown",
                    Quantity    = ci.Quantity,
                    UnitPrice   = ci.PriceAtAdd
                }).ToList()
            };
        }

        public async Task AddToCartAsync(int userId, int productId, int quantity)
        {
            if (quantity < 1) quantity = 1;

            var existing = await _cartRepo.GetCartItemAsync(userId, productId);
            if (existing != null)
            {
                existing.Quantity += quantity;
                _cartRepo.Update(existing);
            }
            else
            {
                var product = await _productRepo.GetByIdAsync(productId)
                    ?? throw new InvalidOperationException("Product not found.");

                await _cartRepo.AddAsync(new CartItem
                {
                    UserId     = userId,
                    ProductId  = productId,
                    Quantity   = quantity,
                    PriceAtAdd = product.Price,
                    AddedAt    = DateTime.UtcNow
                });
            }
            await _cartRepo.SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(int userId, int productId, int quantity)
        {
            var item = await _cartRepo.GetCartItemAsync(userId, productId);
            if (item == null) return;

            if (quantity < 1)
            {
                _cartRepo.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
                _cartRepo.Update(item);
            }
            await _cartRepo.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            var item = await _cartRepo.GetCartItemAsync(userId, productId);
            if (item != null)
            {
                _cartRepo.Remove(item);
                await _cartRepo.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(int userId)
            => await _cartRepo.ClearCartAsync(userId);

        public async Task<int> GetCartCountAsync(int userId)
        {
            var items = await _cartRepo.GetCartByUserAsync(userId);
            return items.Sum(ci => ci.Quantity);
        }
    }
}
