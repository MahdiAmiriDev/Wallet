using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface ICustomEcommerceRepository : IGenericRepository<Order, int>
    {
        /// <summary>
        /// سفارش هایی که بیش از زمان مشخصی از عدم پرداخت آنها گذشه است
        /// را به وضعیت انصراف سیستمی می برد
        /// </summary>
        Task CancelExpiredOrdersAsync();
    }
}
