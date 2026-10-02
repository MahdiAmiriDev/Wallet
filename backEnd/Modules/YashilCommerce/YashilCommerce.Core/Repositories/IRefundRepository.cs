			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IRefundRepository : IGenericRepository<Refund,int>
    {
		          
        IQueryable<Refund> GetByCustomFilter( int? orderId);
           
		
    }
}      
 