using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.AdminUserFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.AdminUserFeatures.Handler;

public class GetPaginatedAdminUserQueryHandler : IRequestHandler<GetPaginatedAdminUserQuery, ResponseDto<PaginatedList<User>>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly GetPaginatedHandler _handler;

	public GetPaginatedAdminUserQueryHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_handler = new GetPaginatedHandler(unitOfWork);
	}

	public Task<ResponseDto<PaginatedList<User>>> Handle(GetPaginatedAdminUserQuery request, CancellationToken cancellationToken)
	{
		var filter = new DefaultPaginationFilter
		{
			Page = request.Page,
			PageSize = request.PageSize,
			SearchTerm = request.SearchTerm,
			SortBy = request.SortBy,
		};

		return _handler.Handle(
			uow => uow.UserRepository.GetPagedResultAsync(
				filter,
				predicate: x=> x.is_active == request.is_active
			)
		);
	}
}