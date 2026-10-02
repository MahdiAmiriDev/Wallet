			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IOrderTrakkingRepository : IGenericRepository<OrderTrakking,int>
    {
		          
        IQueryable<OrderTrakking> GetByCustomFilter( int? referenceTo, int? orderId, int? orderItemId);
           
		
    }
}      
 