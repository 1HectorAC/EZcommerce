
using EZcommerce.Web.Models.Session;
using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EZcommerce.Web.Controllers;

[Controller]
public class CheckoutController: Controller
{
    
    private readonly CheckoutService _service;
    public CheckoutController(CheckoutService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSession([FromBody] List<CartItem> cartItems)
    {
        if(cartItems.Count <= 0)
            return BadRequest("No Items in Cart Found.");
        

        // check cart productIds exits
        // check prices, redirect if error
        // check inventory, redirect if error

        var session = await _service.CreateCheckoutSession(cartItems);
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