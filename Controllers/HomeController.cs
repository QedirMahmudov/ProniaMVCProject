using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaMVCProject.DAL;
using ProniaMVCProject.ViewModels;

namespace ProniaMVCProject.Controllers
{
    public class HomeController : Controller
    {


        public readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //include-icinde reletional propertisi olan tablelerin datalarini getrmek ucun hemin table ile join edir.
            HomeVM homeVM = new HomeVM()
            {
                Slides = _context.Slides.OrderBy(s => s.Order).Take(2).ToList(),
                Products = _context
                .Products
                .Take(4)
                .Include(p => p.ProductImage.Where(pi => pi.IsPrimary != null))
                .ToList()
            };


            return View(homeVM);
        }
    }
}
