
using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
	public interface ICustomerClubPaymentMethodTypeService : IGenericService<CustomerClubPaymentMethodType,int>
    {
                 
        IQueryable<CustomerClubPaymentMethodType> GetByCustomFilter( int? paymentMethodType, int? customerClubId);
        IQueryable<PaymentMethodType> GetByUserId(int id);
        IQueryable<PaymentMethodType> GetByCustomerClubId(int customerClubId);
    }
}      
 