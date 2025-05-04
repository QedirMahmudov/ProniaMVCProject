using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaMVCProject.DAL;
using ProniaMVCProject.Models;

namespace ProniaMVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;

        public ColorController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Color> color = await _context.Colors.ToListAsync();
            return View(color);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Color color)
        {
            if (!ModelState.IsValid) return View();

            bool result = await _context.Colors.AnyAsync(c => c.Name == color.Name);

            if (result)
            {
                ModelState.AddModelError(nameof(color.Name), $"{color.Name} named already exist");
                return View();
            }

            color.CreatedAt = DateTime.Now;

            _context.Colors.Add(color);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Color? color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            //2 ifde olmasa kod problemsiz islemir? netice olaraq tablede olan datalar uzerinde update olunur, onlarinda idsi var,
            if (color is null) return NotFound();

            return View(color);

        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Color color)
        {
            if (!ModelState.IsValid) return View();


            bool result = await _context.Colors.AnyAsync(c => c.Name == color.Name && c.Id != id);

            if (result)
            {
                ModelState.AddModelError(nameof(color.Name), $"{color.Name} named color already exist");
                return View();
            }

            Color? existed = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);

            if (existed.Name == color.Name) return RedirectToAction(nameof(Index));

            existed.Name = color.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //Bunu ozum bilerekden Id gondermeden,ancaq color gondererek yazdim. Sual verecem bu haqda!. Netice olaraq 2 haldada isleyir...
        //public async Task<IActionResult> Update(Color color)
        //{

        //    if (!ModelState.IsValid) return View();


        //    bool result = await _context.Colors.AnyAsync(c => c.Name == color.Name & c.Id != color.Id);
        //    if (result)
        //    {
        //        ModelState.AddModelError(nameof(color.Name), $"{color.Name} named color already exist");
        //        return View();
        //    }

        //    Color? existed = await _context.Colors.FirstOrDefaultAsync(c => c.Id == color.Id);

        //    if (existed.Name == color.Name) return RedirectToAction(nameof(Index));

        //    existed.Name = color.Name;
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}



    }
}
