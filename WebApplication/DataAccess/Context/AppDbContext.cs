// WebApplication/DataAccess/Context/AppDbContext.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Context;

/// <summary>
/// EF Core database context for the Taurus Bike Shop WebApplication.
/// Connects to TaurusBikeShopDB on Google Cloud SQL for SQL Server.
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
public sealed class AppDbContext : DbContext
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
    // Model configuration
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

    // =========================================================================
    // Per-entity configuration methods
    // =========================================================================

    private static void ConfigureUser(ModelBuilder mb)
    {
        mb.Entity<User>(e =>
        {
            e.ToTable("User");
            e.HasKey(u => u.UserId);

            // Filtered unique index — email must be unique when non-NULL
            // (walk-in placeholder users have NULL email)
            e.HasIndex(u => u.Email)
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL")
                .HasDatabaseName("UX_User_Email");

            e.HasIndex(u => u.IsActive)
                .HasDatabaseName("IX_User_IsActive");

            e.HasIndex(u => u.IsWalkIn)
                .HasDatabaseName("IX_User_IsWalkIn");

            e.HasIndex(u => u.PhoneNumber)
                .HasDatabaseName("IX_User_PhoneNumber");

            e.HasIndex(u => u.DefaultAddressId)
                .HasFilter("[DefaultAddressId] IS NOT NULL")
                .HasDatabaseName("IX_User_DefaultAddressId");

            // Circular FK: User.DefaultAddressId → Address.AddressId
            // IsRequired(false) + no cascade delete breaks the dependency cycle.
            // The Address side (Address.UserId → User.UserId) uses CASCADE DELETE.
            e.HasOne(u => u.DefaultAddress)
                .WithMany()
                .HasForeignKey(u => u.DefaultAddressId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureRole(ModelBuilder mb)
    {
        mb.Entity<Role>(e =>
        {
            e.ToTable("Role");
            e.HasKey(r => r.RoleId);

            e.HasIndex(r => r.RoleName)
                .IsUnique()
                .HasDatabaseName("IX_Role_RoleName");
        });
    }

    private static void ConfigureUserRole(ModelBuilder mb)
    {
        mb.Entity<UserRole>(e =>
        {
            e.ToTable("UserRole");
            e.HasKey(ur => ur.UserRoleId);

            // Unique pair — no duplicate role assignments per user
            e.HasIndex(ur => new { ur.UserId, ur.RoleId })
                .IsUnique()
                .HasDatabaseName("UX_UserRole_Pair");

            e.HasIndex(ur => ur.UserId)
                .HasDatabaseName("IX_UserRole_UserId");

            e.HasIndex(ur => ur.RoleId)
                .HasDatabaseName("IX_UserRole_RoleId");

            e.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureAddress(ModelBuilder mb)
    {
        mb.Entity<Address>(e =>
        {
            e.ToTable("Address");
            e.HasKey(a => a.AddressId);

            e.HasIndex(a => new { a.UserId, a.IsDefault })
                .HasFilter("[IsDefault] = 1")
                .HasDatabaseName("IX_Address_IsDefault");

            e.HasIndex(a => a.IsSnapshot)
                .HasDatabaseName("IX_Address_IsSnapshot");

            e.HasIndex(a => a.UserId)
                .HasDatabaseName("IX_Address_UserId");

            // Address.UserId → User.UserId with CASCADE DELETE
            // The reverse circular FK (User.DefaultAddressId) uses NoAction
            e.HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureOTPVerification(ModelBuilder mb)
    {
        mb.Entity<OTPVerification>(e =>
        {
            e.ToTable("OTPVerification");
            e.HasKey(o => o.OTPId);

            e.HasIndex(o => o.Email)
                .HasDatabaseName("IX_OTP_Email");

            e.HasIndex(o => o.ExpiresAt)
                .HasDatabaseName("IX_OTP_ExpiresAt");

            e.HasIndex(o => o.IsUsed)
                .HasDatabaseName("IX_OTP_IsUsed");

            // No FK to User — OTP is created before the user account exists
        });
    }

    private static void ConfigureGuestSession(ModelBuilder mb)
    {
        mb.Entity<GuestSession>(e =>
        {
            e.ToTable("GuestSession");
            e.HasKey(g => g.GuestSessionId);

            e.HasIndex(g => g.SessionToken)
                .IsUnique()
                .HasDatabaseName("UX_GuestSession_Token");

            e.HasIndex(g => g.ExpiresAt)
                .HasDatabaseName("IX_GuestSession_ExpiresAt");

            e.HasIndex(g => g.SessionToken)
                .HasDatabaseName("IX_GuestSession_Token");

            e.HasOne(g => g.ConvertedToUser)
                .WithMany()
                .HasForeignKey(g => g.ConvertedToUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureCategory(ModelBuilder mb)
    {
        mb.Entity<Category>(e =>
        {
            e.ToTable("Category");
            e.HasKey(c => c.CategoryId);

            e.HasIndex(c => c.CategoryCode)
                .IsUnique()
                .HasDatabaseName("UQ_Category_Code");

            e.HasIndex(c => c.IsActive)
                .HasDatabaseName("IX_Category_IsActive");

            e.HasIndex(c => c.DisplayOrder)
                .HasDatabaseName("IX_Category_DisplayOrder");

            e.HasIndex(c => c.ParentCategoryId)
                .HasDatabaseName("IX_Category_ParentCategoryId");

            // Self-referencing hierarchy — no cascade delete to prevent
            // accidental tree destruction
            e.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureBrand(ModelBuilder mb)
    {
        mb.Entity<Brand>(e =>
        {
            e.ToTable("Brand");
            e.HasKey(b => b.BrandId);

            e.HasIndex(b => b.BrandName)
                .IsUnique()
                .HasDatabaseName("UQ_Brand_Name");

            e.HasIndex(b => b.IsActive)
                .HasDatabaseName("IX_Brand_IsActive");
        });
    }

    private static void ConfigureProduct(ModelBuilder mb)
    {
        mb.Entity<Product>(e =>
        {
            e.ToTable("Product");
            e.HasKey(p => p.ProductId);

            e.Property(p => p.Price).HasPrecision(18, 2);

            // Filtered unique SKU — unique when non-NULL
            e.HasIndex(p => p.SKU)
                .IsUnique()
                .HasFilter("[SKU] IS NOT NULL")
                .HasDatabaseName("UX_Product_SKU");

            e.HasIndex(p => p.CategoryId)
                .HasDatabaseName("IX_Product_CategoryId");

            e.HasIndex(p => p.BrandId)
                .HasDatabaseName("IX_Product_BrandId");

            e.HasIndex(p => p.IsActive)
                .HasDatabaseName("IX_Product_IsActive");

            e.HasIndex(p => p.IsFeatured)
                .HasDatabaseName("IX_Product_IsFeatured");

            e.HasIndex(p => p.Name)
                .HasDatabaseName("IX_Product_Name");

            e.HasIndex(p => new { p.Price, p.IsActive })
                .HasDatabaseName("IX_Product_Price");

            e.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureProductVariant(ModelBuilder mb)
    {
        mb.Entity<ProductVariant>(e =>
        {
            e.ToTable("ProductVariant");
            e.HasKey(pv => pv.ProductVariantId);

            e.Property(pv => pv.AdditionalPrice).HasPrecision(18, 2);

            e.HasIndex(pv => pv.ProductId)
                .HasDatabaseName("IX_ProductVariant_ProductId");

            e.HasIndex(pv => pv.IsActive)
                .HasDatabaseName("IX_ProductVariant_IsActive");

            e.HasIndex(pv => pv.ReorderThreshold)
                .HasDatabaseName("IX_ProductVariant_ReorderThreshold");

            e.HasIndex(pv => pv.SKU)
                .HasDatabaseName("IX_ProductVariant_SKU");

            e.HasOne(pv => pv.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureProductImage(ModelBuilder mb)
    {
        mb.Entity<ProductImage>(e =>
        {
            e.ToTable("ProductImage");
            e.HasKey(pi => pi.ProductImageId);

            // Filtered unique index — only one primary image per product
            e.HasIndex(pi => pi.ProductId)
                .IsUnique()
                .HasFilter("[IsPrimary] = 1")
                .HasDatabaseName("UX_ProductImage_Primary");

            e.HasIndex(pi => pi.ProductId)
                .HasDatabaseName("IX_ProductImage_ProductId");

            e.HasIndex(pi => pi.IsPrimary)
                .HasDatabaseName("IX_ProductImage_IsPrimary");

            e.HasIndex(pi => pi.ImageType)
                .HasDatabaseName("IX_ProductImage_ImageType");

            e.HasIndex(pi => pi.DisplayOrder)
                .HasDatabaseName("IX_ProductImage_DisplayOrder");

            e.HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(pi => pi.UploadedBy)
                .WithMany()
                .HasForeignKey(pi => pi.UploadedByUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigurePriceHistory(ModelBuilder mb)
    {
        mb.Entity<PriceHistory>(e =>
        {
            e.ToTable("PriceHistory");
            e.HasKey(ph => ph.PriceHistoryId);

            e.Property(ph => ph.OldPrice).HasPrecision(18, 2);
            e.Property(ph => ph.NewPrice).HasPrecision(18, 2);

            e.HasIndex(ph => new { ph.ProductId, ph.ChangedAt })
                .HasDatabaseName("IX_PriceHistory_ProductId");

            e.HasIndex(ph => ph.ChangedAt)
                .HasDatabaseName("IX_PriceHistory_ChangedAt");

            e.HasOne(ph => ph.Product)
                .WithMany(p => p.PriceHistory)
                .HasForeignKey(ph => ph.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(ph => ph.ChangedBy)
                .WithMany()
                .HasForeignKey(ph => ph.ChangedByUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

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

            // PickupOrderId is NOT auto-generated — it mirrors the parent Order PK.
            // DatabaseGeneratedOption.None tells EF Core not to treat it as identity.
            e.Property(po => po.PickupOrderId)
                .ValueGeneratedNever();

            e.HasIndex(po => po.OrderId)
                .IsUnique()
                .HasDatabaseName("UX_PickupOrder_Order");

            e.HasIndex(po => po.PickupExpiresAt)
                .HasFilter("[PickupExpiresAt] IS NOT NULL")
                .HasDatabaseName("IX_PickupOrder_ExpiresAt");

            e.HasIndex(po => po.OrderId)
                .HasDatabaseName("IX_PickupOrder_OrderId");

            // Shared-PK 1:1 — PickupOrderId = OrderId
            e.HasOne(po => po.Order)
                .WithOne(o => o.PickupOrder)
                .HasForeignKey<PickupOrder>(po => po.PickupOrderId)
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

    private static void ConfigureReview(ModelBuilder mb)
    {
        mb.Entity<Review>(e =>
        {
            e.ToTable("Review");
            e.HasKey(r => r.ReviewId);

            e.HasIndex(r => r.ProductId)
                .HasDatabaseName("IX_Review_ProductId");

            e.HasIndex(r => r.UserId)
                .HasDatabaseName("IX_Review_UserId");

            e.HasIndex(r => r.OrderId)
                .HasDatabaseName("IX_Review_OrderId");

            e.HasIndex(r => r.Rating)
                .HasDatabaseName("IX_Review_Rating");

            e.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(r => r.Product)
                .WithMany()
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(r => r.Order)
                .WithMany(o => o.Reviews)
                .HasForeignKey(r => r.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigurePOSSession(ModelBuilder mb)
    {
        mb.Entity<POS_Session>(e =>
        {
            e.ToTable("POS_Session");
            e.HasKey(p => p.POSSessionId);

            e.Property(p => p.TotalSales).HasPrecision(18, 2);

            e.HasIndex(p => p.UserId)
                .HasDatabaseName("IX_POS_Session_UserId");

            e.HasIndex(p => p.ShiftStart)
                .HasDatabaseName("IX_POS_Session_ShiftStart");

            e.HasIndex(p => p.ShiftEnd)
                .HasDatabaseName("IX_POS_Session_ShiftEnd");

            e.HasOne(p => p.User)
                .WithMany(u => u.POSSessions)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureSupportTicket(ModelBuilder mb)
    {
        mb.Entity<SupportTicket>(e =>
        {
            e.ToTable("SupportTicket");
            e.HasKey(t => t.TicketId);

            e.HasIndex(t => t.UserId)
                .HasDatabaseName("IX_Ticket_UserId");

            e.HasIndex(t => t.TicketStatus)
                .HasDatabaseName("IX_Ticket_Status");

            e.HasIndex(t => t.TicketCategory)
                .HasDatabaseName("IX_Ticket_Category");

            e.HasIndex(t => t.CreatedAt)
                .HasDatabaseName("IX_Ticket_CreatedAt");

            e.HasIndex(t => t.OrderId)
                .HasFilter("[OrderId] IS NOT NULL")
                .HasDatabaseName("IX_Ticket_OrderId");

            e.HasIndex(t => t.AssignedToUserId)
                .HasFilter("[AssignedToUserId] IS NOT NULL")
                .HasDatabaseName("IX_Ticket_AssignedTo");

            // Two separate FK relationships to User — must be configured
            // explicitly to avoid EF Core ambiguity
            e.HasOne(t => t.User)
                .WithMany(u => u.SupportTickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(t => t.Order)
                .WithMany(o => o.SupportTickets)
                .HasForeignKey(t => t.OrderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureSupportTicketReply(ModelBuilder mb)
    {
        mb.Entity<SupportTicketReply>(e =>
        {
            e.ToTable("SupportTicketReply");
            e.HasKey(r => r.ReplyId);

            e.HasIndex(r => r.TicketId)
                .HasDatabaseName("IX_Reply_TicketId");

            e.HasIndex(r => r.UserId)
                .HasDatabaseName("IX_Reply_UserId");

            e.HasIndex(r => r.CreatedAt)
                .HasDatabaseName("IX_Reply_CreatedAt");

            e.HasOne(r => r.Ticket)
                .WithMany(t => t.Replies)
                .HasForeignKey(r => r.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureSupportTask(ModelBuilder mb)
    {
        mb.Entity<SupportTask>(e =>
        {
            e.ToTable("SupportTask");
            e.HasKey(t => t.TaskId);

            e.HasIndex(t => t.TicketId)
                .HasDatabaseName("IX_SupportTask_TicketId");

            e.HasIndex(t => t.TaskStatus)
                .HasDatabaseName("IX_SupportTask_TaskStatus");

            e.HasIndex(t => t.AssignedToUserId)
                .HasFilter("[AssignedToUserId] IS NOT NULL")
                .HasDatabaseName("IX_SupportTask_AssignedToUserId");

            e.HasIndex(t => t.DueDate)
                .HasFilter("[DueDate] IS NOT NULL")
                .HasDatabaseName("IX_SupportTask_DueDate");

            e.HasOne(t => t.Ticket)
                .WithMany(st => st.Tasks)
                .HasForeignKey(t => t.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureNotification(ModelBuilder mb)
    {
        mb.Entity<Notification>(e =>
        {
            e.ToTable("Notification");
            e.HasKey(n => n.NotificationId);

            e.HasIndex(n => n.UserId)
                .HasDatabaseName("IX_Notif_UserId");

            e.HasIndex(n => n.Status)
                .HasDatabaseName("IX_Notif_Status");

            e.HasIndex(n => new { n.Status, n.CreatedAt })
                .HasFilter("[Status] = 'Pending'")
                .HasDatabaseName("IX_Notif_Pending");

            e.HasIndex(n => n.Channel)
                .HasDatabaseName("IX_Notif_Channel");

            e.HasIndex(n => n.CreatedAt)
                .HasDatabaseName("IX_Notif_CreatedAt");

            e.HasIndex(n => n.OrderId)
                .HasFilter("[OrderId] IS NOT NULL")
                .HasDatabaseName("IX_Notif_OrderId");

            e.HasIndex(n => n.TicketId)
                .HasFilter("[TicketId] IS NOT NULL")
                .HasDatabaseName("IX_Notif_TicketId");

            e.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(n => n.Order)
                .WithMany(o => o.Notifications)
                .HasForeignKey(n => n.OrderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(n => n.Ticket)
                .WithMany(t => t.Notifications)
                .HasForeignKey(n => n.TicketId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureSystemLog(ModelBuilder mb)
    {
        mb.Entity<SystemLog>(e =>
        {
            e.ToTable("SystemLog");
            e.HasKey(s => s.SystemLogId);

            e.HasIndex(s => s.UserId)
                .HasDatabaseName("IX_SystemLog_UserId");

            e.HasIndex(s => s.EventType)
                .HasDatabaseName("IX_SystemLog_EventType");

            e.HasIndex(s => s.CreatedAt)
                .HasDatabaseName("IX_SystemLog_CreatedAt");

            e.HasOne(s => s.User)
                .WithMany(u => u.SystemLogs)
                .HasForeignKey(s => s.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}