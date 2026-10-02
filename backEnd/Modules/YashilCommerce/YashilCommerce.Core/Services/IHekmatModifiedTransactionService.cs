using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
	public interface IHekmatModifiedTransactionService : IGenericService<HekmatModifiedTransaction,int>
    {
        /// <summary>
        /// دریافت تراکنش عودت بر اساس refundDetailId
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        Task<HekmatModifiedTransaction> GetByRefundDetailIdAsync(int refundDetailId, bool readOnly = true);

    }
}      
 