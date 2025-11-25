using Microsoft.AspNetCore.Authentication.Cookies;
using MongoDB.Driver;
using RestaurantApp.Models;
using RestaurantApp.Services;

var builder = WebApplication.CreateBuilder(args);

// MongoSettings
var mongoSettings = builder.Configuration.GetSection("MongoSettings").Get<MongoSettings>();
if (mongoSettings == null)
    throw new InvalidOperationException("MongoSettings section is missing in appsettings.json");

// MongoClient singleton
var mongoClient = new MongoClient(mongoSettings.ConnectionString);
builder.Services.AddSingleton<IMongoClient>(mongoClient);

// MongoService singleton
builder.Services.AddSingleton<MongoService>();

// Scoped services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<OrderService>();

// Controllers with views
builder.Services.AddControllersWithViews();

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
