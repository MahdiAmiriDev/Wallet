using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IHekmatCanceledTransactionRepository : IGenericRepository<HekmatCanceledTransaction,int>
    {
        /// <summary>
        /// دریافت تراکنش عودت بر اساس refundDetailId
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        Task<HekmatCanceledTransaction> GetByRefundDetailIdAsync(int refundDetailId, bool readOnly = true);
    }
}      
 