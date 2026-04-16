// WebApplication/DataAccess/Context/AppDbContext.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Context;

/// <summary>
/// EF Core database context for the Taurus Bike Shop WebApplication.
/// Connects to TaurusBikeShopSqlServer2026 on Google Cloud SQL for SQL Server.
/// <para>
/// All entity relationships, foreign keys, cascade behaviours, unique indexes,
/// and filtered indexes are configured exclusively here via Fluent API in
/// <see cref="OnModelCreating"/>. No relationship data annotations exist on
/// entity classes — only validation annotations (Required, MaxLength) are
/// permitted there.
/// </para>
/// <para>
/// <b>5 shared-PK 1:1 subtypes</b> configured via HasOne/WithOne/HasForeignKey:
/// <see cref="GCashPayment"/>, <see cref="BankTransferPayment"/>,
/// <see cref="LalamoveDelivery"/>, <see cref="LBCDelivery"/>,
/// <see cref="PickupOrder"/>.
/// </para>
/// <para>
/// <b>Circular FK:</b> <c>User.DefaultAddressId → Address.AddressId</c> is
/// configured with <c>IsRequired(false)</c> and no cascade delete to break
/// the dependency cycle.
/// </para>
/// </summary>
public sealed partial class AppDbContext : DbContext
{
    /// <inheritdoc/>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // =========================================================================
    // DbSets — all 38 tables
    // =========================================================================

    // Group 1 — Auth & Users
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<OTPVerification> OTPVerifications => Set<OTPVerification>();
    public DbSet<GuestSession> GuestSessions => Set<GuestSession>();

    // Group 2 — Product Catalog
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<PriceHistory> PriceHistories => Set<PriceHistory>();

    // Group 3 — Supply Chain
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();

    // Group 4 — Vouchers
    public DbSet<Voucher> Vouchers => Set<Voucher>();
    public DbSet<UserVoucher> UserVouchers => Set<UserVoucher>();
    public DbSet<VoucherUsage> VoucherUsages => Set<VoucherUsage>();

    // Group 5 — Cart & Wishlist
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();

    // Group 6 — Orders
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<PickupOrder> PickupOrders => Set<PickupOrder>();

    // Group 7 — Inventory
    public DbSet<InventoryLog> InventoryLogs => Set<InventoryLog>();

    // Group 8 — Payments
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<GCashPayment> GCashPayments => Set<GCashPayment>();
    public DbSet<BankTransferPayment> BankTransferPayments => Set<BankTransferPayment>();
    public DbSet<StorePaymentAccount> StorePaymentAccounts => Set<StorePaymentAccount>();

    // Group 9 — Delivery
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<LalamoveDelivery> LalamoveDeliveries => Set<LalamoveDelivery>();
    public DbSet<LBCDelivery> LBCDeliveries => Set<LBCDelivery>();

    // Group 10 — Reviews & POS
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<POS_Session> POSSessions => Set<POS_Session>();

    // Group 11 — Support
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
    public DbSet<SupportTicketReply> SupportTicketReplies => Set<SupportTicketReply>();
    public DbSet<SupportTask> SupportTasks => Set<SupportTask>();

    // Group 12 — Comms & Audit
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<SystemLog> SystemLogs => Set<SystemLog>();

    // =========================================================================
    // Model configuration — delegates to partial class files by domain group
    // =========================================================================

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUser(modelBuilder);
        ConfigureRole(modelBuilder);
        ConfigureUserRole(modelBuilder);
        ConfigureAddress(modelBuilder);
        ConfigureOTPVerification(modelBuilder);
        ConfigureGuestSession(modelBuilder);
        ConfigureCategory(modelBuilder);
        ConfigureBrand(modelBuilder);
        ConfigureProduct(modelBuilder);
        ConfigureProductVariant(modelBuilder);
        ConfigureProductImage(modelBuilder);
        ConfigurePriceHistory(modelBuilder);
        ConfigureSupplier(modelBuilder);
        ConfigurePurchaseOrder(modelBuilder);
        ConfigurePurchaseOrderItem(modelBuilder);
        ConfigureVoucher(modelBuilder);
        ConfigureUserVoucher(modelBuilder);
        ConfigureVoucherUsage(modelBuilder);
        ConfigureCart(modelBuilder);
        ConfigureCartItem(modelBuilder);
        ConfigureWishlist(modelBuilder);
        ConfigureOrder(modelBuilder);
        ConfigureOrderItem(modelBuilder);
        ConfigurePickupOrder(modelBuilder);
        ConfigureInventoryLog(modelBuilder);
        ConfigurePayment(modelBuilder);
        ConfigureGCashPayment(modelBuilder);
        ConfigureBankTransferPayment(modelBuilder);
        ConfigureStorePaymentAccount(modelBuilder);
        ConfigureDelivery(modelBuilder);
        ConfigureLalamoveDelivery(modelBuilder);
        ConfigureLBCDelivery(modelBuilder);
        ConfigureReview(modelBuilder);
        ConfigurePOSSession(modelBuilder);
        ConfigureSupportTicket(modelBuilder);
        ConfigureSupportTicketReply(modelBuilder);
        ConfigureSupportTask(modelBuilder);
        ConfigureNotification(modelBuilder);
        ConfigureSystemLog(modelBuilder);
    }
}
