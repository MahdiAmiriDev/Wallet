
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
	public interface IRefundService : IGenericService<Refund,int>
    {
                 
        IQueryable<Refund> GetByCustomFilter( int? orderId);
    }
}      
 