using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class VocherTypeRepository : GenericApplicationBasedRepository<VocherType, int>, IVocherTypeRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public VocherTypeRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<VocherType> GetByCustomFilter(int? companyId, bool readOnly = false)
        {
            var query = GetAll(readOnly);
            if (companyId.HasValue)
            {
                query = query.Where(x => x.CompanyId == companyId.Value);
            }

            return query;
        }

        /// <summary>
        /// انواع ووچر های مربوط به یک مرکز را بر می گرداند
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        public IQueryable<VocherType> GetVocherTypesByCompanyId(int companyId)
        {
            return GetAll().Where(vt => vt.CompanyId == companyId);
        }
    }
}