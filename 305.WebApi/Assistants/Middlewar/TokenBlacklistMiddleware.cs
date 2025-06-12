using _305.Application.IUOW;
using Serilog;

namespace _305.WebApi.Assistants.Middlewar;

/// <summary>
/// Middleware برای جلوگیری از استفاده توکن‌هایی که در لیست سیاه قرار گرفته‌اند.
/// </summary>
public class TokenBlacklistMiddleware(RequestDelegate next, IUnitOfWork unitOfWork)
{
	public async Task InvokeAsync(HttpContext context)
	{
		var token = ExtractBearerToken(context);

		if (!string.IsNullOrWhiteSpace(token))
		{
			var isBlacklisted = await unitOfWork.TokenBlacklistRepository
				.AnyAsync(x => x.token == token && x.expiry_date > DateTime.UtcNow);

			if (isBlacklisted)
			{
				Log.Warning("Blacklisted token used: {Token}", token);
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("Token is invalid or expired.");
				return;
			}
		}

		await next(context);
	}

	/// <summary>
	/// استخراج توکن Bearer از هدر Authorization.
	/// </summary>
	private static string? ExtractBearerToken(HttpContext context)
	{
		var authHeader = context.Request.Headers["Authorization"].ToString();
		if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
			return authHeader.Substring("Bearer ".Length).Trim();
		return null;
	}
}