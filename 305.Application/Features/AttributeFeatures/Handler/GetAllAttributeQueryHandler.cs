using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.AttributeFeatures.Query;
using _305.Application.Features.AttributeFeatures.Response;
using _305.Application.IUOW;
using MediatR;

namespace _305.Application.Features.AttributeFeatures.Handler
{
     public class GetAllProductCategoryQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllAttributeQuery, ResponseDto<List<AttributeResponse>>>
{
    private readonly GetAllHandler _handler = new();

    public Task<ResponseDto<List<AttributeResponse>>> Handle(GetAllAttributeQuery request, CancellationToken cancellationToken)
    {
        return _handler.HandleAsync<Domain.Entity.Attribute, AttributeResponse>(
            unitOfWork.AttributeRepository.FindListAsync(cancellationToken: cancellationToken)
        );
    }
}
}