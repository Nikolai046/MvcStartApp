using Microsoft.EntityFrameworkCore;
using MvcStartApp.Middlewares;
using MvcStartApp.Models.DB;
using MvcStartApp.Models.Repository;

var builder = WebApplication.CreateBuilder(args);

// Настраиваем логирование
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddConsole()
        .AddFilter(DbLoggerCategory.Database.Command.Name,
            LogLevel.Information);
});

builder.Services.AddDbContext<BlogContext>((serviceProvider, options) =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));

    // Получаем ILoggerFactory из DI
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    options.UseLoggerFactory(loggerFactory);
});

// регистрация сервиса репозитория для взаимодействия с базой данных
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseMiddleware<LoggingMiddleware>();

// Включаем обработку ошибок, включая 404
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();