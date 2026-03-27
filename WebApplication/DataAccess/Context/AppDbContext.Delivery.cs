// WebApplication/DataAccess/Context/AppDbContext.Delivery.cs
// Entity configurations: Delivery, LalamoveDelivery, LBCDelivery

using Microsoft.EntityFrameworkCore;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Context;

public sealed partial class AppDbContext
{
    private static void ConfigureDelivery(ModelBuilder mb)
    {
        mb.Entity<Delivery>(e =>
        {
            e.ToTable("Delivery");
            e.HasKey(d => d.DeliveryId);

            e.HasIndex(d => d.OrderId)
                .HasDatabaseName("IX_Delivery_OrderId");

            e.HasIndex(d => d.Courier)
                .HasDatabaseName("IX_Delivery_Courier");

            e.HasIndex(d => d.DeliveryStatus)
                .HasDatabaseName("IX_Delivery_Status");

            e.HasIndex(d => d.IsDelayed)
                .HasFilter("[IsDelayed] = 1")
                .HasDatabaseName("IX_Delivery_IsDelayed");

            e.HasOne(d => d.Order)
                .WithMany(o => o.Deliveries)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureLalamoveDelivery(ModelBuilder mb)
    {
        mb.Entity<LalamoveDelivery>(e =>
        {
            e.ToTable("LalamoveDelivery");

            // Shared PK — not auto-generated, copied from parent Delivery.DeliveryId
            e.HasKey(l => l.DeliveryId);
            e.Property(l => l.DeliveryId).ValueGeneratedNever();

            e.HasIndex(l => l.BookingRef)
                .HasFilter("[BookingRef] IS NOT NULL")
                .HasDatabaseName("IX_LalamoveDelivery_BookingRef");

            // Shared-PK 1:1 relationship
            e.HasOne(l => l.Delivery)
                .WithOne(d => d.LalamoveDelivery)
                .HasForeignKey<LalamoveDelivery>(l => l.DeliveryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureLBCDelivery(ModelBuilder mb)
    {
        mb.Entity<LBCDelivery>(e =>
        {
            e.ToTable("LBCDelivery");

            // Shared PK — not auto-generated, copied from parent Delivery.DeliveryId
            e.HasKey(l => l.DeliveryId);
            e.Property(l => l.DeliveryId).ValueGeneratedNever();

            e.HasIndex(l => l.TrackingNumber)
                .HasFilter("[TrackingNumber] IS NOT NULL")
                .HasDatabaseName("IX_LBCDelivery_TrackingNumber");

            // Shared-PK 1:1 relationship
            e.HasOne(l => l.Delivery)
                .WithOne(d => d.LBCDelivery)
                .HasForeignKey<LBCDelivery>(l => l.DeliveryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
