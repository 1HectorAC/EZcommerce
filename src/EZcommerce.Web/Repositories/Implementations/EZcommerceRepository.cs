
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
    public async Task<List<Product>> GetProductsAsync()
    {
        return await _context.Products
        .AsNoTracking()
        .ToListAsync();
    }

    public async Task<Product?> GetProductbyIdAsync(int id)
    {
        return await _context.Products
        .AsNoTracking()
        .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Product?> GetProductbyIdWithInventoryAndCategoryAsync(int id)
    {
        return await _context.Products
        .AsNoTracking()
        .Include(i => i.Inventory)
        .Include(i => i.Category)
        .FirstOrDefaultAsync(i => i.Id == id);
    }

}