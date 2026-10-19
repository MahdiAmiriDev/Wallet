
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilPayment.Core.Services
{
	public interface ICreditOrCashPaymentService : IGenericService<CreditOrCashPayment,int>
    {
                 
        IQueryable<CreditOrCashPayment> GetByCustomFilter( int? cardId, int? merchantId, int? status);
           
		
    }
}      
 