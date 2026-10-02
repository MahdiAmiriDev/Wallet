using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IProductTypeService : IGenericService<ProductType, int>
    {

        IQueryable<ProductType> GetByCustomFilter(int? parentId);
    }
}
