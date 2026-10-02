using System.Threading.Tasks;
            using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IHekmatEffectlessCanceledTransactionRepository : IGenericRepository<HekmatEffectlessCanceledTransaction,int>
    {
        /// <summary>
        /// دریافت تراکنش عودت بوسیله refundDetailId
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        Task<HekmatEffectlessCanceledTransaction> GetByRefundDetailIdAsync(int refundDetailId, bool readOnly);
    }
}      
 