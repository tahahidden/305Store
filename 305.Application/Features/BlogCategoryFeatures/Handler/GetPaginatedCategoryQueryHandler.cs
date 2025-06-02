using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.BlogCategoryFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.BlogCategoryFeatures.Handler;

public class GetPaginatedCategoryQueryHandler(IUnitOfWork unitOfWork)
	: IRequestHandler<GetPaginatedCategoryQuery, ResponseDto<PaginatedList<BlogCategory>>>
{
	private readonly GetPaginatedHandler _handler = new(unitOfWork);

	public async Task<ResponseDto<PaginatedList<BlogCategory>>> Handle(GetPaginatedCategoryQuery request, CancellationToken cancellationToken)
	{
		var filter = new DefaultPaginationFilter
		{
			Page = request.Page,
			PageSize = request.PageSize,
			SearchTerm = request.SearchTerm,
			SortBy = request.SortBy
		};

		return await _handler.Handle<BlogCategory>(
			uow => uow.BlogCategoryRepository.GetPagedResultAsync(
				filter,
				predicate: null
			)
		);
	}
}

