using System.Linq;
using System.Linq.Dynamic.Core;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;

using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class CompanyGroupRepository : GenericApplicationBasedRepository<CompanyGroup, int>, ICompanyGroupRepository
    {
        public CompanyGroupRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
        }


        public IQueryable<CompanyGroup> GetByCustomFilter(int? companyGroupId)
        {
            return DbSet.Where(x => !companyGroupId.HasValue ||
                               (companyGroupId.HasValue && companyGroupId.Value == x.Id)
                               );
        }
    }
}
