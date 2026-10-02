
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IVocherService : IGenericService<Vocher, int>
    {

        IQueryable<Vocher> GetByCustomFilter(int? vocherTypeId);
        /// <summary>
        /// دریافت ووچر استفاده نشده بر اساس نوع ووچر
        /// </summary>
        /// <param name="vocherTypeId"></param>
        /// <returns></returns>
        Vocher GetNotUsedVocherByVocherTypeId(int vocherTypeId);
        /// <summary>
        /// دریافت ووچر بر اساس آیتم سفارش
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        Vocher GetByOrderItemId(int orderItemId);
        /// <summary>
        /// دریافت ووچر استفاده نشده بر اساس آیتم سفارش
        /// </summary>
        /// <param name="OrderItemId"></param>
        /// <returns></returns>
        Vocher GetNotUsedVocherByOrderItemId(int OrderItemId);
        void AssignVocherForOrderItems(int applicationId);
        /// <summary>
        /// درج کد کوپن رمز شده از روی جدول
        /// UploadedVoucher
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <param name="userId">شناسه کاربر لاگین کرده</param>
        /// <param name="companyId">شناسه مرکز</param>
        /// <param name="voucherTypeId">شناسه نوع کوپن</param>
        /// <returns></returns>
        Task<int> AddVoucherFromUploadedVoucher(int applicationId, int userId, int companyId, int voucherTypeId);
    }
}
