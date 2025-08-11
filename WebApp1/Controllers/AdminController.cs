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
                content = new Content
                {
                    Title = "Default Title",
                    PhoneNumber = "5551234567",
                    HeaderTitleSm = "Small Header",
                    HeaderTitleGr = "Large Header",
                    HeaderParagraph = "Header paragraph text",
                    AboutTitle = "About Us",
                    AboutParagraph = "About paragraph text",
                    MenuTitle = "Our Menu",
                    GalleryTitle = "Gallery",
                    FooterTitle = "Contact Us",
                    FooterFaceBookUrl = "",
                    FooterInstagramUrl = "",
                    FooterTwitterUrl = "",
                    FooterLocation = "",
                    FooterOpeningHours = "",
                    FooterEmail = ""
                };
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
            // Debug: Gelen veriyi kontrol et
            TempData["DebugMessage"] = $"POST received - Title: {dto_Content?.Title}, Phone: {dto_Content?.PhoneNumber}";
            
            var content = context.Contents.FirstOrDefault();
            if (content == null)
            {
                TempData["ErrorMessage"] = "No content found in database";
                return RedirectToAction("HomeContentEdit");
            }

            // Basit test - sadece title'ı güncelle
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

            // HeaderImageUrl1
            if (dto_Content.HeaderImageUrl1 != null && dto_Content.HeaderImageUrl1.Length > 0)
            {
                var newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(dto_Content.HeaderImageUrl1.FileName);
                var imageFullPath = Path.Combine(environment.WebRootPath, "images", "content", newFileName);
                using (var stream = new FileStream(imageFullPath, FileMode.Create))
                {
                    await dto_Content.HeaderImageUrl1.CopyToAsync(stream);
                }
                // Eski dosyayı sil
                if (!string.IsNullOrEmpty(content.HeaderImageUrl1))
                {
                    var oldImagePath = Path.Combine(environment.WebRootPath, "images", "content", content.HeaderImageUrl1);
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }
                content.HeaderImageUrl1 = newFileName;
            }
            // HeaderImageUrl2
            if (dto_Content.HeaderImageUrl2 != null && dto_Content.HeaderImageUrl2.Length > 0)
            {
                var newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(dto_Content.HeaderImageUrl2.FileName);
                var imageFullPath = Path.Combine(environment.WebRootPath, "images", "content", newFileName);
                using (var stream = new FileStream(imageFullPath, FileMode.Create))
                {
                    await dto_Content.HeaderImageUrl2.CopyToAsync(stream);
                }
                if (!string.IsNullOrEmpty(content.HeaderImageUrl2))
                {
                    var oldImagePath = Path.Combine(environment.WebRootPath, "images", "content", content.HeaderImageUrl2);
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }
                content.HeaderImageUrl2 = newFileName;
            }
            // AboutImageUrl
            if (dto_Content.AboutImageUrl != null && dto_Content.AboutImageUrl.Length > 0)
            {
                var newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(dto_Content.AboutImageUrl.FileName);
                var imageFullPath = Path.Combine(environment.WebRootPath, "images", "content", newFileName);
                using (var stream = new FileStream(imageFullPath, FileMode.Create))
                {
                    await dto_Content.AboutImageUrl.CopyToAsync(stream);
                }
                if (!string.IsNullOrEmpty(content.AboutImageUrl))
                {
                    var oldImagePath = Path.Combine(environment.WebRootPath, "images", "content", content.AboutImageUrl);
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }
                content.AboutImageUrl = newFileName;
            }
            
            await context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Content updated successfully!";
            return RedirectToAction("HomeContentEdit");
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

        #region Gallery

        public IActionResult GalleryList()
        {
            var galleryList = context.Galleries.ToList();
            var contentList = context.Contents.ToList();

            var galleryModel = new HomeContentViewModel
            {
                Galleries = galleryList,
                Contents = contentList
            };

            return View(galleryModel);
        }


        public IActionResult GalleryAdd()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> GalleryAdd(Dto_Gallery dto_gallery)
        {
            if (!ModelState.IsValid)
            {
                return View(dto_gallery);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(dto_gallery.ImageUrl.FileName);
            string directoryPath = Path.Combine(environment.WebRootPath, "images", "gallery");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string imageFullPath = Path.Combine(directoryPath, newFileName);

            using (var stream = new FileStream(imageFullPath, FileMode.Create))
            {
                await dto_gallery.ImageUrl.CopyToAsync(stream);
            }

            var gallery = new Gallery
            {
                ImageUrl = newFileName
            };

            context.Galleries.Add(gallery);
            await context.SaveChangesAsync();

            // Listeye yönlendir
            return RedirectToAction("GalleryList");
        }



        public IActionResult Edit(int id)
        {
            var gallery = context.Galleries.Find(id);
            if (gallery == null)
                return NotFound();

            var dto = new Dto_Gallery
            {
                Id = gallery.Id,
                ImageUrl = gallery.ImageUrl != null ? new FormFile(new MemoryStream(), 0, 0, null, gallery.ImageUrl) : null // burada ImageUrl'yi FormFile olarak ayarlıyoruz
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> GalleryEdit(Dto_Gallery dto)
        {
            var gallery = context.Galleries.Find(dto.Id);
            if (gallery == null)
                return NotFound();

            if (dto.ImageUrl != null)
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(dto.ImageUrl.FileName);
                string imageFullPath = Path.Combine(environment.WebRootPath, "images", "gallery", newFileName);

                using (var stream = new FileStream(imageFullPath, FileMode.Create))
                {
                    await dto.ImageUrl.CopyToAsync(stream);
                }

                // Eski resmi sil
                var oldPath = Path.Combine(environment.WebRootPath, "images", "gallery", gallery.ImageUrl);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                gallery.ImageUrl = newFileName;
            }

            await context.SaveChangesAsync();
            return RedirectToAction("GalleryList");
        }

        [HttpGet]
        public IActionResult GalleryDelete(int id)
        {
            var gallery = context.Galleries.Find(id);
            if (gallery == null)
                return NotFound();

            // Resmi sil
            var imagePath = Path.Combine(environment.WebRootPath, "images", "gallery", gallery.ImageUrl);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            context.Galleries.Remove(gallery);
            context.SaveChanges();

            return RedirectToAction("GalleryList");
        }

        #endregion










    }
}