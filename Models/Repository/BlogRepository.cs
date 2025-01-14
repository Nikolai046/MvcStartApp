using Microsoft.EntityFrameworkCore;
using MvcStartApp.Models.DB;

namespace MvcStartApp.Models.Repository;

public interface IBlogRepository
{
    Task AddUser(User user);
    Task<User[]> GetUsers();
}

public class BlogRepository : IBlogRepository
{
    // ссылка на контекст
    private readonly BlogContext _context;

    // Метод-конструктор для инициализации
    public BlogRepository(BlogContext context)
    {
        _context = context;
    }

    public async Task AddUser(User user)
    {
        // Добавление пользователя
        var entry = _context.Entry(user);
        if (entry.State == EntityState.Detached)
            await _context.Users.AddAsync(user);

        // Сохранение изенений
        await _context.SaveChangesAsync();
    }

    public Task<User[]> GetUsers()
    {
        throw new NotImplementedException();
    }
}