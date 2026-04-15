
using EZcommerce.Web.Models.Session;
using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EZcommerce.Web.Controllers;

public class CartController: Controller
{
    private readonly ICartService _cart;
    public CartController(ICartService cart)
    {
        _cart = cart;
    }
    public IActionResult Index()
    {
        var cart = _cart.GetCart();
        return View(cart);
    }
    public IActionResult Add(CartItem cartItem)
    {

        _cart.AddToCart(cartItem);
        return RedirectToAction("Index");
    }

    public IActionResult Remove(int id)
    {
        _cart.RemoveFromCart(id);
        return RedirectToAction("Index");
    }


}