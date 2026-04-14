
using EZcommerce.Web.Models;

namespace EZcommerce.Web.Repositories;
public interface IEZcommerceRepository
{
    Task<List<Product>> GetProductsAsync();

    Task<Product?> GetProductbyIdAsync(int id);
}