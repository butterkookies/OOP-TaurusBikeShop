// WebApplication/BusinessLogic/Services/CartService.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Services;

/// <summary>
/// Implements <see cref="ICartService"/> — full cart lifecycle including
/// guest cart support and guest-to-user merge on login.
/// Flowchart: Part 3 — Cart &amp; Checkout.
/// </summary>
public sealed class CartService : ICartService
{
    private readonly CartRepository    _cartRepo;
    private readonly IProductService   _productService;

    /// <inheritdoc/>
    public CartService(
        CartRepository  cartRepo,
        IProductService productService)
    {
        _cartRepo       = cartRepo       ?? throw new ArgumentNullException(nameof(cartRepo));
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }

    /// <inheritdoc/>
    public async Task<CartViewModel> GetCartAsync(
        int? userId,
        int? guestSessionId,
        CancellationToken cancellationToken = default)
    {
        Cart? cart = await _cartRepo.GetActiveCartAsync(userId, guestSessionId, cancellationToken);

        if (cart is null)
            return new CartViewModel();

        return MapToViewModel(cart);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> AddItemAsync(
        int? userId,
        int? guestSessionId,
        int productId,
        int? variantId,
        int qty,
        CancellationToken cancellationToken = default)
    {
        if (qty < 1)
            return ServiceResult.Fail("Quantity must be at least 1.");

        // Resolve the variant: use provided variantId or find the Default variant
        int resolvedVariantId;
        if (variantId.HasValue)
        {
            resolvedVariantId = variantId.Value;
        }
        else
        {
            ProductVariant? defaultVariant = await _cartRepo.Context.ProductVariants
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    v => v.ProductId == productId && v.IsActive,
                    cancellationToken);

            if (defaultVariant is null)
                return ServiceResult.Fail("This product is not available.");

            resolvedVariantId = defaultVariant.ProductVariantId;
        }

        // Stock check
        bool hasStock = await _productService.CheckStockAsync(resolvedVariantId, qty, cancellationToken);
        if (!hasStock)
            return ServiceResult.Fail("Insufficient stock for the requested quantity.");

        // Get or create active cart
        Cart? cart = await _cartRepo.GetActiveCartAsync(userId, guestSessionId, cancellationToken);
        if (cart is null)
        {
            cart = new Cart
            {
                UserId         = userId,
                GuestSessionId = guestSessionId,
                IsCheckedOut   = false,
                CreatedAt      = DateTime.UtcNow
            };
            await _cartRepo.Context.Carts.AddAsync(cart, cancellationToken);
            await _cartRepo.Context.SaveChangesAsync(cancellationToken);
        }

        // If same variant already in cart — increment quantity
        CartItem? existingItem = await _cartRepo.Context.CartItems
            .FirstOrDefaultAsync(
                ci => ci.CartId == cart.CartId
                   && ci.ProductId == productId
                   && ci.ProductVariantId == resolvedVariantId,
                cancellationToken);

        if (existingItem != null)
        {
            bool hasStockForTotal = await _productService
                .CheckStockAsync(resolvedVariantId, existingItem.Quantity + qty, cancellationToken);
            if (!hasStockForTotal)
                return ServiceResult.Fail("Not enough stock to add that many. Please check your cart.");

            existingItem.Quantity += qty;
        }
        else
        {
            // Get the current variant price snapshot
            (decimal TotalPrice, int StockQuantity)? priceInfo =
                await _productService.GetVariantPriceAsync(resolvedVariantId, cancellationToken);

            if (priceInfo is null)
                return ServiceResult.Fail("Product variant is no longer available.");

            CartItem item = new()
            {
                CartId           = cart.CartId,
                ProductId        = productId,
                ProductVariantId = resolvedVariantId,
                Quantity         = qty,
                PriceAtAdd       = priceInfo.Value.TotalPrice,
                AddedAt          = DateTime.UtcNow
            };
            await _cartRepo.Context.CartItems.AddAsync(item, cancellationToken);
        }

        cart.LastUpdatedAt = DateTime.UtcNow;
        await _cartRepo.Context.SaveChangesAsync(cancellationToken);

        int count = await _cartRepo.GetCartItemCountAsync(cart.CartId, cancellationToken);
        return ServiceResult.Ok();
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> UpdateQuantityAsync(
        int cartItemId,
        int newQty,
        int? ownerUserId,
        CancellationToken cancellationToken = default)
    {
        if (newQty < 1)
            return ServiceResult.Fail("Quantity must be at least 1.");

        CartItem? item = await _cartRepo.GetCartItemAsync(cartItemId, cancellationToken);
        if (item is null)
            return ServiceResult.Fail("Cart item not found.");

        // Ownership check
        Cart? cart = await _cartRepo.GetByIdAsync(item.CartId, cancellationToken);
        if (cart is null || (ownerUserId.HasValue && cart.UserId != ownerUserId))
            return ServiceResult.Fail("Unauthorised.");

        if (item.ProductVariantId.HasValue)
        {
            bool hasStock = await _productService
                .CheckStockAsync(item.ProductVariantId.Value, newQty, cancellationToken);
            if (!hasStock)
                return ServiceResult.Fail("Not enough stock available.");
        }

        item.Quantity          = newQty;
        cart.LastUpdatedAt     = DateTime.UtcNow;
        await _cartRepo.Context.SaveChangesAsync(cancellationToken);

        return ServiceResult.Ok();
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> RemoveItemAsync(
        int cartItemId,
        int? ownerUserId,
        CancellationToken cancellationToken = default)
    {
        CartItem? item = await _cartRepo.GetCartItemAsync(cartItemId, cancellationToken);
        if (item is null)
            return ServiceResult.Fail("Cart item not found.");

        Cart? cart = await _cartRepo.GetByIdAsync(item.CartId, cancellationToken);
        if (cart is null || (ownerUserId.HasValue && cart.UserId != ownerUserId))
            return ServiceResult.Fail("Unauthorised.");

        _cartRepo.Context.CartItems.Remove(item);
        cart.LastUpdatedAt = DateTime.UtcNow;
        await _cartRepo.Context.SaveChangesAsync(cancellationToken);

        return ServiceResult.Ok();
    }

    /// <inheritdoc/>
    public async Task ClearCartAsync(
        int cartId,
        CancellationToken cancellationToken = default)
    {
        Cart? cart = await _cartRepo.GetByIdAsync(cartId, cancellationToken);
        if (cart is null) return;

        cart.IsCheckedOut  = true;
        cart.LastUpdatedAt = DateTime.UtcNow;
        await _cartRepo.Context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task MergeGuestCartAsync(
        int guestSessionId,
        int userId,
        CancellationToken cancellationToken = default)
    {
        Cart? guestCart = await _cartRepo.GetActiveCartAsync(
            null, guestSessionId, cancellationToken);

        if (guestCart is null || !guestCart.Items.Any())
            return;

        Cart? userCart = await _cartRepo.GetActiveCartAsync(
            userId, null, cancellationToken);

        if (userCart is null)
        {
            // Re-own the guest cart instead of creating a new one
            guestCart.UserId         = userId;
            guestCart.GuestSessionId = null;
            guestCart.LastUpdatedAt  = DateTime.UtcNow;
            await _cartRepo.Context.SaveChangesAsync(cancellationToken);
            return;
        }

        // Merge guest items into user cart
        foreach (CartItem guestItem in guestCart.Items)
        {
            CartItem? existingItem = await _cartRepo.Context.CartItems
                .FirstOrDefaultAsync(
                    ci => ci.CartId == userCart.CartId
                       && ci.ProductId == guestItem.ProductId
                       && ci.ProductVariantId == guestItem.ProductVariantId,
                    cancellationToken);

            if (existingItem != null)
                existingItem.Quantity += guestItem.Quantity;
            else
                await _cartRepo.Context.CartItems.AddAsync(new CartItem
                {
                    CartId           = userCart.CartId,
                    ProductId        = guestItem.ProductId,
                    ProductVariantId = guestItem.ProductVariantId,
                    Quantity         = guestItem.Quantity,
                    PriceAtAdd       = guestItem.PriceAtAdd,
                    AddedAt          = DateTime.UtcNow
                }, cancellationToken);
        }

        guestCart.IsCheckedOut  = true;
        userCart.LastUpdatedAt  = DateTime.UtcNow;
        await _cartRepo.Context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> GetCartCountAsync(
        int? userId,
        int? guestSessionId,
        CancellationToken cancellationToken = default)
    {
        Cart? cart = await _cartRepo.GetActiveCartAsync(userId, guestSessionId, cancellationToken);
        if (cart is null) return 0;
        return await _cartRepo.GetCartItemCountAsync(cart.CartId, cancellationToken);
    }

    // =========================================================================
    // Private mapping
    // =========================================================================

    private static CartViewModel MapToViewModel(Cart cart)
    {
        List<CartItemViewModel> items = cart.Items.Select(ci => new CartItemViewModel
        {
            CartItemId  = ci.CartItemId,
            ProductId   = ci.ProductId,
            VariantId   = ci.ProductVariantId,
            ProductName = ci.Product?.Name ?? "Product",
            VariantName = ci.Variant?.VariantName == "Default" ? null : ci.Variant?.VariantName,
            ImageUrl    = ci.Product?.Images?.FirstOrDefault(i => i.IsPrimary)?.ImageUrl
                       ?? ci.Product?.Images?.FirstOrDefault()?.ImageUrl,
            Quantity    = ci.Quantity,
            UnitPrice   = ci.PriceAtAdd
        }).ToList();

        decimal subTotal = items.Sum(i => i.LineTotal);

        return new CartViewModel
        {
            CartId   = cart.CartId,
            Items    = items.AsReadOnly(),
            SubTotal = subTotal
        };
    }
}