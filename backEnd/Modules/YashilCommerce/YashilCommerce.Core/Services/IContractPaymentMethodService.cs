
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilCommerce.Core.Services
{
	public interface IContractPaymentMethodService : IGenericService<ContractPaymentMethod,int>
    {
                 
        IQueryable<ContractPaymentMethod> GetByCustomFilter( int? contractId, int? paymentMethodType);
           
		
    }
}      
 