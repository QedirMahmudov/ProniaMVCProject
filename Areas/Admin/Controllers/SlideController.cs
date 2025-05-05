using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaMVCProject.DAL;
using ProniaMVCProject.Models;

namespace ProniaMVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        //                                      wwwroota qeder olan adresi ozunde saxlayir
        public SlideController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slide> slides = await _context.Slides.ToListAsync();
            return View(slides);
        }
        public IActionResult Create()
        {
            return View();
        }
        //public string Test()
        //{
        //    string name = "flowers.jpg";

        //    return string.Concat(Guid.NewGuid().ToString(), name.Substring(name.LastIndexOf(".")));
        //}
        [HttpPost]
        public async Task<IActionResult> Create(Slide slide)
        {

            if (!slide.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError(nameof(Slide.Photo), "File type is incorrect");
                return View();
            }

            if (slide.Photo.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError(nameof(Slide.Photo), "File size should be less than 2MB");
                return View();
            }

            bool existingSlide = await _context.Slides.AnyAsync(s => s.Order == slide.Order);

            if (existingSlide)
            {
                ModelState.AddModelError(nameof(Slide.Order), "choose another order");
                return View();
            }

            //_env.WebRoot-----> wwwroota qeder olan adresi yazdirir                fullname photo.jpg
            //                   her kompda isleyir  

            string fileName = string.Concat(Guid.NewGuid().ToString(), slide.Photo.FileName.Substring(slide.Photo.FileName.LastIndexOf(".")));
            string path = Path.Combine(_env.WebRootPath, "assets", "images", "website-images", fileName);


            FileStream fileStream = new FileStream(path, FileMode.Create);
            //gonderilen phtonu yuxaridaki streamin adresine yukleyir
            await slide.Photo.CopyToAsync(fileStream);

            slide.Image = fileName;

            slide.CreatedAt = DateTime.Now;
            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));








            //"assets\\images\\website-images\\" / lar ferqli olur deye / lardan uzaq durmaliyiq!




            //                  slideflower.webp              image/webp                    63294
            //return Content(slide.Photo.FileName + " " + slide.Photo.ContentType + " " + slide.Photo.Length);













            //if (!ModelState.IsValid) return View();

            //bool existingSlide = await _context.Slides.AnyAsync(s => s.Order == slide.Order);

            //if (existingSlide)
            //{
            //    ModelState.AddModelError("Order", $" number of  {slide.Order} has already exist");
            //    return View();
            //}
            //slide.CreatedAt = DateTime.Now;

            //await _context.Slides.AddAsync(slide);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }
    }
}
