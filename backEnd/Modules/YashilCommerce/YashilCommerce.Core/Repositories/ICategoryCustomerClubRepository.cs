			using System.Linq;
            using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface ICategoryCustomerClubRepository : IGenericRepository<CategoryCustomerClub,int>
    {
		          
        IQueryable<CategoryCustomerClub> GetByCustomFilter( int? customerClubId, int? categoryId, int? applicationId);

    }
}      
 