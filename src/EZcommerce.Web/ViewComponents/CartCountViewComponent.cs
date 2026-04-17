
using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;

public class CartCountViewComponent: ViewComponent
{
    private readonly ICartService _cart;

    public CartCountViewComponent(ICartService cart)
    {
        _cart = cart;
    }
    public IViewComponentResult Invoke()
    {
        var count = _cart.GetInventoryCount();
        return View(count);
    }
}