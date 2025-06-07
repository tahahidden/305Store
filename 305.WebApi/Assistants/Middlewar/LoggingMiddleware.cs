using Microsoft.AspNetCore.Http;
using System.Text;

namespace _305.WebApi.Assistants.Middlewar;

/// <summary>
/// Middleware برای لاگ گرفتن درخواست‌ها در فایل متنی.
/// </summary>
public class LoggingMiddleware
{
	private readonly RequestDelegate _next;
	private static readonly string LogPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "requests.txt");

	public LoggingMiddleware(RequestDelegate next)
	{
		_next = next;
		EnsureLogDirectoryExists();
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
	private static void EnsureLogDirectoryExists()
	{
		var directory = Path.GetDirectoryName(LogPath);
		if (!Directory.Exists(directory))
			Directory.CreateDirectory(directory!);
	}

	/// <summary>
	/// نوشتن لاگ در فایل به صورت async.
	/// </summary>
	private static async Task WriteLogAsync(byte[] logBytes)
	{
		await using var stream = new FileStream(LogPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 4096, useAsync: true);
		await stream.WriteAsync(logBytes, 0, logBytes.Length);
	}
}