
using EZcommerce.Web.Models.Session;

namespace EZcommerce.Web.Services;

public interface ICartService
{
    List<CartItem> GetCart();

    void SaveCart(List<CartItem> cart);

    void AddToCart(CartItem cartItem);

    void RemoveFromCart(int productId);

    void ClearCart();
}