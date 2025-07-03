using Microsoft.AspNetCore.Mvc;
using WebApp1.Models.DataTransferObjects;
using WebApp1.Models;
using WebApp1.Services;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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


        #region HomePage


        public IActionResult HomeContentEdit()
        {
            var content = context.Contents.FirstOrDefault(); // Tek bir içerik kaydı varsayımı
            if (content == null)
            {
                content = new Content();
                context.Contents.Add(content);
                context.SaveChanges();
            }

            var dto_Content = new Dto_Content
            {
                Id = content.Id,
                Title = content.Title,
                PhoneNumber = content.PhoneNumber,
                HeaderTitleSm = content.HeaderTitleSm,
                HeaderTitleGr = content.HeaderTitleGr,
                HeaderImageUrl1 = null, // Dosya yüklemesi için null
                HeaderImageUrl2 = null, // Dosya yüklemesi için null
                HeaderParagraph = content.HeaderParagraph,
                AboutTitle = content.AboutTitle,
                AboutParagraph = content.AboutParagraph,
                AboutImageUrl = null, // Dosya yüklemesi için null
                MenuTitle = content.MenuTitle,
                GalleryTitle = content.GalleryTitle,
                FooterTitle = content.FooterTitle,
                FooterFaceBookUrl = content.FooterFaceBookUrl,
                FooterInstagramUrl = content.FooterInstagramUrl,
                FooterTwitterUrl = content.FooterTwitterUrl,
                FooterLocation = content.FooterLocation,
                FooterOpeningHours = content.FooterOpeningHours,
                FooterEmail = content.FooterEmail
            };
            ViewData["HeaderImageUrl1"] = content.HeaderImageUrl1;
            ViewData["HeaderImageUrl2"] = content.HeaderImageUrl2;
            ViewData["AboutImageUrl"] = content.AboutImageUrl;
            return View(dto_Content);
        }

        // Content Edit (POST)
        [HttpPost]
        public async Task<IActionResult> HomeContentEdit(Dto_Content dto_Content)
        {
            var content = context.Contents.FirstOrDefault();
            if (content == null)
            {
                return RedirectToAction("ContentEdit");
            }

            if (!ModelState.IsValid)
            {
                return View(dto_Content);
            }

            // HeaderImageUrl1 işleme
            if (dto_Content.HeaderImageUrl1 != null && dto_Content.HeaderImageUrl1.Length > 0)
            {
                string newFileName1 = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(dto_Content.HeaderImageUrl1.FileName);
                string imageFullPath1 = Path.Combine(environment.WebRootPath, "images", "content", newFileName1);
                string directoryPath = Path.Combine(environment.WebRootPath, "images", "content");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                using (var stream = new FileStream(imageFullPath1, FileMode.Create))
                {
                    await dto_Content.HeaderImageUrl1.CopyToAsync(stream);
                }
                content.HeaderImageUrl1 = newFileName1;
            }

            // HeaderImageUrl2 işleme
            if (dto_Content.HeaderImageUrl2 != null && dto_Content.HeaderImageUrl2.Length > 0)
            {
                string newFileName2 = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(dto_Content.HeaderImageUrl2.FileName);
                string imageFullPath2 = Path.Combine(environment.WebRootPath, "images", "content", newFileName2);
                string directoryPath = Path.Combine(environment.WebRootPath, "images", "content");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                using (var stream = new FileStream(imageFullPath2, FileMode.Create))
                {
                    await dto_Content.HeaderImageUrl2.CopyToAsync(stream);
                }
                content.HeaderImageUrl2 = newFileName2;
            }

            // AboutImageUrl işleme
            if (dto_Content.AboutImageUrl != null && dto_Content.AboutImageUrl.Length > 0)
            {
                string newFileName3 = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(dto_Content.AboutImageUrl.FileName);
                string imageFullPath3 = Path.Combine(environment.WebRootPath, "images", "content", newFileName3);
                string directoryPath = Path.Combine(environment.WebRootPath, "images", "content");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                using (var stream = new FileStream(imageFullPath3, FileMode.Create))
                {
                    await dto_Content.AboutImageUrl.CopyToAsync(stream);
                }
                content.AboutImageUrl = newFileName3;
            }

            content.Title = dto_Content.Title;
            content.PhoneNumber = dto_Content.PhoneNumber;
            content.HeaderTitleSm = dto_Content.HeaderTitleSm;
            content.HeaderTitleGr = dto_Content.HeaderTitleGr;
            content.HeaderParagraph = dto_Content.HeaderParagraph;
            content.AboutTitle = dto_Content.AboutTitle;
            content.AboutParagraph = dto_Content.AboutParagraph;
            content.MenuTitle = dto_Content.MenuTitle;
            content.GalleryTitle = dto_Content.GalleryTitle;
            content.FooterTitle = dto_Content.FooterTitle;
            content.FooterFaceBookUrl = dto_Content.FooterFaceBookUrl;
            content.FooterInstagramUrl = dto_Content.FooterInstagramUrl;
            content.FooterTwitterUrl = dto_Content.FooterTwitterUrl;
            content.FooterLocation = dto_Content.FooterLocation;
            content.FooterOpeningHours = dto_Content.FooterOpeningHours;
            content.FooterEmail = dto_Content.FooterEmail;

            await context.SaveChangesAsync();

            return RedirectToAction("ContentEdit");
        }

        #endregion






        #region Products
        public IActionResult ProductList()
        {
            var products = context.Products.Include(p => p.Category).ToList();

            return View(products);
        }

        public IActionResult ProductAdd()
        {
            ViewData["Categories"] = context.CategoryMenus.ToList();
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

            ViewData["Categories"] = context.CategoryMenus.ToList();
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

        public IActionResult ProductDelete(int id)
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




        #endregion






        #region Category
        // Category Menu List
        public IActionResult CategoryList()
        {
            var categories = context.CategoryMenus.ToList();
            return View(categories);
        }

        // Category Menu Add (GET)
        public IActionResult CategoryAdd()
        {
            return View();
        }

        // Category Menu Add (POST)
        [HttpPost]
        public async Task<IActionResult> CategoryAdd(Dto_CategoryMenu dto_CategoryMenu)
        {
            if (!ModelState.IsValid)
            {
                return View(dto_CategoryMenu);
            }

            var category = new CategoryMenu
            {
                Name = dto_CategoryMenu.Name,
                Description = dto_CategoryMenu.Description,
                CreatedAt = dto_CategoryMenu.CreatedAt
            };

            context.CategoryMenus.Add(category);
            await context.SaveChangesAsync();

            return RedirectToAction("CategoryList");
        }

        // Category Menu Edit (GET)
        public IActionResult CategoryEdit(int id)
        {
            var category = context.CategoryMenus.Find(id);
            if (category == null)
            {
                return RedirectToAction("CategoryList");
            }

            var dto_CategoryMenu = new Dto_CategoryMenu
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt
            };

            return View(dto_CategoryMenu);
        }

        // Category Menu Edit (POST)
        [HttpPost]
        public async Task<IActionResult> CategoryEdit(int id, Dto_CategoryMenu dto_CategoryMenu)
        {
            var category = context.CategoryMenus.Find(id);
            if (category == null)
            {
                return RedirectToAction("CategoryList");
            }

            if (!ModelState.IsValid)
            {
                return View(dto_CategoryMenu);
            }

            category.Name = dto_CategoryMenu.Name;
            category.Description = dto_CategoryMenu.Description;
            category.CreatedAt = dto_CategoryMenu.CreatedAt;

            await context.SaveChangesAsync();

            return RedirectToAction("CategoryList");
        }

        // Category Menu Delete
        public IActionResult CategoryDelete(int id)
        {
            var category = context.CategoryMenus.Find(id);
            if (category == null)
            {
                return RedirectToAction("CategoryList");
            }

            context.CategoryMenus.Remove(category);
            context.SaveChanges();

            return RedirectToAction("CategoryList");
        }
        #endregion










    }
}