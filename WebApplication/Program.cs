// WebApplication/Program.cs

using Google.Cloud.Storage.V1;
using AppWebApplication = Microsoft.AspNetCore.Builder.WebApplication;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.BusinessLogic.Services;
using WebApplication.DataAccess.Context;
using WebApplication.DataAccess.Repositories;
using WebApplication.Utilities;

WebApplicationBuilder builder = AppWebApplication.CreateBuilder(args);

builder.Services.Configure<HostOptions>(options =>
    options.BackgroundServiceExceptionBehavior =
        BackgroundServiceExceptionBehavior.Ignore);

builder.Services.Configure<CloudinarySettings>(
builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.Configure<WebApplication.Models.SmtpSettings>(
    builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IEmailSender, GmailEmailSender>();

// =============================================================================
// DATABASE
// =============================================================================

builder.Services.AddDbContext<AppDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("TaurusBikeShopSqlServer2026")
        ?? throw new InvalidOperationException(
            "Connection string 'TaurusBikeShopSqlServer2026' not found in configuration. " +
            "Ensure appsettings.Development.json is present and excluded from source control.");

    options.UseSqlServer(connectionString, sqlOptions =>
    {
        // Retry on transient failures — Google Cloud SQL connections can briefly drop
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);

        // Use split queries by default for multi-collection Includes to avoid
        // the CartesianExplosion / MultipleCollectionIncludeWarning.
        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
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
// If credentials are missing, the app still starts — uploads will fail at runtime
// with a clear error instead of crashing the entire application on startup.

StorageClient? gcsClient = null;
try
{
    gcsClient = StorageClient.Create();
}
catch (Exception ex) when (builder.Environment.IsDevelopment())
{
    // GCS credentials not configured locally — log and continue.
    // File upload features (payment proofs, support attachments, product images)
    // will be unavailable until GOOGLE_APPLICATION_CREDENTIALS is set.
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"[WARNING] Google Cloud Storage unavailable: {ex.Message}");
    Console.WriteLine("[WARNING] File upload features will be disabled. Set GOOGLE_APPLICATION_CREDENTIALS to enable.");
    Console.ResetColor();
}

if (gcsClient is not null)
{
    builder.Services.AddSingleton(gcsClient);

    builder.Services.AddScoped<FileUploadHelper>(provider =>
    {
        StorageClient storageClient = provider.GetRequiredService<StorageClient>();
        string bucketName = builder.Configuration
            .GetValue<string>("GoogleCloudStorage:BucketName")
            ?? throw new InvalidOperationException(
                "GoogleCloudStorage:BucketName is not configured in appsettings.");
        return new FileUploadHelper(storageClient, bucketName);
    });
}
else
{
    // Register a null placeholder so DI doesn't throw when controllers
    // that depend on FileUploadHelper are resolved. The helper itself is
    // only called during actual file upload actions.
    builder.Services.AddScoped<FileUploadHelper>(_ =>
        null!);
}

// =============================================================================
// REPOSITORIES
// =============================================================================

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<BrandRepository>();
builder.Services.AddScoped<CartRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<StorePaymentAccountRepository>();
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
builder.Services.AddHostedService<WebApplication.BackgroundJobs.NotificationDispatchJob>();

// =============================================================================
// CORS
// =============================================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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

AppWebApplication app = builder.Build();

// =============================================================================
// RENDER PORT BINDING
// =============================================================================
// Render sets a PORT environment variable. Bind to 0.0.0.0:{PORT} so the app
// is reachable outside the container. Falls back to default Kestrel behavior
// when PORT is not set (local development).

string? port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    app.Urls.Add($"http://0.0.0.0:{port}");
}

// =============================================================================
// MIDDLEWARE PIPELINE
// =============================================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // HSTS: tell browsers to only connect via HTTPS for 1 year
    app.UseHsts();
}

// Render terminates TLS at its reverse proxy, so the app receives plain HTTP.
// HTTPS redirection in-app would cause redirect loops. Only enable locally.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Antiforgery token endpoint — serves a token for AJAX calls on pages that
// have no visible <form> (site.js fetches /antiforgery/token on load).
app.MapGet("/antiforgery/token", (IAntiforgery antiforgery, HttpContext context) =>
{
    AntiforgeryTokenSet tokens = antiforgery.GetAndStoreTokens(context);
    return Results.Ok(new { token = tokens.RequestToken });
});

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