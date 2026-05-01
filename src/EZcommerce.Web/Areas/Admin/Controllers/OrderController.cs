using EZcommerce.Web.Models;
using EZcommerce.Web.Models.ViewModels;
using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EZcommerce.Areas.Admin.Controllers;

[Area("Admin")]
[Controller]
public class OrderController : Controller
{
    private readonly IEZcommerceService _service;
    public OrderController(IEZcommerceService service)
    {
        _service = service;
    }
    public async Task<IActionResult> Index()
    {
        var orders = await _service.OrderGetAllAsync();
        return View(orders);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var order = await _service.OrderGetByIdAsync(id);
        if (order is null)
        {
            Console.WriteLine("Order/Edit Error: order was null");
            return BadRequest();
        }

        var orderModel = new OrderViewModel
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            CustomerEmail = order.CustomerEmail,
            CustomerPhone = order.CustomerPhone,
            ShippingAddressLine1 = order.ShippingAddressLine1,
            ShippingAddressLine2 = order.ShippingAddressLine2,
            City = order.City,
            State = order.State,
            ZipCode = order.ZipCode,
            Country = order.Country,
            TotalAmmount = order.TotalAmmount,
            Status = order.Status
        };

        return View(orderModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(OrderViewModel order)
    {
        if (!ModelState.IsValid)
        {
            return View(order);
        }
        try
        {
            await _service.OrderUpdateAsync(order);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Order/Edit, Post Error: " + ex.Message);
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var order = await _service.OrderGetByIdAsync(id);

        if (order is null)
        {
            Console.WriteLine("Order/Delete, product is null");
            return BadRequest();
        }

        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            _service.OrderRemove(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Order/DeleteConfirmed Error deleting: " + ex.Message);
        }
        return RedirectToAction("Index");
    }

}