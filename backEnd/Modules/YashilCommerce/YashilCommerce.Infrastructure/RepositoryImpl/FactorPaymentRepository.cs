using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class FactorPaymentRepository : GenericApplicationBasedRepository<FactorPayment, int>,
        IFactorPaymentRepository
    {
        public FactorPaymentRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
        }


        public IQueryable<FactorPayment> GetByCustomFilter(int factorId)
        {
            var query = GetAll(true);

            query = query.Where(x => x.FactorId == factorId);

            return query;
        }

        public IQueryable<FactorPayment> GetFactorPayments(int factorId)
        {
            return DbSet.Where(x => x.FactorId == factorId);
        }


    }
}