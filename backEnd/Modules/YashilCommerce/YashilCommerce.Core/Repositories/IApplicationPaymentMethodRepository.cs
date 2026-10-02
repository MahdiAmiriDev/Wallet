			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IApplicationPaymentMethodRepository : IGenericRepository<ApplicationPaymentMethod,int>
    {
		          
        IQueryable<ApplicationPaymentMethod> GetByCustomFilter( int? paymentMethodType);
        IQueryable<PaymentMethodType> GetApplicationPaymentMethods(int applicationId, bool readOnly);
    }
}      
 