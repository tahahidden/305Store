using _305.Application.Features.AdminUserFeatures.Command;
using MediatR;

namespace _305.Application.Features.AdminUserFeatures.Handler;

public class CreateAdminUserCommandHandler : IRequestHandler<CreateAdminUserCommand, ResponseDto<string>>
{
	private readonly CreateHandler _handler;
	private readonly IUnitOfWork _unitOfWork;

	public CreateAdminUserCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_handler = new CreateHandler(unitOfWork);
	}

	public async Task<ResponseDto<string>> Handle(CreateAdminUserCommand request, CancellationToken cancellationToken)
	{

		var slug = request.slug ?? SlugHelper.GenerateSlug(request.name);
		var validations = new List<ValidationItem>
		{
		   new ()
		   {
			   Rule = async () => await _unitOfWork.UserRepository.ExistsAsync(x => x.email == request.email),
			   Value = "ایمیل"
		   },
		   new ()
		   {
			   Rule = async () => await _unitOfWork.UserRepository.ExistsAsync(x => x.name == request.name),
			   Value = "نام کاربری"
		   },
		   new ()
		   {
			   Rule = async () => await _unitOfWork.UserRepository.ExistsAsync(x => x.slug == slug),
			   Value = "نامک"
		   }

		};


		return await _handler.HandleAsync(
		   validations: validations,
		   onCreate: async () =>
		   {
			   var entity = new User()
			   {
				   name = request.name,
				   email = request.email,
				   password_hash = PasswordHasher.Hash(request.password),
				   concurrency_stamp = StampGenerator.CreateSecurityStamp(32),
				   security_stamp = StampGenerator.CreateSecurityStamp(32),
				   created_at = DateTime.Now,
				   updated_at = DateTime.Now,
				   failed_login_count = 0,
				   is_active = true,
				   is_delete_able = true,
				   is_locked_out = false,
				   is_mobile_confirmed = true,
				   last_login_date_time = DateTime.Now,
				   lock_out_end_time = DateTime.Now,
				   mobile = "",
				   slug = slug,
			   };
			   await _unitOfWork.UserRepository.AddAsync(entity);
			   return entity.id.ToString();
		   },
		   createMessage: null,
		   cancellationToken: cancellationToken
	   );
	}
}


