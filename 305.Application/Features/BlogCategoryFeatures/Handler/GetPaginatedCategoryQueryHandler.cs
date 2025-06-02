using _305.Application.Features.BlogCategoryFeatures.Query;
using Core.EntityFramework.Models;
using Core.Pagination;
using DataLayer.Base.Handler;
using DataLayer.Base.Response;
using DataLayer.Repository;
using MediatR;

namespace _305.Application.Features.BlogCategoryFeatures.Handler;

public class GetPaginatedCategoryQueryHandler : IRequestHandler<GetPaginatedCategoryQuery, ResponseDto<PaginatedList<BlogCategory>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetPaginatedHandler _handler;

    public GetPaginatedCategoryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _handler = new GetPaginatedHandler(unitOfWork);
    }

    public Task<ResponseDto<PaginatedList<BlogCategory>>> Handle(GetPaginatedCategoryQuery request, CancellationToken cancellationToken)
    {
        var filter = new DefaultPaginationFilter
        {
            Page = request.Page,
            PageSize = request.PageSize,
            SearchTerm = request.SearchTerm,
            SortBy = request.SortBy
        };

        return _handler.Handle(
            uow => uow.BlogCategoryRepository.GetPagedResultAsync(
                filter,
                predicate:null,
                includes: Array.Empty<string>()
            )
        );
    }
}

