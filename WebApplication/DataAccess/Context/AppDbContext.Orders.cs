// WebApplication/DataAccess/Context/AppDbContext.Orders.cs
// Entity configurations: Order, OrderItem, PickupOrder, InventoryLog

using Microsoft.EntityFrameworkCore;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Context;

public sealed partial class AppDbContext
{
    private static void ConfigureOrder(ModelBuilder mb)
    {
        mb.Entity<Order>(e =>
        {
            e.ToTable("Order");
            e.HasKey(o => o.OrderId);

            e.Property(o => o.DiscountAmount).HasPrecision(18, 2);
            e.Property(o => o.ShippingFee).HasPrecision(18, 2);
            e.Property(o => o.SubTotal).HasPrecision(18, 2);

            e.HasIndex(o => o.OrderNumber)
                .IsUnique()
                .HasDatabaseName("IX_Order_OrderNumber");

            e.HasIndex(o => o.UserId)
                .HasDatabaseName("IX_Order_UserId");

            e.HasIndex(o => o.OrderStatus)
                .HasDatabaseName("IX_Order_OrderStatus");

            e.HasIndex(o => o.OrderDate)
                .HasDatabaseName("IX_Order_OrderDate");

            e.HasIndex(o => o.IsWalkIn)
                .HasDatabaseName("IX_Order_IsWalkIn");

            e.HasIndex(o => o.ShippingAddressId)
                .HasFilter("[ShippingAddressId] IS NOT NULL")
                .HasDatabaseName("IX_Order_ShippingAddressId");

            e.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(o => o.ShippingAddress)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.ShippingAddressId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureOrderItem(ModelBuilder mb)
    {
        mb.Entity<OrderItem>(e =>
        {
            e.ToTable("OrderItem");
            e.HasKey(oi => oi.OrderItemId);

            e.Property(oi => oi.UnitPrice).HasPrecision(18, 2);

            e.HasIndex(oi => oi.OrderId)
                .HasDatabaseName("IX_OrderItem_OrderId");

            e.HasIndex(oi => oi.ProductId)
                .HasDatabaseName("IX_OrderItem_ProductId");

            e.HasIndex(oi => oi.ProductVariantId)
                .HasDatabaseName("IX_OrderItem_ProductVariantId");

            e.HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(oi => oi.Variant)
                .WithMany()
                .HasForeignKey(oi => oi.ProductVariantId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigurePickupOrder(ModelBuilder mb)
    {
        mb.Entity<PickupOrder>(e =>
        {
            e.ToTable("PickupOrder");
            e.HasKey(po => po.PickupOrderId);

            // PickupOrderId is an IDENTITY column — EF Core must not suppress generation.

            e.HasIndex(po => po.OrderId)
                .IsUnique()
                .HasDatabaseName("UX_PickupOrder_Order");

            e.HasIndex(po => po.PickupExpiresAt)
                .HasFilter("[PickupExpiresAt] IS NOT NULL")
                .HasDatabaseName("IX_PickupOrder_ExpiresAt");

            e.HasIndex(po => po.OrderId)
                .HasDatabaseName("IX_PickupOrder_OrderId");

            // FK is OrderId (not PickupOrderId) — matches FK_PickupOrder_Order in schema.
            e.HasOne(po => po.Order)
                .WithOne(o => o.PickupOrder)
                .HasForeignKey<PickupOrder>(po => po.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureInventoryLog(ModelBuilder mb)
    {
        mb.Entity<InventoryLog>(e =>
        {
            e.ToTable("InventoryLog");
            e.HasKey(il => il.InventoryLogId);

            e.HasIndex(il => il.ProductId)
                .HasDatabaseName("IX_InvLog_ProductId");

            e.HasIndex(il => il.ProductVariantId)
                .HasDatabaseName("IX_InvLog_ProductVariantId");

            e.HasIndex(il => il.OrderId)
                .HasDatabaseName("IX_InvLog_OrderId");

            e.HasIndex(il => il.PurchaseOrderId)
                .HasDatabaseName("IX_InvLog_PurchaseOrderId");

            e.HasIndex(il => il.ChangeType)
                .HasDatabaseName("IX_InvLog_ChangeType");

            e.HasIndex(il => il.CreatedAt)
                .HasDatabaseName("IX_InvLog_CreatedAt");

            e.HasOne(il => il.Product)
                .WithMany()
                .HasForeignKey(il => il.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(il => il.Variant)
                .WithMany()
                .HasForeignKey(il => il.ProductVariantId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(il => il.Order)
                .WithMany(o => o.InventoryLogs)
                .HasForeignKey(il => il.OrderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(il => il.PurchaseOrder)
                .WithMany(po => po.InventoryLogs)
                .HasForeignKey(il => il.PurchaseOrderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(il => il.ChangedBy)
                .WithMany()
                .HasForeignKey(il => il.ChangedByUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
