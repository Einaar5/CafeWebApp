using Microsoft.AspNetCore.Mvc;
using WebApp1.Models.DataTransferObjects;
using WebApp1.Models;
using WebApp1.Services;
using System.IO;
using System.Threading.Tasks;

namespace WebApp1.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            var productCount = context.Products.Count();
            var categoryCount = context.CategoryMenus.Count();
            ViewBag.ProductCount = productCount;
            ViewBag.CategoryCount = categoryCount;
            return View();
        }

        public IActionResult ProductList()
        {
            var products = context.Products.ToList();
            return View(products);
        }

        public IActionResult ProductAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductAdd(Dto_Product dto_Product)
        {
            if (dto_Product.ImageFile == null || dto_Product.ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageUrl", "Lütfen bir resim yükleyin.");
            }

            if (!ModelState.IsValid)
            {
                return View(dto_Product);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(dto_Product.ImageFile.FileName);
            string imageFullPath = Path.Combine(environment.WebRootPath, "images", "productImage", newFileName);

            string directoryPath = Path.Combine(environment.WebRootPath, "images", "productImage");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var stream = new FileStream(imageFullPath, FileMode.Create))
            {
                await dto_Product.ImageFile.CopyToAsync(stream);
            }

            Product product = new Product
            {
                Name = dto_Product.Name,
                Description = dto_Product.Description,
                CategoryId = dto_Product.CategoryId,
                Price = dto_Product.Price,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = newFileName,
                isPopular = dto_Product.isPopular
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            return RedirectToAction("ProductList");
        }

        public IActionResult ProductEdit(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("ProductList");
            }

            Dto_Product dto_Product = new Dto_Product
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Price = product.Price,
                CreatedAt = product.CreatedAt,
                isPopular = product.isPopular
            };

            ViewData["ImageFileName"] = product.ImageUrl;
            return View(dto_Product);
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(int id, Dto_Product dto_Product)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ImageFileName"] = product.ImageUrl;
                return View(dto_Product);
            }

            string newFileName = product.ImageUrl;
            if (dto_Product.ImageFile != null && dto_Product.ImageFile.Length > 0)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(dto_Product.ImageFile.FileName);
                string imageFullPath = Path.Combine(environment.WebRootPath, "images", "productImage", newFileName);

                string directoryPath = Path.Combine(environment.WebRootPath, "images", "productImage");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (var stream = new FileStream(imageFullPath, FileMode.Create))
                {
                    await dto_Product.ImageFile.CopyToAsync(stream);
                }

                string oldImagePath = Path.Combine(environment.WebRootPath, "images", "productImage", product.ImageUrl);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            product.Name = dto_Product.Name;
            product.Description = dto_Product.Description;
            product.CategoryId = dto_Product.CategoryId;
            product.Price = dto_Product.Price;
            product.isPopular = dto_Product.isPopular;
            product.ImageUrl = newFileName;

            context.SaveChanges();

            return RedirectToAction("ProductList");
        }

        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("ProductList");
            }

            string imageFullPath = Path.Combine(environment.WebRootPath, "images", "productImage", product.ImageUrl);
            if (System.IO.File.Exists(imageFullPath))
            {
                System.IO.File.Delete(imageFullPath);
            }

            context.Products.Remove(product);
            context.SaveChanges();
            return RedirectToAction("ProductList");
        }
    }
}