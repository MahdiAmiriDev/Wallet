using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IHekmatEffectlessModifiedTransactionRepository : IGenericRepository<HekmatEffectlessModifiedTransaction, int>
    {
        /// <summary>
        /// برای دریافت تراکنش عودت براساس refundDetailId
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        Task<HekmatEffectlessModifiedTransaction> GetByRefundDetailIdAsync(int refundDetailId, bool readOnly = true);
    }
}
