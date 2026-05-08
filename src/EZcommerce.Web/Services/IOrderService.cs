using EZcommerce.Web.Models;
using EZcommerce.Web.Models.ViewModels;

namespace EZcommerce.Web.Services;

public interface IOrderService
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task UpdateAsync(Order order);
    Task UpdateAsync(OrderViewModel model);
    Task RemoveAsync(int orderId);
}