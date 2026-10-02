
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilCommerce.Core.Services
{
	public interface IOrderTrakkingService : IGenericService<OrderTrakking,int>
    {
                 
        IQueryable<OrderTrakking> GetByCustomFilter( int? referenceTo, int? orderId, int? orderItemId);
           
		
    }
}      
 