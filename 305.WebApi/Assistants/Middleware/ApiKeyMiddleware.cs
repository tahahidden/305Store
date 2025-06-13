namespace _305.WebApi.Assistants.Middleware;

public class ApiKeyMiddleware
{
	private readonly RequestDelegate _next;
	private const string APIKEY_HEADER = "x-api-key";
	private readonly string _configuredApiKey;

	public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
	{
		_next = next;
		_configuredApiKey = configuration["ApiKeySettings:Key"];
	}

	public async Task InvokeAsync(HttpContext context)
	{
		if (!context.Request.Headers.TryGetValue(APIKEY_HEADER, out var extractedApiKey))
		{
			context.Response.StatusCode = 401;
			await context.Response.WriteAsync("API Key is missing");
			return;
		}

		if (!_configuredApiKey.Equals(extractedApiKey))
		{
			context.Response.StatusCode = 403;
			await context.Response.WriteAsync("Unauthorized client");
			return;
		}

		await _next(context);
	}
}

