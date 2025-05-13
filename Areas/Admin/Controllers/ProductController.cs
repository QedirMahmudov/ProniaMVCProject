using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")] //bu controllere kecid etmek ucun istifadeci login olmalidir! //Controller Base atributdur hemde action base atributdur.(her 2sindede isletmek olur)
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //[Authorize]
        public async Task<IActionResult> Index()
        {
            List<GetProductVM> productVMs = await _context.Products.Select(p =>
            new GetProductVM
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU,
                Price = p.Price,
                CategoryName = p.Category.Name,
                MainImage = p.ProductImage.FirstOrDefault(p => p.IsPrimary == true).Image,

            }

             ).ToListAsync();
            return View(productVMs);
        }


        public async Task<IActionResult> Create()
        {
            CreateProductVM productVM = new CreateProductVM()
            {
                Categories = await _context.Categories.ToListAsync()
            };
            return View(productVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            productVM.Categories = await _context.Categories.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(productVM);
            }

            bool result = productVM.Categories.Any(c => c.Id == productVM.CategoryId);
            if (!result)
            {
                ModelState.AddModelError(nameof(CreateProductVM.CategoryId), "Category does not exist");
                return View(productVM);
            }

            if (!productVM.MainPhoto.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateProductVM.MainPhoto), "file type is incorrect");
                return View(productVM);
            }


            if (productVM.MainPhoto.ValidateSize(FileSize.KB, 500))
            {
                ModelState.AddModelError(nameof(CreateProductVM.MainPhoto), "file must be less than 500kb");
                return View(productVM);
            }


            bool nameResult = await _context.Products.AnyAsync(p => p.Name == productVM.Name);
            if (nameResult)
            {
                ModelState.AddModelError(nameof(CreateProductVM.Name), $"Product with {productVM.Name} already exist");
                return View(productVM);
            }



            ProductImage main = new ProductImage
            {
                Image = await productVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images"),
                IsPrimary = true,

            };


            Product product = new Product
            {
                Name = productVM.Name,
                Price = productVM.Price.Value, //(decimal)productVM.Price
                SKU = productVM.SKU,
                Description = productVM.Description,
                CategoryId = productVM.CategoryId.Value,
                ProductImage = new List<ProductImage>() { main } // product.ProductImage.Add(main);
            };



            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Product? product = await _context.Products.Include(p => p.ProductImage).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            UpdateProductVM productVM = new UpdateProductVM
            {
                Name = product.Name,
                SKU = product.SKU,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                PrimaryImage = product.ProductImage.FirstOrDefault(p => p.IsPrimary == true).Image,
                Categories = await _context.Categories.ToListAsync()

            };


            return View(productVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateProductVM productVM)
        {
            productVM.Categories = await _context.Categories.ToListAsync();
            if (!ModelState.IsValid)
            {

                return View(productVM);
            };


            if (productVM.MainPhoto is not null)
            {
                if (!productVM.MainPhoto.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateProductVM.MainPhoto), "file type is incorrect");
                    return View(productVM);
                }


                if (productVM.MainPhoto.ValidateSize(FileSize.KB, 500))
                {
                    ModelState.AddModelError(nameof(UpdateProductVM.MainPhoto), "file must be less than 500kb");
                    return View(productVM);
                }
            }

            bool result = productVM.Categories.Any(c => c.Id == productVM.CategoryId);
            if (!result)
            {
                ModelState.AddModelError(nameof(UpdateProductVM.CategoryId), "category does not exist");
                return View(productVM);
            }
            bool nameResult = await _context.Products.AnyAsync(p => p.Name == productVM.Name && p.Id != id);
            if (nameResult)
            {
                ModelState.AddModelError(nameof(UpdateProductVM.Name), "Product already exist");
                return View(productVM);
            }


            Product? existed = await _context.Products.Include(p => p.ProductImage).FirstOrDefaultAsync(p => p.Id == id);


            if (productVM.MainPhoto is not null)
            {
                ProductImage main = new ProductImage
                {
                    Image = await productVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images"),
                    IsPrimary = true,
                    CreatedAt = DateTime.Now
                };

                ProductImage existedMain = existed.ProductImage.FirstOrDefault(pi => pi.IsPrimary == true);
                existedMain.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");

                existed.ProductImage.Remove(existedMain);
                existed.ProductImage.Add(main);

            }


            existed.Name = productVM.Name;
            existed.Price = productVM.Price.Value;
            existed.Description = productVM.Description;
            existed.CategoryId = productVM.CategoryId.Value;
            existed.SKU = productVM.SKU;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Product? product = await _context.Products.Include(p => p.ProductImage).FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return NotFound();

            foreach (ProductImage proImage in product.ProductImage)
            {
                proImage.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


    }
}
