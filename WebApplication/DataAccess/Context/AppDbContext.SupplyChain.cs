// WebApplication/DataAccess/Context/AppDbContext.SupplyChain.cs
// Entity configurations: Supplier, PurchaseOrder, PurchaseOrderItem

using Microsoft.EntityFrameworkCore;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Context;

public sealed partial class AppDbContext
{
    private static void ConfigureSupplier(ModelBuilder mb)
    {
        mb.Entity<Supplier>(e =>
        {
            e.ToTable("Supplier");
            e.HasKey(s => s.SupplierId);

            // Filtered unique email — unique when non-NULL
            e.HasIndex(s => s.Email)
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL")
                .HasDatabaseName("UX_Supplier_Email");

            e.HasIndex(s => s.IsActive)
                .HasDatabaseName("IX_Supplier_IsActive");
        });
    }

    private static void ConfigurePurchaseOrder(ModelBuilder mb)
    {
        mb.Entity<PurchaseOrder>(e =>
        {
            e.ToTable("PurchaseOrder");
            e.HasKey(po => po.PurchaseOrderId);

            e.HasIndex(po => po.SupplierId)
                .HasDatabaseName("IX_PurchaseOrder_SupplierId");

            e.HasIndex(po => po.Status)
                .HasDatabaseName("IX_PurchaseOrder_Status");

            e.HasIndex(po => po.OrderDate)
                .HasDatabaseName("IX_PurchaseOrder_OrderDate");

            e.HasIndex(po => po.CreatedByUserId)
                .HasDatabaseName("IX_PurchaseOrder_CreatedByUserId");

            e.HasOne(po => po.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(po => po.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(po => po.CreatedBy)
                .WithMany()
                .HasForeignKey(po => po.CreatedByUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigurePurchaseOrderItem(ModelBuilder mb)
    {
        mb.Entity<PurchaseOrderItem>(e =>
        {
            e.ToTable("PurchaseOrderItem");
            e.HasKey(poi => poi.PurchaseOrderItemId);

            e.Property(poi => poi.UnitPrice).HasPrecision(18, 2);

            e.HasIndex(poi => poi.PurchaseOrderId)
                .HasDatabaseName("IX_POItem_PurchaseOrderId");

            e.HasIndex(poi => poi.ProductId)
                .HasDatabaseName("IX_POItem_ProductId");

            e.HasIndex(poi => poi.ProductVariantId)
                .HasDatabaseName("IX_POItem_ProductVariantId");

            e.HasOne(poi => poi.PurchaseOrder)
                .WithMany(po => po.Items)
                .HasForeignKey(poi => poi.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(poi => poi.Product)
                .WithMany()
                .HasForeignKey(poi => poi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(poi => poi.Variant)
                .WithMany()
                .HasForeignKey(poi => poi.ProductVariantId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
