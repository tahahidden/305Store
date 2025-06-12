using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.BlogCategoryFeatures.Command;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.BlogCategoryFeatures.Handler;

public class DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteCategoryCommand, ResponseDto<string>>
{
    private readonly DeleteHandler _handler = new(unitOfWork);

    public async Task<ResponseDto<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _handler.HandleAsync<BlogCategory, string>(
            findEntityAsync: () => unitOfWork.BlogCategoryRepository.FindSingle(x => x.id == request.id),
            onDeleteAsync: entity => unitOfWork.BlogCategoryRepository.Remove(entity),
            entityName: "دسته‌بندی",
            notFoundMessage: "دسته‌بندی پیدا نشد",
            successMessage: "دسته‌بندی با موفقیت حذف شد",
            cancellationToken: cancellationToken
        );
    }
}