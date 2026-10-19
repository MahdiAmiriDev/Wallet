			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilPayment.Core.Repositories
{
	public interface ICreditOrCashPaymentRepository : IGenericRepository<CreditOrCashPayment,int>
    {
		          
        IQueryable<CreditOrCashPayment> GetByCustomFilter( int? cardId, int? merchantId, int? status);
           
		
    }
}      
 