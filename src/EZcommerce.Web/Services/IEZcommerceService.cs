
using EZcommerce.Web.Models;
using EZcommerce.Web.Models.Session;
using EZcommerce.Web.Models.ViewModels;

namespace EZcommerce.Web.Services;

public interface IEZcommerceService
{
    // Consider Seperating Product/Order/Payment into seperate services

    Task ValidateCart(List<CartItem> items);

    Task<int> InitiateOrderFromCartItems(List<CartItem> items);

    Task LowerInventoriesByCartItems(List<CartItem> items);

    Task<List<Product>> ProductGetAllWithInventoryAndCategoryAsync();
    Task<Product?> ProductGetByIdWithInventoryAndCategoryAsync(int id);
    Task ProductAndInventoryAddAsync(ProductCreateViewModel model);
    Task ProductAndInventoryUpdateAsync(ProductCreateViewModel model);
    Task ProductRemoveAsync(int id);

    Task<List<Order>> OrderGetAllAsync();
    Task<Order?> OrderGetByIdAsync(int id);
    Task OrderInventoryRollbackAsync(int orderId);
    Task OrderUpdateAsync(Order orderChanges);
    Task OrderUpdateAsync(OrderViewModel model);
    Task OrderRemoveAsync(int orderId);

    Task<List<Payment>> PaymentGetAllAsync();
    Task<Payment?> PaymentGetByIdAsync(int id);
    Task PaymentAddAsync(Payment payment);
    Task PaymentUpdateAsync(Payment payment);
    Task PaymentRemoveAsync(int id);

    Task<List<Category>> CategoryGetAllAsync();
}