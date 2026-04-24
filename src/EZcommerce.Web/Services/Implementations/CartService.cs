
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

    public int GetCount()
    {
        var cart = GetCart();
        return cart.Count;
    }

    public int GetInventoryCount()
    {
        var cart = GetCart();
        return cart.Sum(i => i.Quantity);
    }

    public int GetCartItemQuantity(int productId)
    {
        var cart = GetCart();
        var existing = cart.FirstOrDefault(i => i.ProductId == productId);
        if(existing == null)
            return 0;
        return existing.Quantity;
    }

    public decimal GetTotalPrice()
    {
        var cart = GetCart();
        var totalPrice = cart.Sum(i => i.PriceSnapshot * i.Quantity);
        return Math.Round(totalPrice, 2);
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

    public void DecrementCartItemQuantity(int productId)
    {
        var cart = GetCart();
        var existing = cart.FirstOrDefault(i => i.ProductId == productId);
    
        if(existing is null || existing.Quantity <= 0)
        {
            Console.WriteLine("DecrementCartItemQuantity: No productId found");
            // Maybe return error instead
            return;
        }
        if(existing.Quantity == 1)
        {
            // Maybe return error instead, not its responsibility
            RemoveFromCart(productId); 
        }
        else
        {
            existing.Quantity -= 1;
            SaveCart(cart);
        }
    }

    public void ClearCart()
    {
        Session.Remove(CartKey);
    }

}