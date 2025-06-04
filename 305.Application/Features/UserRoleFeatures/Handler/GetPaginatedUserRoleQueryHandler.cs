using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.UserRoleFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace _305.Application.Features.UserRoleFeatures.Handler;

public class GetPaginatedUserRoleQueryHandler(IUnitOfWork unitOfWork)
	: IRequestHandler<GetPaginatedUserRoleQuery, ResponseDto<PaginatedList<UserRole>>>
{
	private readonly GetPaginatedHandler _handler = new(unitOfWork);

	public Task<ResponseDto<PaginatedList<UserRole>>> Handle(GetPaginatedUserRoleQuery request, CancellationToken cancellationToken)
	{
		var filter = new DefaultPaginationFilter
		{
			Page = request.Page,
			PageSize = request.PageSize,
			SearchTerm = request.SearchTerm,
			SortBy = request.SortBy
		};

		return _handler.Handle(
			uow => uow.UserRoleRepository.GetPagedResultAsync(
				filter,
				predicate: null,
				includeFunc: x=>x.Include(y => y.role).Include(y => y.user)
			)
		);
	}
}

