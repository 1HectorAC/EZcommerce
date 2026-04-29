
using Microsoft.AspNetCore.Mvc;

namespace EZcommerce.Areas.Admin.Controllers;

[Area("Admin")]
[Controller]
public class HomeController: Controller
{
    public IActionResult Index()
    {
        return View();
    }
}