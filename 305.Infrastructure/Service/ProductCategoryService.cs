using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.IService;
using _305.Application.IUOW;
using _305.Domain.Entity;

namespace _305.Infrastructure.Service
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddCategoryRelations(long productId, List<long> categoryIds)
        {
            var relations = new List<CategoryProductRelation>();
            foreach (var categoryId in categoryIds)
            {
                relations.Add(new CategoryProductRelation(
                    name: $"Relation-{productId}-{categoryId}",
                    slug: $"relation-{productId}-{categoryId}",
                    productId: productId,
                    productCategoryId: categoryId,
                    isRequired: true
                ));


            }
            await _unitOfWork.CategoryProductRelationRepository.AddRangeAsync(relations);
            await _unitOfWork.CommitAsync(new CancellationToken());
        }
    }
}