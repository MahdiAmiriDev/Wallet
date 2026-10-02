			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IOrderNoteRepository : IGenericRepository<OrderNote,int>
    {
		          
        IQueryable<OrderNote> GetByCustomFilter( int? orderId);
           
		
    }
}      
 