using Microsoft.EntityFrameworkCore;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User — map to [User] table (reserved keyword in SQL Server)
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.HasKey(u => u.UserId);
                e.Property(u => u.Email).HasMaxLength(255);
                e.Property(u => u.PasswordHash).HasMaxLength(255);
                e.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
                e.Property(u => u.LastName).HasMaxLength(100).IsRequired();
                e.HasIndex(u => u.Email).IsUnique();
            });

            // Category
            modelBuilder.Entity<Category>(e =>
            {
                e.HasKey(c => c.CategoryId);
                e.Property(c => c.CategoryCode).HasMaxLength(20).IsRequired();
                e.Property(c => c.Name).HasMaxLength(100).IsRequired();
                e.HasIndex(c => c.CategoryCode).IsUnique();
                e.HasOne(c => c.ParentCategory)
                 .WithMany(c => c.SubCategories)
                 .HasForeignKey(c => c.ParentCategoryId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Product
            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(p => p.ProductId);
                e.Property(p => p.Name).HasMaxLength(200).IsRequired();
                e.Property(p => p.Price).HasColumnType("decimal(10,2)");
                e.Property(p => p.Currency).HasMaxLength(3).HasDefaultValue("PHP");
                e.HasOne(p => p.Category)
                 .WithMany(c => c.Products)
                 .HasForeignKey(p => p.CategoryId);
            });

            // CartItem — stored per-user in session-backed table
            modelBuilder.Entity<CartItem>(e =>
            {
                e.HasKey(ci => ci.CartItemId);
                e.Property(ci => ci.PriceAtAdd).HasColumnType("decimal(10,2)");
                e.HasOne(ci => ci.User)
                 .WithMany()
                 .HasForeignKey(ci => ci.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(ci => ci.Product)
                 .WithMany(p => p.CartItems)
                 .HasForeignKey(ci => ci.ProductId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Order — [Order] is reserved
            modelBuilder.Entity<Order>(e =>
            {
                e.ToTable("Order");
                e.HasKey(o => o.OrderId);
                e.Property(o => o.OrderNumber).HasMaxLength(50).IsRequired();
                e.Property(o => o.SubTotal).HasColumnType("decimal(10,2)");
                e.Property(o => o.DiscountAmount).HasColumnType("decimal(10,2)");
                e.Property(o => o.ShippingFee).HasColumnType("decimal(10,2)");
                e.Property(o => o.TotalAmount).HasColumnType("decimal(10,2)");
                e.HasIndex(o => o.OrderNumber).IsUnique();
                e.HasOne(o => o.User)
                 .WithMany(u => u.Orders)
                 .HasForeignKey(o => o.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // OrderItem
            modelBuilder.Entity<OrderItem>(e =>
            {
                e.HasKey(oi => oi.OrderItemId);
                e.Property(oi => oi.UnitPrice).HasColumnType("decimal(10,2)");
                e.Property(oi => oi.Subtotal).HasColumnType("decimal(10,2)");
                e.HasOne(oi => oi.Order)
                 .WithMany(o => o.OrderItems)
                 .HasForeignKey(oi => oi.OrderId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(oi => oi.Product)
                 .WithMany(p => p.OrderItems)
                 .HasForeignKey(oi => oi.ProductId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Payment
            modelBuilder.Entity<Payment>(e =>
            {
                e.HasKey(p => p.PaymentId);
                e.Property(p => p.Amount).HasColumnType("decimal(10,2)");
                e.HasOne(p => p.Order)
                 .WithMany(o => o.Payments)
                 .HasForeignKey(p => p.OrderId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Delivery
            modelBuilder.Entity<Delivery>(e =>
            {
                e.HasKey(d => d.DeliveryId);
                e.HasOne(d => d.Order)
                 .WithMany(o => o.Deliveries)
                 .HasForeignKey(d => d.OrderId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Review
            modelBuilder.Entity<Review>(e =>
            {
                e.HasKey(r => r.ReviewId);
                e.HasOne(r => r.User)
                 .WithMany(u => u.Reviews)
                 .HasForeignKey(r => r.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(r => r.Product)
                 .WithMany(p => p.Reviews)
                 .HasForeignKey(r => r.ProductId)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(r => r.Order)
                 .WithMany(o => o.Reviews)
                 .HasForeignKey(r => r.OrderId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
