using System.Collections.Generic;
using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface ISpecificationOptionRepository : IGenericRepository<SpecificationOption, int>
    {
        IQueryable<SpecificationOption> GetByCustomFilter(int? specificationId);

        IQueryable<SpecificationOption> GetByCategoryIds(List<int> editModelCategories);
    }
}