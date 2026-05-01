using EZcommerce.Web.Models;
using EZcommerce.Web.Models.ViewModels;
using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EZcommerce.Areas.Admin.Controllers;

[Area("Admin")]
[Controller]
public class PaymentController : Controller
{
    private readonly IEZcommerceService _service;
    public PaymentController(IEZcommerceService service)
    {
        _service = service;
    }
    public async Task<IActionResult> Index()
    {
        var payments = await _service.PaymentGetAllAsync();
        return View(payments);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var payment = await _service.PaymentGetByIdAsync(id);
        if (payment is null)
        {
            Console.WriteLine("Payment/Edit Error: order was null");
            return BadRequest();
        }

        

        return View(payment);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Payment payment)
    {
        if (!ModelState.IsValid)
        {
            return View(payment);
        }
        try
        {
            await _service.PaymentEditAsync(payment);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Payment/Edit, Post Error: " + ex.Message);
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var payment = await _service.PaymentGetByIdAsync(id);

        if (payment is null)
        {
            Console.WriteLine("Payment/Delete, product is null");
            return BadRequest();
        }

        return View(payment);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            _service.PaymentRemove(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Order/DeleteConfirmed Error deleting: " + ex.Message);
        }
        return RedirectToAction("Index");
    }

}