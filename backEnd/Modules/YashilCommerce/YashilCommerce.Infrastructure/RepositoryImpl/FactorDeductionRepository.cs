using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class FactorDeductionRepository : GenericApplicationBasedRepository<FactorDeduction, int>,
        IFactorDeductionRepository
    {
        public FactorDeductionRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
        }


        public IQueryable<FactorDeduction> GetByCustomFilter(int? factorId)
        {
            var query = GetAll(true);
            if (factorId.HasValue)
            {
                query = query.Where(x => x.FactorId == factorId.Value);
            }


            return query;
        }
    }
}