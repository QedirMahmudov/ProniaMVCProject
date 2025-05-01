using Microsoft.AspNetCore.Mvc;

namespace ProniaMVCProject.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
