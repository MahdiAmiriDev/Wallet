
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilPayment.Core.Services
{
	public interface IPaymentGatewayPaymentMethodService : IGenericService<PaymentGatewayPaymentMethod,int>
    {
                 
        IQueryable<PaymentGatewayPaymentMethod> GetByCustomFilter( int? paymentGatewayId, int? paymentMethodType);
           
		
    }
}      
 