using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.IBaseRepository;
using _305.Application.IUOW;
using _305.Domain.Common;
using Serilog;

namespace _305.Application.Base.Handler;

/// <summary>
/// هندلر عمومی برای ویرایش (Edit) موجودیت‌ها در لایه داده.
/// از الگوی Generic، Repository و Unit of Work برای به‌روزرسانی موجودیت‌ها استفاده می‌کند.
/// </summary>
/// <typeparam name="TCommand">نوع کامند (در صورت نیاز برای توسعه بیشتر)</typeparam>
/// <typeparam name="TEntity">نوع موجودیت که باید از <see cref="IBaseEntity"/> ارث‌بری کند</typeparam>
public class EditHandler<TCommand, TEntity>
    where TEntity : class, IBaseEntity
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<TEntity> _repository;
    private readonly ILogger _logger;

    /// <summary>
    /// سازنده کلاس با تزریق وابستگی‌ها
    /// </summary>
    public EditHandler(IUnitOfWork unitOfWork, IRepository<TEntity> repository, ILogger? logger = null)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _logger = logger ?? Log.ForContext<EditHandler<TCommand, TEntity>>();
    }

    /// <summary>
    /// اجرای عملیات ویرایش موجودیت.
    /// </summary>
    public async Task<ResponseDto<string>> HandleAsync(
        long id,
        List<ValidationItem>? validations,
        Func<TEntity, Task<string>> updateEntity,
        Func<TEntity, Task>? beforeUpdate = null,
        Func<TEntity, Task>? afterUpdate = null,
        string propertyName = "رکورد",
        CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.FindSingle(x => x.id == id);
            if (entity == null)
                return Responses.NotFound<string>(null, propertyName);

            if (validations != null)
            {
                foreach (var validation in validations)
                {
                    if (!await validation.Rule()) continue;
                    return validation.IsExistRole
                        ? Responses.Exist<string>(null, null, validation.Value)
                        : Responses.NotFound<string>(null, validation.Value);
                }
            }

            if (beforeUpdate is not null)
                await beforeUpdate(entity);

            var result = await updateEntity(entity);

            if (afterUpdate is not null)
                await afterUpdate(entity);

            var committed = await _unitOfWork.CommitAsync(cancellationToken);
            return !committed ? Responses.ExceptionFail(result, $"{propertyName} ویرایش نشد", 500) : Responses.Success(result, $"{propertyName} با موفقیت ویرایش شد");
        }
        catch (OperationCanceledException)
        {
            return ExceptionHandlers.CancellationException<string>(_logger);
        }
        catch (Exception ex)
        {
            return ExceptionHandlers.GeneralException<string>(ex, _logger);
        }
    }
}
