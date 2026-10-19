using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilPayment.Core.Repositories
{
	public interface IHekmatPaymentGatewayTransactionBulkRequestCategoryRepository : IGenericRepository<HekmatPaymentGatewayTransactionBulkRequestCategory,int>
    {
        IQueryable<HekmatPaymentGatewayTransactionBulkRequestCategory> GetByCustomFilter( int? parentId, int? yashilHekmatTerminalId);
    }
}      
 