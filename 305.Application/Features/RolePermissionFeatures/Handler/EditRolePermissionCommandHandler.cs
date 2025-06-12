using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.Features.RolePermissionFeatures.Command;
using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.RolePermissionFeatures.Handler;
public class EditRolePermissionCommandHandler(IUnitOfWork unitOfWork, IBaseRepository.IRepository<RolePermission> repository)
    : IRequestHandler<EditRolePermissionCommand, ResponseDto<string>>
{
    private readonly EditHandler<EditRolePermissionCommand, RolePermission> _handler = new(unitOfWork, repository);
    // استفاده مستقیم از IUnitOfWork برای دادن Repository به هندلر

    public async Task<ResponseDto<string>> Handle(EditRolePermissionCommand request, CancellationToken cancellationToken)
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
            },
            new ()
            {
                Rule = async () => await repository.ExistsAsync(x => x.role_id == request.role_id && x.permission_id == request.permission_id &&  x.id != request.id),
                Value = "ارتباط نقش و دسترسی",
                IsExistRole = true
            },
            new ()
            {
                Rule = async () => !(await unitOfWork.PermissionRepository.ExistsAsync(x => x.id == request.permission_id)),
                Value = "دسترسی",
                IsExistRole = true
            },
            new ()
            {
                Rule = async () => !(await unitOfWork.RoleRepository.ExistsAsync(x => x.id == request.role_id)),
                Value = "نقش",
                IsExistRole = true
            },
        };

        return await _handler.HandleAsync(
            id: request.id,
            validations: validations,
            updateEntity: entity =>
            {
                entity.name = request.name;
                entity.slug = slug;
                entity.updated_at = request.updated_at;
                entity.permission_id = request.permission_id;
                entity.role_id = request.role_id;
                return Task.FromResult(slug);
            },

            propertyName: "ارتباط نقش و کاربر",
            cancellationToken: cancellationToken
        );
    }
}