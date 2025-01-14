using Microsoft.EntityFrameworkCore;
using MvcStartApp.Models.Entities;

namespace MvcStartApp.Models.DB;

/// <summary>
/// Класс контекста, предоставляющий доступ к сущностям базы данных
/// </summary>
public sealed class BlogContext : DbContext
{
    private readonly ILoggerFactory _loggerFactory;

    /// Ссылка на таблицу Users
    public DbSet<User> Users { get; set; }

    /// Ссылка на таблицу UserPosts
    public DbSet<UserPost> UserPosts { get; set; }

    // Логика взаимодействия с таблицами в БД
    public BlogContext(DbContextOptions<BlogContext> options, ILoggerFactory loggerFactory) : base(options)
    {
        _loggerFactory = loggerFactory;
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Добавляем логгер в контекст
        optionsBuilder
            .UseLoggerFactory(_loggerFactory)  // Подключаем логгер
            .EnableSensitiveDataLogging()      // Показывать значения параметров в запросах
            .EnableDetailedErrors();           // Показывать детальные ошибки

        base.OnConfiguring(optionsBuilder);
    }
}