
using System.Text.Json.Serialization;
using EZcommerce.Web.Models.Session;
using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EZcommerce.Web.Controllers;

public class CartController : Controller
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

    [HttpPost]
    public IActionResult Add([FromBody] CartItem cartItem)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Inputed data was not properly formated");
        }
        _cart.AddToCart(cartItem);

        //var cartItemQuantity = _cart.GetCartItemQuantity(cartItem.ProductId);

        //consider returning quantity
        return Ok(1);
    }

    [HttpGet("[controller]/LowerQuantity/{productId}")]
    public IActionResult LowerQuantity([FromRoute] int productId)
    {
        _cart.DecrementCartItemQuantity(productId);

        //consider returning quantity
        return Ok(1);
    }

    public IActionResult Remove(int id)
    {
        _cart.RemoveFromCart(id);
        return RedirectToAction("Index");
    }

    // Used in navbar for initial cart total display
    [HttpGet]
    public IActionResult GetInventoryCount()
    {
        int totalCount = _cart.GetInventoryCount();
        return Ok(totalCount);
    }

}