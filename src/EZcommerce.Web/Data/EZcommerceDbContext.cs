
using EZcommerce.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EZcommerce.Web.Data;

public class EZcommerceDbContext: DbContext
{
    public EZcommerceDbContext(DbContextOptions<EZcommerceDbContext> options): base(options) {}


    public DbSet<Category> Categories {get; set;}

    public DbSet<Product> Products {get; set;}

    public DbSet<Inventory> Inventories {get; set;}

    public DbSet<Order> Orders {get; set;}

    public DbSet<OrderItem> OrderItems {get; set;}
    
    public DbSet<Payment> Payments {get; set;}

}