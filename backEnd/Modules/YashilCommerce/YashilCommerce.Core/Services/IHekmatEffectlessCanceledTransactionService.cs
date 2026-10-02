using System.Threading.Tasks;
            using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
	public interface IHekmatEffectlessCanceledTransactionService : IGenericService<HekmatEffectlessCanceledTransaction,int>
    {
        /// <summary>
        /// جهت دریافت تراکنش عودت بوسیله refundDetailId
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        Task<HekmatEffectlessCanceledTransaction> GetByRefundDetailIdAsync(int refundDetailId, bool readOnly = true);
    }
}      
 