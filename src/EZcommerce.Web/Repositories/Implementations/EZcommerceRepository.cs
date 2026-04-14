
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
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetProductbyIdAsync(int id)
    {
        return await _context.Products.FirstOrDefaultAsync(i => i.Id == id);
    }
}