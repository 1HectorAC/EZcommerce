
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

    public async Task<bool> ProductAnyAsync(int id)
    {
        return await _context.Products.AsNoTracking().AnyAsync(i => i.Id == id);
    }

    public async Task<Product?> ProductGetByIdAsync(int id)
    {
        return await _context.Products.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Product?> ProductGetByIdWithInventoryAsync(int id)
    {
        return await _context.Products.AsNoTracking().Include(i => i.Inventory).FirstOrDefaultAsync(i => i.Id == id);
    }


    public async Task<List<Order>> OrderGetAllAsync()
    {
        return await _context.Orders.AsNoTracking().ToListAsync();
    }
    public async Task<Order?> OrderGetByIdAsync(int id)
    {
        return await _context.Orders.AsNoTracking().FirstOrDefaultAsync( i => i.Id == id);
    }
    public async Task<Order?> OrderGetByIdNoTrackingAsync(int id)
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

    public async Task<Inventory?> InventoryGetByProductIdAsync(int id)
    {
        return await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == id);
    }

    public async Task<List<OrderItem>> OrderItemGetByOrderIdWithProductAndInventoryAsync(int orderId)
    {
        return await _context.OrderItems.Include(i => i.Product).ThenInclude(j => j.Inventory).Where(i => i.OrderId == orderId).ToListAsync();
    }


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }


}