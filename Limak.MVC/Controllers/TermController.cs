using Microsoft.AspNetCore.Mvc;

namespace Limak.MVC.Controllers;

public class TermController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
