using Microsoft.AspNetCore.Mvc;

namespace Limak.MVC.Controllers
{
    public class FAQController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
