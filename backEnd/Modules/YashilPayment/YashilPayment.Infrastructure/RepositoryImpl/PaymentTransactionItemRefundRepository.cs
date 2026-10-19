using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilPayment.Core.Repositories;

namespace YashilPayment.Infrastructure.RepositoryImpl
{
    
    public class PaymentTransactionItemRefundRepository : GenericApplicationBasedRepository<PaymentTransactionItemRefund, int>, IPaymentTransactionItemRefundRepository
    {
        public PaymentTransactionItemRefundRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
        {
        }
        public IQueryable<PaymentTransactionItemRefund> GetByCustomFilter(int? refundPaymentTransactionItemRequestId)
        {
            var query = GetAll(true);
            if(refundPaymentTransactionItemRequestId.HasValue)
            {
                query = query.Where(x => x.RefundPaymentTransactionItemRequestId == refundPaymentTransactionItemRequestId.Value);
            }
            return query;
        }
         
    }
}      
