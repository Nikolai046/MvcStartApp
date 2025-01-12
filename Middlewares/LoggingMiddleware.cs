namespace MvcStartApp.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    ///  Middleware-компонент должен иметь конструктор, принимающий RequestDelegate
    /// </summary>
    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    ///  Необходимо реализовать метод Invoke  или InvokeAsync
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        LogConsole(context);
        await LogFile(context);

        // Передача запроса далее по конвейеру
        await _next.Invoke(context);
    }

    private void LogConsole(HttpContext context)
    {
        // Для логирования данных о запросе используем свойста объекта HttpContext
        Console.WriteLine($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");
    }

    private async Task LogFile(HttpContext context)
    {
        // Строка для публикации в лог
        string logMessage = $"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}{Environment.NewLine}";

        // Путь до лога (опять-таки, используем свойства IWebHostEnvironment)
        string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "RequestLog.txt");

        // Используем асинхронную запись в файл
        await File.AppendAllTextAsync(logFilePath, logMessage);
    }

}