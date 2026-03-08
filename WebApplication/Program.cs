using Microsoft.AspNetCore.Builder;      // For WebApplication
using Microsoft.Extensions.Hosting;       // For IHostEnvironment
using Microsoft.Extensions.DependencyInjection; // For AddControllersWithViews

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); 
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   // Needed to serve wwwroot/css and js
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Urls.Add("http://0.0.0.0:5064"); // Allows all devices on LAN to connect
app.Urls.Add("https://0.0.0.0:7177"); // optional for PC only

app.Run();