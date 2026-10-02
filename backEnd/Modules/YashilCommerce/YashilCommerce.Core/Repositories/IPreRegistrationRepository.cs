			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IPreRegistrationRepository : IGenericRepository<PreRegistration,int>
    {
		          
        IQueryable<PreRegistration> GetByCustomFilter( int? userId, int? productSalesPlanningId);
           
		
    }
}      
 