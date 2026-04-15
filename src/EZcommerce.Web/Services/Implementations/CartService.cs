
using EZcommerce.Web.Models.Session;
using System.Text.Json;

namespace EZcommerce.Web.Services.Implementations;

public class CartService: ICartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string CartKey = "Cart";

    public CartService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session => _httpContextAccessor.HttpContext!.Session;

    public List<CartItem> GetCart()
    {
        var value = Session.GetString(CartKey);
        if(value is null)
            return new List<CartItem>();
        return JsonSerializer.Deserialize<List<CartItem>>(value) ?? new List<CartItem>();
    }

    public void SaveCart(List<CartItem> cart)
    {
        var value = JsonSerializer.Serialize(cart);
        Session.SetString(CartKey, value);
    }

    public void AddToCart(CartItem cartItem)
    {
        var cart = GetCart();

        var existing = cart.FirstOrDefault(i => i.ProductId == cartItem.ProductId);
        if(existing != null)
        {
            existing.Quantity +=1;
        }
        else
        {
            cart.Add(cartItem);
        }
        SaveCart(cart);
    }

    public void RemoveFromCart(int productId)
    {
        var cart = GetCart();
        cart.RemoveAll(i => i.ProductId == productId);
        SaveCart(cart);
    }

    public void ClearCart()
    {
        Session.Remove(CartKey);
    }

}