using EZcommerce.Web.Models;
using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

    public async Task<IActionResult> Create()
    {
        var categories = await _service.CategoryGetAllAsync();
        ViewBag.categories = new SelectList(categories, "Id", "Name");


        return View();
    }

    [HttpPost]
    public IActionResult Create(ProductCreateViewModel model)
    {
        Console.WriteLine("Create function called");
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Product/Create: Model invalid");
            return View(model);
        }

        Console.WriteLine(model.Price);
        // Add Product Here

        return RedirectToAction("Index");
    }

}