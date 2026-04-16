// WebApplication/DataAccess/Context/AppDbContext.Payments.cs
// Entity configurations: Payment, GCashPayment, BankTransferPayment, StorePaymentAccount

using Microsoft.EntityFrameworkCore;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Context;

public sealed partial class AppDbContext
{
    private static void ConfigurePayment(ModelBuilder mb)
    {
        mb.Entity<Payment>(e =>
        {
            e.ToTable("Payment");
            e.HasKey(p => p.PaymentId);

            e.Property(p => p.Amount).HasPrecision(18, 2);

            e.HasIndex(p => p.OrderId)
                .HasDatabaseName("IX_Payment_OrderId");

            e.HasIndex(p => p.PaymentMethod)
                .HasDatabaseName("IX_Payment_Method");

            e.HasIndex(p => p.PaymentStatus)
                .HasDatabaseName("IX_Payment_Status");

            e.HasIndex(p => p.PaymentStage)
                .HasDatabaseName("IX_Payment_Stage");

            e.HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureGCashPayment(ModelBuilder mb)
    {
        mb.Entity<GCashPayment>(e =>
        {
            e.ToTable("GCashPayment");

            // Shared PK — not auto-generated, copied from parent Payment.PaymentId
            e.HasKey(g => g.PaymentId);
            e.Property(g => g.PaymentId).ValueGeneratedNever();

            e.HasIndex(g => g.GcashTransactionId)
                .HasFilter("[GcashTransactionId] IS NOT NULL")
                .HasDatabaseName("IX_GCashPayment_TxnId");

            // Shared-PK 1:1 relationship
            e.HasOne(g => g.Payment)
                .WithOne(p => p.GCashPayment)
                .HasForeignKey<GCashPayment>(g => g.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureBankTransferPayment(ModelBuilder mb)
    {
        mb.Entity<BankTransferPayment>(e =>
        {
            e.ToTable("BankTransferPayment");

            // Shared PK — not auto-generated, copied from parent Payment.PaymentId
            e.HasKey(bt => bt.PaymentId);
            e.Property(bt => bt.PaymentId).ValueGeneratedNever();

            e.HasIndex(bt => bt.BpiReferenceNumber)
                .HasFilter("[BpiReferenceNumber] IS NOT NULL")
                .HasDatabaseName("IX_BTP_BpiRef");

            e.HasIndex(bt => bt.VerificationDeadline)
                .HasFilter("[VerificationDeadline] IS NOT NULL")
                .HasDatabaseName("IX_BTP_VerificationDeadline");

            // Shared-PK 1:1 relationship
            e.HasOne(bt => bt.Payment)
                .WithOne(p => p.BankTransferPayment)
                .HasForeignKey<BankTransferPayment>(bt => bt.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Separate FK to User for the admin who verified the payment
            e.HasOne(bt => bt.VerifiedBy)
                .WithMany()
                .HasForeignKey(bt => bt.VerifiedByUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureStorePaymentAccount(ModelBuilder mb)
    {
        mb.Entity<StorePaymentAccount>(e =>
        {
            e.ToTable("StorePaymentAccount");
            e.HasKey(a => a.StorePaymentAccountId);

            // Filtered unique index — at most one active account per method.
            e.HasIndex(a => a.PaymentMethod)
                .HasFilter("[IsActive] = 1")
                .IsUnique()
                .HasDatabaseName("UX_StorePaymentAccount_ActivePerMethod");

            e.HasIndex(a => new { a.PaymentMethod, a.DisplayOrder })
                .HasDatabaseName("IX_StorePaymentAccount_Method");
        });
    }
}
