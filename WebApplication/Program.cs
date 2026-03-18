// WebApplication/Program.cs

using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.BusinessLogic.Services;
using WebApplication.DataAccess.Context;
using WebApplication.DataAccess.Repositories;
using WebApplication.Utilities;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// =============================================================================
// DATABASE
// =============================================================================

builder.Services.AddDbContext<AppDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("TaurusBikeShopDB")
        ?? throw new InvalidOperationException(
            "Connection string 'TaurusBikeShopDB' not found in configuration. " +
            "Ensure appsettings.Development.json is present and excluded from source control.");

    options.UseSqlServer(connectionString, sqlOptions =>
    {
        // Retry on transient failures — Google Cloud SQL connections can briefly drop
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);
    });
});

// =============================================================================
// AUTHENTICATION & SESSION
// =============================================================================

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    int idleTimeoutMinutes = builder.Configuration
        .GetValue<int>("Session:IdleTimeoutMinutes", 30);

    options.IdleTimeout = TimeSpan.FromMinutes(idleTimeoutMinutes);
    options.Cookie.Name = builder.Configuration
        .GetValue<string>("Session:Cookie:Name") ?? ".TaurusBikeShop.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.None
        : CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = builder.Configuration
            .GetValue<string>("Authentication:Cookie:Name") ?? ".TaurusBikeShop.Auth";
        options.LoginPath = builder.Configuration
            .GetValue<string>("Authentication:Cookie:LoginPath") ?? "/Customer/Login";
        options.LogoutPath = builder.Configuration
            .GetValue<string>("Authentication:Cookie:LogoutPath") ?? "/Customer/Logout";
        options.AccessDeniedPath = builder.Configuration
            .GetValue<string>("Authentication:Cookie:AccessDeniedPath") ?? "/Customer/Login";

        string expireValue = builder.Configuration
            .GetValue<string>("Authentication:Cookie:ExpireTimeSpan") ?? "01:00:00";
        options.ExpireTimeSpan = TimeSpan.Parse(expireValue);

        options.SlidingExpiration = builder.Configuration
            .GetValue<bool>("Authentication:Cookie:SlidingExpiration", true);

        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.None
            : CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

builder.Services.AddAuthorization();

// =============================================================================
// GOOGLE CLOUD STORAGE
// =============================================================================

// StorageClient uses Application Default Credentials (ADC).
// On Google Cloud infrastructure, ADC is resolved automatically.
// For local development, set GOOGLE_APPLICATION_CREDENTIALS environment variable
// to point to a service account JSON key file.
builder.Services.AddSingleton<StorageClient>(_ => StorageClient.Create());

builder.Services.AddScoped<FileUploadHelper>(provider =>
{
    StorageClient storageClient = provider.GetRequiredService<StorageClient>();
    string bucketName = builder.Configuration
        .GetValue<string>("GoogleCloudStorage:BucketName")
        ?? throw new InvalidOperationException(
            "GoogleCloudStorage:BucketName is not configured in appsettings.");
    return new FileUploadHelper(storageClient, bucketName);
});

// =============================================================================
// REPOSITORIES
// =============================================================================

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<BrandRepository>();
builder.Services.AddScoped<CartRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ReviewRepository>();
builder.Services.AddScoped<SupplierRepository>();
builder.Services.AddScoped<SupportRepository>();
builder.Services.AddScoped<VoucherRepository>();
builder.Services.AddScoped<WishlistRepository>();

// =============================================================================
// SERVICES
// =============================================================================

builder.Services.AddScoped<INotificationService, NotificationService>();

// NOTE: The following service registrations are added as each step is implemented.
// Uncomment each line when the corresponding service class is created:
//
// Step 11 — Authentication
builder.Services.AddScoped<IUserService, UserService>();
//
// Step 12 — Product Catalog
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBrandService, BrandService>();
//
// Step 13 — Cart
builder.Services.AddScoped<ICartService, CartService>();
//
// Step 14 — Wishlist
builder.Services.AddScoped<IWishlistService, WishlistService>();
//
// Step 15 — Vouchers
builder.Services.AddScoped<IVoucherService, VoucherService>();
//
// Step 16 — Checkout & Orders
builder.Services.AddScoped<IOrderService, OrderService>();
//
// Step 17 — Payments
builder.Services.AddScoped<IPaymentService, PaymentService>();
//
// Step 19 — Reviews
builder.Services.AddScoped<IReviewService, ReviewService>();
//
// Step 20 — Support Tickets
builder.Services.AddScoped<ISupportService, SupportService>();
//
// Step 22 — Background Jobs
builder.Services.AddHostedService<WebApplication.BackgroundJobs.InventorySyncJob>();
builder.Services.AddHostedService<WebApplication.BackgroundJobs.PendingOrderMonitorJob>();
builder.Services.AddHostedService<WebApplication.BackgroundJobs.PaymentTimeoutJob>();
builder.Services.AddHostedService<WebApplication.BackgroundJobs.StockMonitorJob>();
builder.Services.AddHostedService<WebApplication.BackgroundJobs.DeliveryStatusPollJob>();

// =============================================================================
// MVC
// =============================================================================

builder.Services.AddControllersWithViews(options =>
{
    // Global anti-forgery validation filter — applies [ValidateAntiForgeryToken]
    // to all POST actions automatically. Individual actions can opt out with
    // [IgnoreAntiforgeryToken] when needed (e.g. webhook callbacks).
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());
});

// =============================================================================
// BUILD
// =============================================================================

WebApplication app = builder.Build();

// =============================================================================
// MIDDLEWARE PIPELINE
// =============================================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // HSTS: tell browsers to only connect via HTTPS for 1 year
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// =============================================================================
// ROUTES
// =============================================================================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// =============================================================================
// RUN
// =============================================================================

app.Run();