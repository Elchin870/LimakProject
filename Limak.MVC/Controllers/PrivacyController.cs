using Microsoft.AspNetCore.Mvc;

namespace Limak.MVC.Controllers;

public class PrivacyController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
