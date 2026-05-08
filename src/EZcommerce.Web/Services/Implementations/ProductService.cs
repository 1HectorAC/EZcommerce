
using EZcommerce.Web.Models;
using EZcommerce.Web.Repositories;
using EZcommerce.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace EZcommerce.Web.Services.Implementations;
public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _productRepo;

    public ProductService(IGenericRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }
    public async Task<List<Product>> GetAllWithInventoryAndCategoryAsync()
    {
        return await _productRepo.Query()
            .AsNoTracking()
            .Include(i => i.Inventory)
            .Include(i => i.Category)
            .ToListAsync();
    }
    public async Task<Product?> GetByIdWithInventoryAndCategoryAsync(int id)
    {
        return await _productRepo.Query()
            .AsNoTracking()
            .Include(i => i.Inventory)
            .Include(i => i.Category)
            .FirstOrDefaultAsync(i => i.Id == id);
    }
    public async Task AddWithInventoryAsync(ProductCreateViewModel model)
    {
        var product = new Product
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            ImageUrl = model.ImageUrl,
            CategoryId = model.CategoryId,
            Created_at = DateTime.UtcNow,
            Inventory = new Inventory { Quantity = model.InventoryQuantity }
        };
        await _productRepo.AddAsync(product);
        await _productRepo.SaveChangesAsync();
    }
    public async Task UpdateWithInventoryAsync(ProductCreateViewModel model)
    {
        var product = await _productRepo.Query()
            .Include(i => i.Inventory)
            .FirstOrDefaultAsync(i => i.Id == model.Id);

        if (product is null)
            throw new Exception();

        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.ImageUrl = model.ImageUrl;
        product.CategoryId = model.CategoryId;
        product.Inventory!.Quantity = model.InventoryQuantity;

        await _productRepo.SaveChangesAsync();
    }
    public async Task RemoveAsync(int id)
    {
        var product = await _productRepo.GetByIdAsync(id) ?? throw new Exception();

        _productRepo.Remove(product);
        await _productRepo.SaveChangesAsync();
    }
}