using Microsoft.AspNetCore.Mvc;

namespace Limak.MVC.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
