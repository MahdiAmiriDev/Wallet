using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface ICompanyGroupRepository : IGenericRepository<CompanyGroup,int>
    {
        IQueryable<CompanyGroup> GetByCustomFilter(int? companyGroupId);
    }
}      
 