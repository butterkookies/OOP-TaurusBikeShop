// WebApplication/DataAccess/Context/AppDbContext.Commerce.cs
// Entity configurations: Voucher, UserVoucher, VoucherUsage, Cart, CartItem, Wishlist

using Microsoft.EntityFrameworkCore;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Context;

public sealed partial class AppDbContext
{
    private static void ConfigureVoucher(ModelBuilder mb)
    {
        mb.Entity<Voucher>(e =>
        {
            e.ToTable("Voucher");
            e.HasKey(v => v.VoucherId);

            e.Property(v => v.DiscountValue).HasPrecision(18, 2);
            e.Property(v => v.MinimumOrderAmount).HasPrecision(18, 2);

            e.HasIndex(v => v.Code)
                .IsUnique()
                .HasDatabaseName("IX_Voucher_Code");

            e.HasIndex(v => v.IsActive)
                .HasDatabaseName("IX_Voucher_IsActive");

            e.HasIndex(v => v.StartDate)
                .HasDatabaseName("IX_Voucher_StartDate");

            e.HasIndex(v => v.EndDate)
                .HasDatabaseName("IX_Voucher_EndDate");
        });
    }

    private static void ConfigureUserVoucher(ModelBuilder mb)
    {
        mb.Entity<UserVoucher>(e =>
        {
            e.ToTable("UserVoucher");
            e.HasKey(uv => uv.UserVoucherId);

            // Unique pair — no duplicate voucher assignments per user
            e.HasIndex(uv => new { uv.UserId, uv.VoucherId })
                .IsUnique()
                .HasDatabaseName("UX_UserVoucher_Pair");

            e.HasIndex(uv => uv.UserId)
                .HasDatabaseName("IX_UserVoucher_UserId");

            e.HasIndex(uv => uv.VoucherId)
                .HasDatabaseName("IX_UserVoucher_VoucherId");

            e.HasOne(uv => uv.User)
                .WithMany(u => u.UserVouchers)
                .HasForeignKey(uv => uv.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(uv => uv.Voucher)
                .WithMany(v => v.UserVouchers)
                .HasForeignKey(uv => uv.VoucherId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureVoucherUsage(ModelBuilder mb)
    {
        mb.Entity<VoucherUsage>(e =>
        {
            e.ToTable("VoucherUsage");
            e.HasKey(vu => vu.VoucherUsageId);

            e.Property(vu => vu.DiscountAmount).HasPrecision(18, 2);

            e.HasIndex(vu => vu.VoucherId)
                .HasDatabaseName("IX_VoucherUsage_VoucherId");

            e.HasIndex(vu => vu.UserId)
                .HasDatabaseName("IX_VoucherUsage_UserId");

            e.HasIndex(vu => vu.OrderId)
                .HasDatabaseName("IX_VoucherUsage_OrderId");

            e.HasOne(vu => vu.Voucher)
                .WithMany(v => v.Usages)
                .HasForeignKey(vu => vu.VoucherId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(vu => vu.User)
                .WithMany()
                .HasForeignKey(vu => vu.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(vu => vu.Order)
                .WithMany(o => o.VoucherUsages)
                .HasForeignKey(vu => vu.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureCart(ModelBuilder mb)
    {
        mb.Entity<Cart>(e =>
        {
            e.ToTable("Cart");
            e.HasKey(c => c.CartId);

            // Filtered unique index — at most one active cart per registered user
            e.HasIndex(c => c.UserId)
                .IsUnique()
                .HasFilter("[IsCheckedOut] = 0 AND [UserId] IS NOT NULL")
                .HasDatabaseName("UX_Cart_User_Active");

            // Filtered unique index — at most one active cart per guest session
            e.HasIndex(c => c.GuestSessionId)
                .IsUnique()
                .HasFilter("[IsCheckedOut] = 0 AND [GuestSessionId] IS NOT NULL")
                .HasDatabaseName("UX_Cart_Guest_Active");

            e.HasIndex(c => c.UserId)
                .HasDatabaseName("IX_Cart_UserId");

            e.HasIndex(c => c.GuestSessionId)
                .HasDatabaseName("IX_Cart_GuestSessionId");

            e.HasIndex(c => c.IsCheckedOut)
                .HasDatabaseName("IX_Cart_IsCheckedOut");

            e.HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(c => c.GuestSession)
                .WithMany(g => g.Carts)
                .HasForeignKey(c => c.GuestSessionId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureCartItem(ModelBuilder mb)
    {
        mb.Entity<CartItem>(e =>
        {
            e.ToTable("CartItem");
            e.HasKey(ci => ci.CartItemId);

            e.Property(ci => ci.PriceAtAdd).HasPrecision(18, 2);

            e.HasIndex(ci => ci.CartId)
                .HasDatabaseName("IX_CartItem_CartId");

            e.HasIndex(ci => ci.ProductId)
                .HasDatabaseName("IX_CartItem_ProductId");

            e.HasIndex(ci => ci.ProductVariantId)
                .HasDatabaseName("IX_CartItem_ProductVariantId");

            e.HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(ci => ci.Variant)
                .WithMany()
                .HasForeignKey(ci => ci.ProductVariantId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureWishlist(ModelBuilder mb)
    {
        mb.Entity<Wishlist>(e =>
        {
            e.ToTable("Wishlist");
            e.HasKey(w => w.WishlistId);

            // Unique pair — one wishlist entry per product per user
            e.HasIndex(w => new { w.UserId, w.ProductId })
                .IsUnique()
                .HasDatabaseName("UX_Wishlist_UserProduct");

            e.HasIndex(w => w.UserId)
                .HasDatabaseName("IX_Wishlist_UserId");

            e.HasIndex(w => w.ProductId)
                .HasDatabaseName("IX_Wishlist_ProductId");

            e.HasOne(w => w.User)
                .WithMany(u => u.Wishlist)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(w => w.Product)
                .WithMany()
                .HasForeignKey(w => w.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
