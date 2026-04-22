
using EZcommerce.Web.Models.Session;
using EZcommerce.Web.Services;
using EZcommerce.Web.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace EZcommerce.Web.Controllers;

[Controller]
public class CheckoutController: Controller
{
    
    private readonly CheckoutService _checkoutService;
    private readonly IEZcommerceService _service;

    private readonly ICartService _cartService;
    public CheckoutController(CheckoutService checkoutService, IEZcommerceService service, ICartService cartService)
    {
        _checkoutService = checkoutService;
        _service = service;
        _cartService = cartService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSession([FromBody] List<CartItem> cartItems)
    {
        if(cartItems.Count <= 0)
            return BadRequest("No Items in Cart Found.");
        try
        {
           _service.ValidateCart(cartItems) ;
        }catch(Exception ex)
        {
            // Maybe just remove invalid cart items instead of clearing
            _cartService.ClearCart();
            return BadRequest(ex.Message);
        }

        var session = await _checkoutService.CreateCheckoutSession(cartItems);
        Console.WriteLine("session ended");
        // Create Order/OrderItem/Payment Objects here
        // Change Inventory quantity here

        // return session.url
        return Json(new {url = session.Url});
    }

public IActionResult Success()
    {
        return View();
    }

    public IActionResult Cancel()
    {
        return View();
    }
}