
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilCommerce.Core.Services
{
	public interface IOrderNoteService : IGenericService<OrderNote,int>
    {
                 
        IQueryable<OrderNote> GetByCustomFilter( int? orderId);
           
		
    }
}      
 