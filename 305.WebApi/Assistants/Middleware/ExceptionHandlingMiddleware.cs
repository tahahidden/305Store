using Azure;
using System.Text.Json;
using _305.Application.Base.Response;
using Serilog;

namespace _305.WebApi.Assistants.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
	public async Task Invoke(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Unhandled exception occurred");

			context.Response.StatusCode = 500;
			context.Response.ContentType = "application/json";

			var problem = new ResponseDto<string>
			{
				message = "خطای غیرمنتظره‌ای رخ داد.",
				response_code = 500,
				data = "خطای غیرمنتظره‌ای رخ داد.",
				is_success = false
			};

			var json = JsonSerializer.Serialize(problem);
			await context.Response.WriteAsync(json);
		}
	}
}

