
using EZcommerce.Web.Models;
using EZcommerce.Web.Models.ViewModels;

namespace EZcommerce.Web.Repositories;
public interface IEZcommerceRepository
{
    Task<bool> ProductAnyAsync(int id);
    Task<Product?> ProductGetByIdAsync(int id);
    Task<Product?> ProductGetByIdWithInventoryAsync(int id);

    Task<List<Order>> OrderGetAllAsync();
    Task<Order?> OrderGetByIdAsync(int id);
    Task<Order?> OrderGetByIdNoTrackingAsync(int id);
    Task<bool> OrderAnyAsync(int id);
    Task OrderAddAndSaveAsync(Order order);
    Task OrderRemoveAndSaveAsync(Order order);

    Task<Inventory?> InventoryGetByProductIdAsync(int id);

    Task<List<OrderItem>> OrderItemGetByOrderIdWithProductAndInventoryAsync(int orderId);

    Task SaveChangesAsync();

    // Consider Seperating Product/Order/Payment into seperate services
    /*
    Task<List<Product>> ProductGetAllAsync();
    Task<List<Product>> ProductGetAllWithInventoryAsync();
    Task<Product?> ProductGetByIdWithInventoryAndCategoryAsync(int id);
    Task ProductAddAndSave(Product product);
    Task ProductUpdateAndSave(ProductCreateViewModel model);
    void ProductRemoveAndSave(int id);

    
    void OrderRemoveAndSaveAsync(int id);
    void OrderUpdate(Order order);
    Task OrderUpdateAsync(OrderViewModel model);

    Task<List<Payment>> PaymentGetAllAsync();
    Task<Payment?> PaymentGetByIdAsync();
    Task PaymentEditAndSave(Payment payment);
    void PaymentAddAndSave(Payment payment);

    Task<List<Category>> CategoryGetAllAsync();


    */
}