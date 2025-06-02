using _305.Application.Features.AdminUserFeatures.Query;
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
			BoolFilter = request.is_active,
		};

		return _handler.Handle(
			uow => uow.UserRepository.GetPagedResultAsync(
				filter,
				predicate: null
			)
		);
	}
}