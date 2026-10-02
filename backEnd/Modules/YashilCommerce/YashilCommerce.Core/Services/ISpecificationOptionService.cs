using System.Collections.Generic;
using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface ISpecificationOptionService : IGenericService<SpecificationOption, int>
    {
        IQueryable<SpecificationOption> GetByCustomFilter(int? specificationId);


        IQueryable<SpecificationOption> GetByCategoryIds(List<int> editModelCategories);
    }
}