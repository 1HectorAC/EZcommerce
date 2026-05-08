using EZcommerce.Web.Models;
using EZcommerce.Web.Models.ViewModels;
using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EZcommerce.Areas.Admin.Controllers;

[Area("Admin")]
[Controller]
public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
    public async Task<IActionResult> Index()
    {
        var payments = await _paymentService.GetAllAsync();
        return View(payments);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var payment = await _paymentService.GetByIdAsync(id);
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
            await _paymentService.UpdateAsync(payment);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Payment/Edit, Post Error: " + ex.Message);
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var payment = await _paymentService.GetByIdAsync(id);

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
            await _paymentService.RemoveAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Order/DeleteConfirmed Error deleting: " + ex.Message);
        }
        return RedirectToAction("Index");
    }

}