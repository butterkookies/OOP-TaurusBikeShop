// WebApplication/BusinessLogic/Services/WishlistService.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Services;

/// <summary>
/// Implements <see cref="IWishlistService"/> — add, remove, toggle,
/// and list wishlist products for authenticated customers.
/// </summary>
public sealed class WishlistService : IWishlistService
{
    private readonly WishlistRepository _wishlistRepo;

    /// <inheritdoc/>
    public WishlistService(WishlistRepository wishlistRepo)
    {
        _wishlistRepo = wishlistRepo ?? throw new ArgumentNullException(nameof(wishlistRepo));
    }

    /// <inheritdoc/>
    public async Task<WishlistViewModel> GetWishlistAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Wishlist> entries =
            await _wishlistRepo.GetByUserWithProductAsync(userId, cancellationToken);

        IReadOnlyList<WishlistItemViewModel> items = entries
            .Select(w => new WishlistItemViewModel
            {
                WishlistId      = w.WishlistId,
                ProductId       = w.ProductId,
                Name            = w.Product.Name,
                ShortDescription= w.Product.ShortDescription,
                Price           = w.Product.Price,
                Brand           = w.Product.Brand?.BrandName,
                PrimaryImageUrl = w.Product.Images
                    .FirstOrDefault(i => i.IsPrimary)?.ImageUrl
                    ?? w.Product.Images.FirstOrDefault()?.ImageUrl,
                StockQuantity   = w.Product.Variants
                    .Where(v => v.IsActive)
                    .Sum(v => v.StockQuantity),
                AddedAt         = w.AddedAt
            })
            .ToList()
            .AsReadOnly();

        return new WishlistViewModel { Items = items };
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> AddAsync(
        int userId,
        int productId,
        CancellationToken cancellationToken = default)
    {
        // Verify the product exists and is active
        bool productExists = await _wishlistRepo.Context.Products
            .AnyAsync(p => p.ProductId == productId && p.IsActive, cancellationToken);

        if (!productExists)
            return ServiceResult.Fail("Product not found.");

        await _wishlistRepo.AddIdempotentAsync(userId, productId, cancellationToken);
        return ServiceResult.Ok();
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> RemoveAsync(
        int userId,
        int productId,
        CancellationToken cancellationToken = default)
    {
        Wishlist? entry = await _wishlistRepo.Context.Wishlists
            .FirstOrDefaultAsync(
                w => w.UserId == userId && w.ProductId == productId,
                cancellationToken);

        if (entry is null)
            return ServiceResult.Ok(); // Already removed — idempotent

        _wishlistRepo.Context.Wishlists.Remove(entry);
        await _wishlistRepo.Context.SaveChangesAsync(cancellationToken);
        return ServiceResult.Ok();
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<bool>> ToggleAsync(
        int userId,
        int productId,
        CancellationToken cancellationToken = default)
    {
        bool currentlyInWishlist = await _wishlistRepo.Context.Wishlists
            .AnyAsync(
                w => w.UserId == userId && w.ProductId == productId,
                cancellationToken);

        if (currentlyInWishlist)
        {
            ServiceResult removeResult = await RemoveAsync(userId, productId, cancellationToken);
            if (!removeResult.IsSuccess)
                return ServiceResult<bool>.Fail(removeResult.Error!);
            return ServiceResult<bool>.Ok(false); // now NOT in wishlist
        }
        else
        {
            ServiceResult addResult = await AddAsync(userId, productId, cancellationToken);
            if (!addResult.IsSuccess)
                return ServiceResult<bool>.Fail(addResult.Error!);
            return ServiceResult<bool>.Ok(true); // now IS in wishlist
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<int>> GetProductIdsAsync(
        int userId,
        CancellationToken cancellationToken = default)
        => await _wishlistRepo.GetProductIdsForUserAsync(userId, cancellationToken);

    /// <inheritdoc/>
    public async Task<int> GetCountAsync(
        int userId,
        CancellationToken cancellationToken = default)
        => await _wishlistRepo.GetWishlistCountAsync(userId, cancellationToken);
}