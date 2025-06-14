using System.Text;
using Microsoft.Extensions.Options;
using _305.BuildingBlocks.Configurations;
using System.IO;

namespace _305.WebApi.Assistants.Middleware;

/// <summary>
/// Middleware برای لاگ گرفتن درخواست‌ها در فایل متنی.
/// </summary>
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _logPath;

    public LoggingMiddleware(RequestDelegate next, IOptions<RequestLoggingConfig> options)
    {
        _next = next;
        _logPath = options.Value.FilePath;
        EnsureLogDirectoryExists(_logPath);
    }

    /// <summary>
    /// اجرای لاگ و عبور به مرحله بعدی.
    /// </summary>
    public async Task Invoke(HttpContext context)
    {
        var logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {context.Request.Method} {context.Request.Path} | Origin: {context.Request.Headers["Origin"]}\n";
        var logBytes = Encoding.UTF8.GetBytes(logLine);

        await WriteLogAsync(logBytes);
        await _next(context);
    }

    /// <summary>
    /// اطمینان از وجود پوشه لاگ.
    /// </summary>
    private static void EnsureLogDirectoryExists(string logPath)
    {
        var directory = Path.GetDirectoryName(logPath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory!);
    }

    /// <summary>
    /// نوشتن لاگ در فایل به صورت async.
    /// </summary>
    private async Task WriteLogAsync(byte[] logBytes)
    {
        await using var stream = new FileStream(_logPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 4096, useAsync: true);
        await stream.WriteAsync(logBytes, 0, logBytes.Length);
    }
}

