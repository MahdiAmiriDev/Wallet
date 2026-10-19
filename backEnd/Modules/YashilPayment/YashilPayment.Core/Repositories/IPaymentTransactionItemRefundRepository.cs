using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilPayment.Core.Repositories
{
	public interface IPaymentTransactionItemRefundRepository : IGenericRepository<PaymentTransactionItemRefund,int>
    {
        IQueryable<PaymentTransactionItemRefund> GetByCustomFilter(int? refundPaymentTransactionItemRequestId);
    }
}      
 