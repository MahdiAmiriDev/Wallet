using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilPayment.Core.Services
{
	public interface IHekmatPaymentGatewayTransactionBulkRequestCategoryService : IGenericService<HekmatPaymentGatewayTransactionBulkRequestCategory, int>
    {
        IQueryable<HekmatPaymentGatewayTransactionBulkRequestCategory> GetByCustomFilter( int? parentId, int? yashilHekmatTerminalId);
        
    }
}
