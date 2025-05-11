using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaMVCProject.DAL;
using ProniaMVCProject.ViewModels;

namespace ProniaMVCProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //include-icinde reletional propertisi olan tablelerin datalarini getrmek ucun hemin table ile join edir.
            HomeVM homeVM = new HomeVM()
            {
                Slides = await _context.Slides.OrderBy(s => s.Order).Take(2).ToListAsync(),
                Products = await _context
                .Products
                .Take(4)
                .Include(p => p.ProductImage.Where(pi => pi.IsPrimary != null))
                .ToListAsync()
            };


            return View(homeVM);
        }
    }
}
