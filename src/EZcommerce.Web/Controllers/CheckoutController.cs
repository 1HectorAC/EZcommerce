
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
    public async Task<IActionResult> CreateSession()
    {
        //var session = await _service.CreateCheckoutSession();

        // Create Order/OrderItem/Payment Objects here
        // Change Inventory quantity here

        // return session.url
        return Json(new {url = "/Checkout/Success"});
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