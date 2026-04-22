
using EZcommerce.Web.Models.Session;

namespace EZcommerce.Web.Services;

public interface IEZcommerceService
{
    void ValidateCart(List<CartItem> items);

   
}