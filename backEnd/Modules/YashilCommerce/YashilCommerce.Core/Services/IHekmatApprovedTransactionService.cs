using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IHekmatApprovedTransactionService : IGenericService<HekmatApprovedTransaction, int>
    {
        Task DeleteDuplicateHekmatApprovedTransactionsAsync();
        /// <summary>
        /// به روز رسانی تراکنش های حکمتی که اصلاح قیمت برای آنها رخ داده است
        /// </summary>
        /// <returns></returns>
        Task UpdateModifiedHekmatTransactionAsync();
    }
}
