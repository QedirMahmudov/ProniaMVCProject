using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaMVCProject.DAL;
using ProniaMVCProject.Models;


namespace ProniaMVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SizeController : Controller
    {
        private readonly AppDbContext _context;

        public SizeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Size> size = await _context.Sizes.ToListAsync();
            return View(size);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Size size)
        {
            if (!ModelState.IsValid)
                return View();

            bool result = await _context.Sizes.AnyAsync(r => r.Name == size.Name);

            if (result)
            {
                ModelState.AddModelError(nameof(size.Name), $"{size.Name} named already exist");
                return View();
            }

            size.CreatedAt = DateTime.Now;

            _context.Sizes.Add(size);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Size? size = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);

            if (size is null) return NotFound();


            return View(size);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Size size)
        {
            if (!ModelState.IsValid)
                return View();

            bool result = await _context.Sizes.AnyAsync(s => s.Name == size.Name && s.Id != id);

            if (result)
            {
                ModelState.AddModelError(nameof(size.Name), $"{size.Name} named size already exist");
                return View();
            }
            Size? existed = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);

            if (existed.Name == size.Name) return RedirectToAction(nameof(Index));

            existed.Name = size.Name;

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));


        }




    }
}
