using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.BlogFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace _305.Application.Features.BlogFeatures.Handler;

public class GetPaginatedBlogQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetPaginatedBlogQuery, ResponseDto<PaginatedList<Blog>>>
{
    private readonly GetPaginatedHandler _handler = new(unitOfWork);

    public Task<ResponseDto<PaginatedList<Blog>>> Handle(GetPaginatedBlogQuery request, CancellationToken cancellationToken)
    {
        var filter = new DefaultPaginationFilter
        {
            Page = request.Page,
            PageSize = request.PageSize,
            SearchTerm = request.SearchTerm,
            SortBy = request.SortBy
        };

        return _handler.Handle<Blog>(
            uow => uow.BlogRepository.GetPagedResultAsync(
                filter,
                predicate: null,
                includeFunc: q => q.Include(x => x.blog_category)
            )
        );
    }
}