using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Filters.Pagination;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using _305.Application.Features.RoleFeatures.Query;

namespace _305.Application.Features.RoleFeatures.Handler;
public class GetPaginatedRoleQueryHandler(IUnitOfWork unitOfWork)
	: IRequestHandler<GetPaginatedRoleQuery, ResponseDto<PaginatedList<Role>>>
{
	private readonly GetPaginatedHandler _handler = new(unitOfWork);

	public Task<ResponseDto<PaginatedList<Role>>> Handle(GetPaginatedRoleQuery request, CancellationToken cancellationToken)
	{
		var filter = new DefaultPaginationFilter
		{
			Page = request.Page,
			PageSize = request.PageSize,
			SearchTerm = request.SearchTerm,
			SortBy = request.SortBy
		};

		return _handler.Handle(
			uow => uow.RoleRepository.GetPagedResultAsync(
				filter,
				predicate: null,
				includeFunc: null
			)
		);
	}
}