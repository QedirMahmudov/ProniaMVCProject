using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaMVCProject.DAL;
using ProniaMVCProject.Models;
using ProniaMVCProject.Utilities.Enums;
using ProniaMVCProject.Utilities.Extensions;
using ProniaMVCProject.ViewModels;

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
            List<GetSlideVM> slideVMs = await _context.Slides.Select(s =>
                new GetSlideVM
                {
                    Id = s.Id,
                    Title = s.Title,
                    Order = s.Order,
                    Image = s.Image,
                    CreatedAt = s.CreatedAt,
                }).ToListAsync();

            return View(slideVMs);



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
        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        {

            if (!slideVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateSlideVM.Photo), "File type is incorrect");
                return View();
            }

            if (!slideVM.Photo.ValidateSize(FileSize.MB, 1))
            {
                ModelState.AddModelError(nameof(CreateSlideVM.Photo), "File size should be less than 1MB");
                return View();
            }


            //_env.WebRoot-----> wwwroota qeder olan adresi yazdirir                fullname photo.jpg
            //                   her kompda isleyir  

            string filenName = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");



            Slide slide = new Slide()
            {
                Title = slideVM.Title,
                SubTitle = slideVM.SubTitle,
                Description = slideVM.Description,
                Image = filenName,
                Order = slideVM.Order,
                CreatedAt = DateTime.Now
            };


            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));






            //bool existingSlide = await _context.Slides.AnyAsync(s => s.Order == slide.Order);


            //if (existingSlide)
            //{
            //    ModelState.AddModelError(nameof(Slide.Order), "choose another order");
            //    return View();
            //}


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


        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Slide? slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if (slide == null) return NotFound();

            slide.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");

            _context.Remove(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Slide? slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if (slide == null) return NotFound();

            UpdateSlideVM slideVM = new UpdateSlideVM
            {
                Description = slide.Description,
                Image = slide.Image,
                Order = slide.Order,
                SubTitle = slide.SubTitle,
                Title = slide.Title,
            };

            return View(slideVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSlideVM slideVM)
        {
            if (!ModelState.IsValid)
            {
                return View(slideVM);
            }

            Slide? existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if (existed is null) return NotFound();

            if (slideVM.Photo is not null)
            {
                if (!slideVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateSlideVM.Photo), "Type is incorrect");
                    return View(slideVM);
                }
                if (!slideVM.Photo.ValidateSize(FileSize.MB, 1))
                {
                    ModelState.AddModelError(nameof(UpdateSlideVM.Photo), "should be less than 1 mb");
                    return View(slideVM);
                }
                string fileName = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
                existed.Image = fileName;
            }

            existed.Title = slideVM.Title;
            existed.SubTitle = slideVM.SubTitle;
            existed.Description = slideVM.Description;
            existed.Order = slideVM.Order;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
