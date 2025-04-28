using Microsoft.AspNetCore.Mvc;
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
            #region Slides
            //List<Slide> slides = new List<Slide>()
            //{
            //    new Slide
            //    {
            //        Title = "Tikanli Kaktus",
            //        SubTitle = "Zeherlidi",
            //        Description = "kaktus alveri ucun elaqe qurun",
            //        Order= 1,
            //        Image ="kaktus.webp",
            //        CreatedAt= DateTime.Now
            //    },
            //    new Slide
            //    {
            //        Title = "Roza",
            //        SubTitle = "Qirmizi rengli",
            //        Description = "Denesi 20man (Okush)",
            //        Order= 3,
            //        Image ="roza.webp",
            //        CreatedAt= DateTime.Now
            //    },
            //    new Slide
            //    {
            //        Title = "Tulpan",
            //        SubTitle = "Rengbereng Tulpanlar",
            //        Description = "Denesi 5man,Endirim yoxdur:)",
            //        Order= 2,
            //        Image ="tulpan.webp",
            //        CreatedAt= DateTime.Now
            //    }
            //};


            //_context.Slides.AddRange(slides);
            //_context.SaveChanges();
            #endregion





            HomeVM homeVM = new HomeVM()
            {
                Slides = _context.Slides.OrderBy(s => s.Order).Take(2).ToList()
            };


            return View(homeVM);
        }
    }
}
