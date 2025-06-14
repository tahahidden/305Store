using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.BlogCategoryFeatures.Query;
using _305.Application.Features.BlogCategoryFeatures.Response;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.BlogCategoryFeatures.Handler;

public class GetAllCategoryQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllCategoryQuery, ResponseDto<List<BlogCategoryResponse>>>
{
    private readonly GetAllHandler _handler = new();

    public Task<ResponseDto<List<BlogCategoryResponse>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        return _handler.HandleAsync<BlogCategory, BlogCategoryResponse>(
            unitOfWork.BlogCategoryRepository.FindListAsync(cancellationToken: cancellationToken)
        );
    }
}