using _305.Application.IUOW;

namespace _305.WebApi.Assistants.Middlewar;
public class TokenBlacklistMiddleware(
    RequestDelegate next,
    ILogger<TokenBlacklistMiddleware> logger,
    IUnitOfWork unitOfWork)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Extract the token from the Authorization header
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token))
        {
            // Check if the token is blacklisted
            var isBlacklisted = await unitOfWork.TokenBlacklistRepository.IsTokenBlacklisted(token);

            if (isBlacklisted)
            {
                logger.LogWarning("Blacklisted token used: {Token}", token);

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token is invalid or expired.");
                return;
            }
        }

        // Continue to the next middleware
        await next(context);
    }
}
