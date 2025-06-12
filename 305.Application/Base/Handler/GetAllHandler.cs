using _305.Application.Base.Response;
using _305.Application.IUOW;
using Serilog;

namespace _305.Application.Base.Handler;

/// <summary>
/// هندلر عمومی برای تبدیل لیست موجودیت‌ها به DTO و بازگرداندن آن.
/// از AutoMapper برای نگاشت استفاده می‌کند.
/// </summary>
public class GetAllHandler
{
	private readonly ILogger _logger;

	/// <summary>
	/// سازنده کلاس که وابستگی به UnitOfWork را تزریق می‌کند.
	/// </summary>
	public GetAllHandler(ILogger? logger = null)
	{
		_logger = logger ?? Log.ForContext<GetAllHandler>();
	}

	/// <summary>
	/// دریافت لیست موجودیت‌ها و تبدیل آن‌ها به DTO
	/// </summary>
	public ResponseDto<List<TDto>> Handle<TEntity, TDto>(List<TEntity> entities)
		where TEntity : class
		where TDto : class, new()
	{
		try
		{
			var dtoList = entities
				.Select(Mapper.Mapper.Map<TEntity, TDto>)
				.ToList();

			return Responses.Data(dtoList, $"{dtoList.Count} آیتم دریافت شد");
		}
		catch (Exception ex)
		{
			_logger.Error(ex, "خطا در تبدیل لیست {Entity} به {Dto}: {Message}",
				typeof(TEntity).Name,
				typeof(TDto).Name,
				ex.Message);

			return Responses.ExceptionFail<List<TDto>>(null);
		}
	}
}