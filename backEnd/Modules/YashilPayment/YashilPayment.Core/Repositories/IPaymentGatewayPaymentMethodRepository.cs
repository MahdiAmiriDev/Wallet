			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilPayment.Core.Repositories
{
	public interface IPaymentGatewayPaymentMethodRepository : IGenericRepository<PaymentGatewayPaymentMethod,int>
    {
		          
        IQueryable<PaymentGatewayPaymentMethod> GetByCustomFilter( int? paymentGatewayId, int? paymentMethodType);
           
		
    }
}      
 