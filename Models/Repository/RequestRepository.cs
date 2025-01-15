using Microsoft.EntityFrameworkCore;
using MvcStartApp.Models.DB;
using MvcStartApp.Models.Entities;

namespace MvcStartApp.Models.Repository;


public interface IRequestRepository
{
    Task AddRequest(Request request);
    Task<Request[]> GetRequests();
}
public class RequestRepository : IRequestRepository
{
    private readonly BlogContext _context;

    public RequestRepository(BlogContext context)
    {
        _context = context;
    }
    public async Task AddRequest(Request request)
    {
        // Добавление лога
        var entry = _context.Entry(request);
        if (entry.State == EntityState.Detached)
            await _context.Requests.AddAsync(request);

        // Сохранение изменений
        await _context.SaveChangesAsync();
    }

    public async Task<Request[]> GetRequests()
    {
        // Получение всех логов
        return await _context.Requests.ToArrayAsync();
    }
}