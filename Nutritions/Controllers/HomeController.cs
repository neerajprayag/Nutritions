using Microsoft.AspNetCore.Mvc;

namespace Nutritions.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
