using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IFactorPaymentService : IGenericService<FactorPayment, int>
    {
        IQueryable<FactorPayment> GetByCustomFilter(int factorId);

        /// <summary>
        /// دریافت لیست پرداخت های یک فاکتور
        /// این متد زمانی استفاده میشود که بخواهیم لیست تمامی پرداخت های یک فاکتور گروه بندی شده را مشاهده کنیم.
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        IQueryable<FactorPayment> GetFactorPayments(int factorId);

        
    }
}