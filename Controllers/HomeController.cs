using Microsoft.AspNetCore.Mvc;
using MvcStartApp.Models;
using MvcStartApp.Models.Repository;
using System.Diagnostics;

namespace MvcStartApp.Controllers
{
    // ReSharper disable once HollowTypeName
    public class HomeController : Controller
    {
        private readonly IBlogRepository _repo;
        private readonly ILogger<HomeController> _logger;

        // ReSharper disable once ConvertToPrimaryConstructor
        public HomeController(ILogger<HomeController> logger, IBlogRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            return View();
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

        [Route("Error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            if (statusCode == 404)
            {
                // Переход на страницу 404.cshtml
                return View("Error");
            }

            return View("Error"); // Можете создать страницу для других ошибок
        }
    }
}