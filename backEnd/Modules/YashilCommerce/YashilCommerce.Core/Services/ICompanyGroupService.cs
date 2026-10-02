using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface ICompanyGroupService : IGenericService<CompanyGroup, int>
    {
        IQueryable<CompanyGroup> GetByCustomFilter(int? companyGroupId);
    }
}
