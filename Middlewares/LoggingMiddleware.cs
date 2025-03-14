﻿using MvcStartApp.Models.Entities;
using MvcStartApp.Models.Repository;

namespace MvcStartApp.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly object LockObject = new();
    private readonly string _logFilePath;
    private readonly IServiceProvider _serviceProvider;

    // Добавляем множество для отслеживания обработанных запросов
    private static readonly HashSet<long> ProcessedRequestTimes = [];

    private static readonly Lock ProcessedRequestsLock = new();

    public LoggingMiddleware(RequestDelegate next, IWebHostEnvironment environment, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;

        // Создаём директорию для логов при инициализации
        var logDir = Path.Combine(environment.ContentRootPath, "Logs");
        if (!Directory.Exists(logDir))
            Directory.CreateDirectory(logDir);

        _logFilePath = Path.Combine(logDir, "RequestLog.txt");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Используем только временную метку для идентификации запроса
        var requestTicks = DateTime.Now.Ticks;

        // Проверяем уникальность запроса
        bool isNewRequest;
        lock (ProcessedRequestsLock)
        {
            isNewRequest = ProcessedRequestTimes.Add(requestTicks);

            // Очищаем старые записи (старше 1 минуты)
            var cutoffTime = DateTime.Now.AddMinutes(-1).Ticks;
            ProcessedRequestTimes.RemoveWhere(ticks => ticks < cutoffTime);
        }

        if (!isNewRequest)
        {
            await _next(context);
            return;
        }

        try
        {
            // Сохраняем информацию о запросе
            var requestTime = DateTime.Now;
            var requestUrl = $"http://{context.Request.Host.Value + context.Request.Path}";
            var logMessage = $"[{requestTime:yyyy-MM-dd HH:mm:ss}]: New request to {requestUrl}{Environment.NewLine}";

            // Логируем в консоль
            Console.WriteLine($"\n{logMessage}");

            // Логируем в файл
            await LogToFileAsync(logMessage);

            // Логируем в базу данных
            using (var scope = _serviceProvider.CreateScope())
            {
                await LogToDatabaseAsync(scope.ServiceProvider, requestUrl, requestTime);
            }

            // Передаем запрос дальше по пайплайну
            await _next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in middleware: {ex.Message}");
            await _next(context);
        }
    }

    private async Task LogToFileAsync(string message)
    {
        try
        {
            await Task.Run(() =>
            {
                lock (LockObject)
                {
                    File.AppendAllText(_logFilePath, message);
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }

    private async Task LogToDatabaseAsync(IServiceProvider scopedServices, string url, DateTime requestTime)
    {
        try
        {
            var repo = scopedServices.GetRequiredService<IRequestRepository>();

            var request = new Request
            {
                Id = Guid.NewGuid(),
                Date = requestTime,
                Url = url
            };

            await repo.AddRequest(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to database: {ex.Message}");
        }
    }
}