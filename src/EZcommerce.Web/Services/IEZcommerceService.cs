
using EZcommerce.Web.Models;
using EZcommerce.Web.Models.Session;

namespace EZcommerce.Web.Services;

public interface IEZcommerceService
{
    void ValidateCart(List<CartItem> items);

    Task<int> InitiateOrderFromCartItems(List<CartItem> items);

    Task LowerInventoriesByCartItems(List<CartItem> items);

    Task OrderInventoryRollback(int orderId);

    void OrderRemove(int orderId);

    void OrderUpdate(int orderId, Order orderChanges);

    void PaymentCreate(Payment payment);

    Task<List<Product>> ProductGetAllIncludeInventoryAsync();

    Task<Product?> ProductGetWithInventoryAsync(int id);

    Task ProductCreateWithInventory(ProductCreateViewModel model);

    Task ProductEditWithInventory(ProductCreateViewModel model);
    
    void ProductRemove(int id);
    
    Task<List<Category>> CategoryGetAllAsync();


}