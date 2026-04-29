using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EZcommerce.Areas.Admin.Controllers;

[Area("Admin")]
[Controller]
public class ProductController: Controller
{
    private readonly IEZcommerceService _service;
    public ProductController(IEZcommerceService service)
    {
        _service = service;
    }
    public async Task<IActionResult> Index()
    {
        var products = await _service.ProductGetAllIncludeInventoryAsync();
        return View(products);
    }
}