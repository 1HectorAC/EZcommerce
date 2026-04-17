
using System.Text.Json.Serialization;
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

    [HttpPost]
    public IActionResult Add2([FromBody] CartItem cartItem)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Inputed data was not properly formated");
        }
        Console.WriteLine($"productId: {cartItem.ProductId} name; {cartItem.Name}");
        _cart.AddToCart(cartItem);
        
        //consider returning quantity
        return Ok(1);
    }

    [HttpGet("[controller]/LowerQuantity/{productId}")]
    public IActionResult LowerQuantity([FromRoute] int productId)
    {
        Console.WriteLine("CartController productId: " + productId);
        _cart.DecrementCartItemQuantity(productId);

        //consider returning quantity
        return Ok(1);
    }

    public IActionResult Remove(int id)
    {
        _cart.RemoveFromCart(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult GetCartCount()
    {
        int totalCount = _cart.GetCount();
        return Ok(totalCount);
    }

    [HttpGet]
    public IActionResult GetInventoryCount()
    {
        int totalCount = _cart.GetInventoryCount();
        return Ok(totalCount);
    }

}