			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IApplicationCustomerClubRepository : IGenericRepository<ApplicationCustomerClub,int>
    {
		          
        IQueryable<ApplicationCustomerClub> GetByCustomFilter( int? customerClubId);
           
		
    }
}      
 