using _305.Application.Features.AdminUserFeatures.Command;
using MediatR;

namespace _305.Application.Features.AdminUserFeatures.Handler;

public class EditAdminUserCommandHandler : IRequestHandler<EditAdminUserCommand, ResponseDto<string>>
{
	private readonly EditHandler<EditAdminUserCommand, User> _handler;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IRepository<User> _repository;
	public EditAdminUserCommandHandler(IUnitOfWork unitOfWork, IRepository<User> repository)
	{
		_unitOfWork = unitOfWork;
		_handler = new EditHandler<EditAdminUserCommand, User>(unitOfWork, repository);
		_repository = repository;
	}

	public async Task<ResponseDto<string>> Handle(EditAdminUserCommand request, CancellationToken cancellationToken)
	{

		var slug = request.slug ?? SlugHelper.GenerateSlug(request.name);

		var validations = new List<ValidationItem>
		{
		   new ()
		   {
			   Rule = async () => await _repository.ExistsAsync(x => x.name == request.name && x.id != request.id),
			   Value = "نام"
		   },
		   new ()
		   {
			   Rule = async () => await _repository.ExistsAsync(x => x.email == request.email && x.id != request.id),
			   Value = "ایمیل"
		   },
		   new ()
		   {
			   Rule = async () => await _repository.ExistsAsync(x => x.slug == slug && x.id != request.id),
			   Value = "نامک"
		   }
		};

		return await _handler.HandleAsync(
			id: request.id,
			validations: validations,
			propertyName: "کاربر ادمین",
			updateEntity: async entity =>
			{
				entity.name = request.name;
				entity.slug = slug;
				entity.updated_at = request.updated_at;
				entity.email = request.email;
				entity.password_hash = request.password == null ? entity.password_hash : PasswordHasher.Hash(request.password);
				return slug;
			},
			cancellationToken: cancellationToken
		);
	}
}

