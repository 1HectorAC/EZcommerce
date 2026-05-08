
using DotNetEnv;
using Ezcommerce.Web.data;
using EZcommerce.Models.Settings;
using EZcommerce.Web.Data;
using EZcommerce.Web.Repositories;
using EZcommerce.Web.Repositories.Implementations;
using EZcommerce.Web.Services;
using EZcommerce.Web.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("AppIdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AppIdentityDbContextConnection' not found.");;

Env.Load();
builder.Configuration.AddEnvironmentVariables();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("STRIPE"));

// Add services to the container.
builder.Services.AddControllersWithViews();

//Don't need unless running on multiple servers, for sessions
//builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddDbContext<AppIdentityDbContext>(
    options => options.UseSqlServer(Environment.GetEnvironmentVariable("DB_Connection")));
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>();
    

builder.Services.AddDbContext<EZcommerceDbContext>(
    options => options.UseSqlServer(Environment.GetEnvironmentVariable("DB_Connection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<EZcommerce.Web.Services.CheckoutService>();
builder.Services.AddScoped<IEZcommerceService, EZcommerceService>();
builder.Services.AddScoped<IProductService, EZcommerce.Web.Services.Implementations.ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();


var app = builder.Build();

// Handle initital admin user creation if none exits
using(var scope = app.Services.CreateScope())
{
    var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? throw new Exception("No AdminEmail env. var. provided");
    var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? throw new Exception("No AdminPassword env. var. provided");
    var adminRole = "Admin";
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
    // Check if admin role exits and create
    if(!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }

    if(await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var user = new IdentityUser {UserName=adminEmail, Email=adminEmail};
        await userManager.CreateAsync(user, adminPassword);
        await userManager.AddToRoleAsync(user, adminRole);
    }
}

// Setup global Stripe Api setting, 
//resolve error when calling ChargeService and not finding secret key
var stripeOptions = app.Services.GetRequiredService<IOptions<StripeSettings>>();
StripeConfiguration.ApiKey = stripeOptions.Value.SecretKey;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Before routing and Endpoints.
app.UseSession();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
  

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();
app.Run();
