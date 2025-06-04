using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.UserRoleFeatures.Query;
using _305.Application.Features.UserRoleFeatures.Response;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.UserRoleFeatures.Handler;

public class GetAllUserRoleQueryHandler(IUnitOfWork unitOfWork)
	: IRequestHandler<GetAllUserRoleQuery, ResponseDto<List<UserRoleResponse>>>
{
	private readonly GetAllHandler _handler = new(unitOfWork);

	public Task<ResponseDto<List<UserRoleResponse>>> Handle(GetAllUserRoleQuery request, CancellationToken cancellationToken)
	{
		return Task.FromResult(
			_handler.Handle<UserRole, UserRoleResponse>(
				unitOfWork.UserRoleRepository.FindList()
			)
		);
	}
}