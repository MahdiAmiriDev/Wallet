using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IVocherRepository : IGenericRepository<Vocher, int>
    {

        IQueryable<Vocher> GetByCustomFilter(int? vocherTypeId);
        /// <summary>
        /// دریافت ووچر استفاده نشده بر اساس نوع ووچر
        /// </summary>
        /// <param name="vocherTypeId"></param>
        /// <returns></returns>
        Vocher GetNotUsedVoucherByVoucherTypeId(int vocherTypeId, int applicationId);
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
        Vocher GetNotUsedVoucherByOrderItemId(int orderItemId);
        void AssignVoucherForOrderItems(int applicationId);
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
        /// <summary>
        /// حذف کوپن های تخصیص نیافته یک نوع مشخص 
        /// </summary>
        /// <param name="vocherTypeId"></param>
        /// <returns></returns>
        Task<int> DeleteNotAssignedVochersAsync(int vocherTypeId);
    }
}
