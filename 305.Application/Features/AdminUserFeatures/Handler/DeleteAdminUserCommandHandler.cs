using _305.Application.Features.AdminUserFeatures.Command;
using MediatR;

namespace _305.Application.Features.AdminUserFeatures.Handler;

public class DeleteAdminUserCommandHandler : IRequestHandler<DeleteAdminUserCommand, ResponseDto<string>>
{
	private readonly DeleteHandler _handler;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteAdminUserCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_handler = new DeleteHandler(unitOfWork);
	}

	public async Task<ResponseDto<string>> Handle(DeleteAdminUserCommand request, CancellationToken cancellationToken)
	{
		return await _handler.HandleAsync<User, string>(
			findEntity: () => _unitOfWork.UserRepository.FindSingle(x => x.id == request.id),
			onDelete: entity => _unitOfWork.UserRepository.Remove(entity),
			name: "کاربر ادمین",
			notFoundMessage: "کاربر ادمین پیدا نشد",
			successMessage: "کاربر ادمین با موفقیت حذف شد",
			cancellationToken: cancellationToken
		);
	}
}
