using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.RolePermissionFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace _305.Application.Features.RolePermissionFeatures.Handler;
public class GetPaginatedRolePermissionQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetPaginatedRolePermissionQuery, ResponseDto<PaginatedList<RolePermission>>>
{
    private readonly GetPaginatedHandler _handler = new(unitOfWork);

    public Task<ResponseDto<PaginatedList<RolePermission>>> Handle(GetPaginatedRolePermissionQuery request, CancellationToken cancellationToken)
    {
        var filter = new DefaultPaginationFilter
        {
            Page = request.Page,
            PageSize = request.PageSize,
            SearchTerm = request.SearchTerm,
            SortBy = request.SortBy
        };

        return _handler.Handle(
            uow => uow.RolePermissionRepository.GetPagedResultAsync(
                filter,
                predicate: null,
                includeFunc: x => x.Include(y => y.role).Include(y => y.permission)
            )
        );
    }
}

