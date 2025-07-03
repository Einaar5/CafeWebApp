using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp1.Models;
using WebApp1.Services;

namespace WebApp1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var content = context.Contents.FirstOrDefault();
            if (content == null)
            {
                content = new Content();
                context.Contents.Add(content);
                context.SaveChanges();
            }

            var viewModel = new HomeContentViewModel
            {
                Products = context.Products.Include(p => p.Category).ToList(),
                Galleries = context.Galleries.ToList(),
                Contents = context.Contents.ToList()
            };

            // Mevcut image URL'lerini ViewData'ya ekle
            ViewData["HeaderImageUrl1"] = content.HeaderImageUrl1;
            ViewData["HeaderImageUrl2"] = content?.HeaderImageUrl2;
            ViewData["AboutImageUrl"] = content?.AboutImageUrl;

            return View(viewModel);

           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
