using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.PermissionFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace _305.Application.Features.PermissionFeatures.Handler;

public class GetPaginatedPermissionQueryHandler(IUnitOfWork unitOfWork)
	: IRequestHandler<GetPaginatedPermissionQuery, ResponseDto<PaginatedList<Permission>>>
{
	private readonly GetPaginatedHandler _handler = new(unitOfWork);

	public Task<ResponseDto<PaginatedList<Permission>>> Handle(GetPaginatedPermissionQuery request, CancellationToken cancellationToken)
	{
		var filter = new DefaultPaginationFilter
		{
			Page = request.Page,
			PageSize = request.PageSize,
			SearchTerm = request.SearchTerm,
			SortBy = request.SortBy
		};

		return _handler.Handle(
			uow => uow.PermissionRepository.GetPagedResultAsync(
				filter,
				predicate: null,
				includeFunc: null
			)
		);
	}
}

