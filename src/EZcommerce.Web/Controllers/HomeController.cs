using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EZcommerce.Web.Models;
using EZcommerce.Web.Data;
using EZcommerce.Web.Repositories;
using EZcommerce.Web.Models.ViewModels;

namespace EZcommerce.Web.Controllers;

public class HomeController : Controller
{
    private readonly IEZcommerceRepository _service;

    public HomeController(IEZcommerceRepository service)
    {
        _service = service;

    }

    public async Task<IActionResult> Index()
    {
        var products = await _service.GetProductsAsync();
        return View(products);
    }

    public async Task<IActionResult> ProductDetails(int id)
    {
        var product = await _service.GetProductbyIdAsync(id);
        if(product is null)
        {
            return RedirectToAction("Home", "Index");
        }
        var productInfo = new ProductDetailsViewModel()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description != null ? product.Description : "",
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Category = ""

        };
        return View(productInfo);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
