using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.Features.AdminUserFeatures.Command;
using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.BuildingBlocks.Security;
using _305.Domain.Entity;
using _305.Domain.Events;
using Serilog;
using MediatR;

namespace _305.Application.Features.AdminUserFeatures.Handler;

public class CreateAdminUserCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateAdminUserCommand, ResponseDto<string>>
{
    private readonly CreateHandler _handler = new(unitOfWork);

    public async Task<ResponseDto<string>> Handle(CreateAdminUserCommand request, CancellationToken cancellationToken)
    {

        var slug = request.slug ?? SlugHelper.GenerateSlug(request.name);
        var validations = new List<ValidationItem>
        {
           new ()
           {
               Rule = async () => await unitOfWork.UserRepository.ExistsAsync(x => x.email == request.email),
               Value = "ایمیل"
           },
           new ()
           {
               Rule = async () => await unitOfWork.UserRepository.ExistsAsync(x => x.name == request.name),
               Value = "نام کاربری"
           },
           new ()
           {
               Rule = async () => await unitOfWork.UserRepository.ExistsAsync(x => x.slug == slug),
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
                   created_at = DateTime.UtcNow,
                   updated_at = DateTime.UtcNow,
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
               await unitOfWork.UserRepository.AddAsync(entity);
               entity.AddDomainEvent(new UserCreatedDomainEvent(entity));
               Log.Information("کاربر جدید با نامک {Slug} ایجاد شد", slug);
               return slug;
           },
           successMessage: null,
           cancellationToken: cancellationToken
       );
    }
}


