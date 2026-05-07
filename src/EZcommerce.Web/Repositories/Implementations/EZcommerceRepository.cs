
using EZcommerce.Web.Data;
using EZcommerce.Web.Models;
using Microsoft.EntityFrameworkCore;
namespace EZcommerce.Web.Repositories.Implementations;

public class EZcommerceRepository: IEZcommerceRepository
{
    private readonly EZcommerceDbContext _context;

    public EZcommerceRepository(EZcommerceDbContext context)
    {
        _context = context;
    }
    // Maybe delete later

    /*
    public async Task<List<Product>> ProductGetAllAsync()
    {
        return await _context.Products.AsNoTracking().ToListAsync();
    }
    public async Task<List<Product>> ProductGetAllWithInventoryAsync()
    {
                return await _context.Products.AsNoTracking().Include(i => i.Inventory).ToListAsync();
    }
    public async Task<bool> ProductAnyAsync(int id)
    {
        return await _context.Products.AsNoTracking().AnyAsync(i => i.Id == id);
    }
    public async Task<Product?> ProductGetByIdAsync(int id)
    {
        return await _context.Products.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
    }
    public async Task<Product?> ProductGetByIdWithTrackingAsync(int id)
    {
                return await _context.Products.FirstOrDefaultAsync(i => i.Id == id);

    }
    public async Task<Product?> ProductGetByIdWithInventoryWithTrackingAsync(int id)
    {
        return await _context.Products.Include(i => i.Inventory).FirstOrDefaultAsync(i => i.Id == id);

    }
    public async Task<Product?> ProductGetByIdWithInventoryAsync(int id)
    {
        return await _context.Products.AsNoTracking().Include(i => i.Inventory).FirstOrDefaultAsync(i => i.Id == id);
    }
    public async Task<Product?> ProductGetByIdWithInventoryAndCategoryAsync(int id)
    {
        return await _context.Products.AsNoTracking().Include(i => i.Inventory).Include(i => i.Category).FirstOrDefaultAsync(i => i.Id == id);
    }
    public async Task ProductAddAndSaveAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }
    public async Task ProductRemoveAndSaveAsync(Product product)
    {
         _context.Products.Remove(product);
         await _context.SaveChangesAsync();
    }



    public async Task<List<Order>> OrderGetAllAsync()
    {
        return await _context.Orders.AsNoTracking().ToListAsync();
    }
    public async Task<Order?> OrderGetByIdAsync(int id)
    {
        return await _context.Orders.AsNoTracking().FirstOrDefaultAsync( i => i.Id == id);
    }
    public async Task<Order?> OrderGetByIdWithTrackingAsync(int id)
    {
        return await _context.Orders.FirstOrDefaultAsync( i => i.Id == id);
    }
    public async Task<bool> OrderAnyAsync(int id)
    {
        return await _context.Orders.AsNoTracking().AnyAsync(i => i.Id == id);
    }
    public async Task OrderAddAndSaveAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
    public async Task OrderRemoveAndSaveAsync(Order order)
    {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }


    public async Task<List<Payment>> PaymentGetAllAsync()
    {
        return await _context.Payments.AsNoTracking().ToListAsync();
    }
    public async Task<Payment?> PaymentGetByIdAsync(int id)
    {
        return await _context.Payments.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
    }
    public async Task<Payment?> PaymentGetByIdWithTrackingAsync(int id)
    {
                return await _context.Payments.FirstOrDefaultAsync(i => i.Id == id);

    }
    public async Task PaymentAddAndSaveAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }
    public async Task PaymentRemoveAndSaveAsync(Payment payment)
    {
        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync();
    }

        public async Task<List<Category>> CategoryGetAllAsync()
    {
        return await _context.Categories.AsNoTracking().ToListAsync();
    }


    public async Task<Inventory?> InventoryGetByProductIdAsync(int id)
    {
        return await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == id);
    }

    public async Task<List<OrderItem>> OrderItemGetByOrderIdWithProductAndInventoryAsync(int orderId)
    {
        return await _context.OrderItems.Include(i => i.Product).ThenInclude(j => j!.Inventory).Where(i => i.OrderId == orderId).ToListAsync();
    }


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    */




}