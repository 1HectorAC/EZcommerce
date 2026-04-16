
using EZcommerce.Web.Models.Session;

namespace EZcommerce.Web.Services;

public interface ICartService
{
    List<CartItem> GetCart();

    int GetCartItemQuantity(int productId);

    void SaveCart(List<CartItem> cart);

    void AddToCart(CartItem cartItem);

    void RemoveFromCart(int productId);

    void DecrementCartItemQuantity(int productId);

    void ClearCart();
}