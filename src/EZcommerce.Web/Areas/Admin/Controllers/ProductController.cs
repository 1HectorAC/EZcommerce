using EZcommerce.Web.Models;
using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EZcommerce.Areas.Admin.Controllers;

[Area("Admin")]
[Controller]
public class ProductController : Controller
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

    public async Task<IActionResult> Edit(int id)
    {

        // Get product by id with inventory

        var product = new ProductCreateViewModel
        {
            Id = 1,
            Name = "bill",
            Description = "tissljiojio",
            Price = 11.11m,
            ImageUrl = "",
            CategoryId = 3,
            InventoryQuantity = 1
        };

        var categories = await _service.CategoryGetAllAsync();
        ViewBag.categories = new SelectList(categories, "Id", "Name");

        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductCreateViewModel product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }
        // edit product

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        // get product and pass to View
        var product = new Product
        {
            Id = 1,
            Name = "11",
            Description = "adjsfowjfi",
            Price = 1.11m,
            CategoryId = 1,
            Created_at = new DateTime(2025, 1, 1)
        };
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Delete the product

        return RedirectToAction("Index");
    }


}