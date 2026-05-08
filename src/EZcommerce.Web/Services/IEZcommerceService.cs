
using EZcommerce.Web.Models;
using EZcommerce.Web.Models.Session;
using EZcommerce.Web.Models.ViewModels;

namespace EZcommerce.Web.Services;

public interface IEZcommerceService
{
    Task ValidateCart(List<CartItem> items);
    Task<int> OrderAndOrderItemsAddFromCartItemsAsync(List<CartItem> items);
    Task AddQuantitiesToInventoriesFromOrderAsync(int orderId);
    Task SubtractQuantitiesToInventoriesFromOrderAsync(int orderId);
    Task<List<Category>> CategoryGetAllAsync();
}