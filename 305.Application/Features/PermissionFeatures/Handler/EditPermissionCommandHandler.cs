using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.Features.PermissionFeatures.Command;
using _305.Application.IBaseRepository;
using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.PermissionFeatures.Handler;


public class EditPermissionCommandHandler(IUnitOfWork unitOfWork, IRepository<Permission> repository)
    : IRequestHandler<EditPermissionCommand, ResponseDto<string>>
{
    private readonly EditHandler<EditPermissionCommand, Permission> _handler = new(unitOfWork, repository);
    // استفاده مستقیم از IUnitOfWork برای دادن Repository به هندلر

    public async Task<ResponseDto<string>> Handle(EditPermissionCommand request, CancellationToken cancellationToken)
    {
        var slug = request.slug ?? SlugHelper.GenerateSlug(request.name);

        var validations = new List<ValidationItem>
        {
           new ()
           {
               Rule = async () => await repository.ExistsAsync(x => x.name == request.name && x.id != request.id),
               Value = "نام"
           },
           new ()
           {
               Rule = async () => await repository.ExistsAsync(x => x.slug == slug && x.id != request.id),
               Value = "نامک"
           }
        };

        return await _handler.HandleAsync(
            id: request.id,
            validations: validations,
            updateEntity: entity =>
            {
                entity.name = request.name;
                entity.slug = slug;
                entity.updated_at = request.updated_at;
                return Task.FromResult(slug);
            },

            propertyName: "ارتباط نقش و کاربر",
            cancellationToken: cancellationToken
        );
    }
}