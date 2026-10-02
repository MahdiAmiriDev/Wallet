			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IContractPaymentMethodRepository : IGenericRepository<ContractPaymentMethod,int>
    {
		          
        IQueryable<ContractPaymentMethod> GetByCustomFilter( int? contractId, int? paymentMethodType);
           
		
    }
}      
 