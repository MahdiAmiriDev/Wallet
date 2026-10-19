using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilPayment.Core.Repositories;

namespace YashilPayment.Infrastructure.RepositoryImpl
{
    public class InstallmentRepository : GenericApplicationBasedRepository<Installment, int>, IInstallmentRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public InstallmentRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<Installment> GetByCustomFilter(int? paymentId, int? paymentStatus)
        {
            var query = GetAll(true);
            if (paymentId.HasValue)
            {
                query = query.Where(x => x.PaymentId == paymentId.Value);
            }
            if (paymentStatus.HasValue)
            {
                query = query.Where(x => x.PaymentStatus == paymentStatus.Value);
            }


            return query;
        }

        public IQueryable<Installment> GetInstallmentByPaymentId(int paymentId)
        {
            return DbSet.Where(x => x.PaymentId == paymentId);
        }

    }
}
