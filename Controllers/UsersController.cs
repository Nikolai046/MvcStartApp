using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcStartApp.Models.DB;
using MvcStartApp.Models.Entities;
using MvcStartApp.Models.Repository;

namespace MvcStartApp.Controllers;

public class UsersController : Controller
{
    private readonly IBlogRepository _repo;
    private readonly BlogContext _context;

    public UsersController(IBlogRepository repo, BlogContext context)
    {
        _repo = repo;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var authors = (await _repo.GetUsers()).OrderBy(o => o.JoinDate);
        return View(authors);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User newUser)
    {
        newUser.JoinDate = DateTime.Now;
        await _repo.AddUser(newUser);
        return View(newUser);
    }
    //[HttpPost]
    //public async Task<IActionResult> Register(User user)
    //{
    //    user.JoinDate = DateTime.Now;
    //    user.Id = Guid.NewGuid();

    //    // Добавление пользователя
    //    var entry = _context.Entry(user);
    //    if (entry.State == EntityState.Detached)
    //        await _context.Users.AddAsync(user);

    //    // Сохранение изменений
    //    await _context.SaveChangesAsync();
    //    return Content($"Registration successful, {user.FirstName}");
    //}

}