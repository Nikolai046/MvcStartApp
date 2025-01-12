using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MvcStartApp.Models;

namespace MvcStartApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
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
                // ������� �� �������� 404.cshtml
                return View("Error");
            }

            return View("Error"); // ������ ������� �������� ��� ������ ������
        }
    }
}
