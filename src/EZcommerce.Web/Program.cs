
using DotNetEnv;
using EZcommerce.Web.Data;
using EZcommerce.Web.Repositories;
using EZcommerce.Web.Repositories.Implementations;
using EZcommerce.Web.Services;
using EZcommerce.Web.Services.Implementations;
using Microsoft.EntityFrameworkCore;




var builder = WebApplication.CreateBuilder(args);

Env.Load();



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

builder.Services.AddDbContext<EZcommerceDbContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("DB_Connection")));
builder.Services.AddScoped<IEZcommerceRepository, EZcommerceRepository>();


var app = builder.Build();

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
