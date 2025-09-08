using _305.Application.IUOW;
using Serilog;

public class TokenBlacklistMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public TokenBlacklistMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

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

        await _next(context);
    }

    private static string? ExtractBearerToken(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].ToString();
        if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return authHeader.Substring("Bearer ".Length).Trim();
        return null;
    }
}
