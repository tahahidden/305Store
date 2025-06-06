
using _305.Application.IUOW;

namespace _305.WebApi.Assistants.Middelware;
public class TokenBlacklistMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenBlacklistMiddleware> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public TokenBlacklistMiddleware(RequestDelegate next, ILogger<TokenBlacklistMiddleware> logger, IUnitOfWork unitOfWork)
    {
        _next = next;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Extract the token from the Authorization header
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token))
        {
            // Check if the token is blacklisted
            var isBlacklisted = await _unitOfWork.TokenBlacklistRepository.IsTokenBlacklisted(token);

            if (isBlacklisted)
            {
                _logger.LogWarning("Blacklisted token used: {Token}", token);

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token is invalid or expired.");
                return;
            }
        }

        // Continue to the next middleware
        await _next(context);
    }
}
