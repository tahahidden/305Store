using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _305.Application.IService
{
    public interface IProductCategoryService
    {
        Task AddCategoryRelations(long productId, List<long> categoryIds);
    }
}