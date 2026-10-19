using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilPayment.Core.Repositories;

namespace YashilPayment.Infrastructure.RepositoryImpl
{
    public class CreditOrCashPaymentRepository : GenericApplicationBasedRepository<CreditOrCashPayment, int>, ICreditOrCashPaymentRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public CreditOrCashPaymentRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<CreditOrCashPayment> GetByCustomFilter(int? cardId, int? merchantId, int? status)
        {
            var query = GetAll(true);
            //         if ( cardId.HasValue)
            //{
            //    query = query.Where(x => x.CardId == cardId.Value);
            //}
            //          if ( merchantId.HasValue)
            //{
            //    query = query.Where(x => x.MerchantId == merchantId.Value);
            //}
            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }


            return query;
        }



    }
}
