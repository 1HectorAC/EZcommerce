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
    public async Task<IActionResult> Create(ProductCreateViewModel model)
    {
        Console.WriteLine("Create function called");
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Product/Create: Model invalid");
            return View(model);
        }

        await _service.ProductCreateWithInventory(model);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _service.ProductGetWithInventoryAsync(id);
        if (product is null)
        {
            Console.WriteLine("Product/Edit Error: product was null");
            return BadRequest();
        }

        var productModel = new ProductCreateViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            InventoryQuantity = product.Inventory!.Quantity
        };

        var categories = await _service.CategoryGetAllAsync();
        ViewBag.categories = new SelectList(categories, "Id", "Name");

        return View(productModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductCreateViewModel product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }
        try
        {
            await _service.ProductEditWithInventory(product);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Product/Edit, Post Error: " + ex.Message);
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _service.ProductGetWithInventoryAsync(id);

        if (product is null)
        {
            Console.WriteLine("Product/Delete, product is null");
            return BadRequest();
        }

        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            _service.ProductRemove(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Product/DeleteConfirmed Error deleting: " + ex.Message);
        }
        return RedirectToAction("Index");
    }

}