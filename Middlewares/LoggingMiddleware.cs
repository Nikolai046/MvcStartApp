namespace MvcStartApp.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly Lock LockObject = new();
    private readonly string _logFilePath;

    public LoggingMiddleware(RequestDelegate next, IWebHostEnvironment environment)
    {
        _next = next;

        // Создаём директорию для логов при инициализации
        var logDir = Path.Combine(environment.ContentRootPath, "Logs");
        if (!Directory.Exists(logDir))
            Directory.CreateDirectory(logDir);

        _logFilePath = Path.Combine(logDir, "RequestLog.txt");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Логируем до обработки запроса
        await LogRequestAsync(context);

        // Передаем запрос дальше по пайплайну
        await _next(context);
    }

    private void LogConsole(HttpContext context)
    {
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]: New request to http://{context.Request.Host.Value + context.Request.Path}");
    }

    private async Task LogRequestAsync(HttpContext context)
    {
        // Логируем в консоль
        LogConsole(context);

        // Формируем сообщение для лога
        string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]: New request to http://{context.Request.Host.Value + context.Request.Path}{Environment.NewLine}";

        try
        {
            // Используем lock для синхронизации доступа к файлу
            lock (LockObject)
            {
                // Используем StreamWriter с параметром append: true
                using (var writer = new StreamWriter(_logFilePath, append: true))
                {
                    writer.Write(logMessage);
                }
            }
        }
        catch (Exception ex)
        {
            // Логируем ошибку записи в консоль, чтобы не прерывать обработку запроса
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }
}