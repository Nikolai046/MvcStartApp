using Microsoft.AspNetCore.Mvc;
using MvcStartApp.Models.DB;
using MvcStartApp.Models.Repository;

namespace MvcStartApp.Controllers
{
    public class LogsController : Controller
    {
        private readonly IRequestRepository _repo;
        private readonly BlogContext _context;

        // ReSharper disable once ConvertToPrimaryConstructor
        public LogsController(IRequestRepository repo, BlogContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var logs = (await _repo.GetRequests()).OrderBy(o => o.Date);
            return View(logs);
        }
    }
}