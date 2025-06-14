using _305.Application.Base.Response;
using _305.Application.Base.Validator;
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
        catch (OperationCanceledException)
        {
            return ExceptionHandlers.CancellationException<List<TDto>>(_logger);
        }
        catch (Exception ex)
        {
            return ExceptionHandlers.GeneralException<List<TDto>>(ex, _logger);
        }
    }

    /// <summary>
    /// دریافت لیست موجودیت‌ها به صورت ناهمگام و تبدیل آن‌ها به DTO
    /// </summary>
    public async Task<ResponseDto<List<TDto>>> HandleAsync<TEntity, TDto>(Task<List<TEntity>> entitiesTask)
        where TEntity : class
        where TDto : class, new()
    {
        try
        {
            var entities = await entitiesTask;
            return Handle<TEntity, TDto>(entities);
        }
        catch (OperationCanceledException)
        {
            return ExceptionHandlers.CancellationException<List<TDto>>(_logger);
        }
        catch (Exception ex)
        {
            return ExceptionHandlers.GeneralException<List<TDto>>(ex, _logger);
        }
    }
}