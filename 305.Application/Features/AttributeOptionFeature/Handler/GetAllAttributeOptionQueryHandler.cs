using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.AttributeOptionFeature.Query;
using _305.Application.Features.AttributeOptionFeature.Response;
using _305.Application.IUOW;
using MediatR;

namespace _305.Application.Features.AttributeOptionFeature.Handler
{

    public class GetAllAttributeOptionQueryHandler(IUnitOfWork unitOfWork)
: IRequestHandler<GetAllAttributeOptionQuery, ResponseDto<List<AttributeOptionResponse>>>
    {
        private readonly GetAllHandler _handler = new();

        public Task<ResponseDto<List<AttributeOptionResponse>>> Handle(GetAllAttributeOptionQuery request, CancellationToken cancellationToken)
        {
            return _handler.HandleAsync<Domain.Entity.AttributeOption, AttributeOptionResponse>(
                unitOfWork.AttributeOptionRepository.FindListAsync(cancellationToken: cancellationToken)
            );
        }
    }
}