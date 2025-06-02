using _305.Application.Features.AdminUserFeatures.Query;
using _305.Application.Features.AdminUserFeatures.Response;
using MediatR;

namespace _305.Application.Features.AdminUserFeatures.Handler;

public class GetAdminUserBySlugQueryHandler : IRequestHandler<GetAdminUserBySlugQuery, ResponseDto<AdminUserResponse>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly GetBySlugHandler _handler;

	public GetAdminUserBySlugQueryHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_handler = new GetBySlugHandler(unitOfWork);
	}

	public async Task<ResponseDto<AdminUserResponse>> Handle(GetAdminUserBySlugQuery request, CancellationToken cancellationToken)
	{
		return await _handler.Handle<User, AdminUserResponse>(
			async uow => await uow.UserRepository.FindSingle(x => x.slug == request.slug),
			"کاربر ادمین",
			null
		);
	}
}