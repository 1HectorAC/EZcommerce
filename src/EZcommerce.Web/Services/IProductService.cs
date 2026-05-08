
using EZcommerce.Web.Models;

namespace EZcommerce.Web.Services;

public interface IProductService
{
    Task<List<Product>> GetAllWithInventoryAndCategoryAsync();
    Task<Product?> GetByIdWithInventoryAndCategoryAsync(int id);
    Task AddWithInventoryAsync(ProductCreateViewModel model);
    Task UpdateWithInventoryAsync(ProductCreateViewModel model);
    Task RemoveAsync(int id);
}