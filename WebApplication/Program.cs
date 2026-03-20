using Microsoft.EntityFrameworkCore;
using WebApplication.BackgroundJobs;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.BusinessLogic.Services;
using WebApplication.DataAccess.Context;
using WebApplication.DataAccess.Repositories;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// ── Database ──────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Session (in-memory; swap to distributed cache for multi-instance) ─────────
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout        = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly    = true;
    options.Cookie.IsEssential = true;
});

// ── Repositories ─────────────────────────────────────────────────────────────
builder.Services.AddScoped<IUserRepository,    UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository,    CartRepository>();
builder.Services.AddScoped<IOrderRepository,   OrderRepository>();

// ── Services ─────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IUserService,      UserService>();
builder.Services.AddScoped<IProductService,   ProductService>();
builder.Services.AddScoped<ICartService,      CartService>();
builder.Services.AddScoped<IOrderService,     OrderService>();
builder.Services.AddScoped<IPaymentService,   PaymentService>();
builder.Services.AddScoped<IDeliveryService,  DeliveryService>();
builder.Services.AddScoped<IReviewService,    ReviewService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ISupportService,   SupportService>();

// ── Background Jobs ───────────────────────────────────────────────────────────
builder.Services.AddHostedService<PendingOrderMonitorJob>();
builder.Services.AddHostedService<PaymentTimeoutJob>();
builder.Services.AddHostedService<StockMonitorJob>();

// ── MVC ───────────────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ── HTTP pipeline ─────────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();          // must come before UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Urls.Add("http://0.0.0.0:5064");
app.Urls.Add("https://0.0.0.0:7177");

app.Run();