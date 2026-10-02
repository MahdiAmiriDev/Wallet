using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IFactorPaymentRepository : IGenericRepository<FactorPayment, int>
    {
        IQueryable<FactorPayment> GetByCustomFilter(int factorId);

        /// <summary>
        /// جهت دریافت لیست پرداخت های یک فاکتور گروه بندی شده
        /// این متد در فرم پرداخی های فاکتور کاربرد دارد
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        IQueryable<FactorPayment> GetFactorPayments(int factorId);
    }
}