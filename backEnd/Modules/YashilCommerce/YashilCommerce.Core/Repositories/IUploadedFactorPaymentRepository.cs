using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IUploadedFactorPaymentRepository : IGenericRepository<UploadedFactorPayment, int>
    {
        IQueryable<UploadedFactorPayment> GetByCustomFilter(int? appDocumentId);

        /// <summary>
        /// بررسی پارامترهای بارگذاری شده پرداختی های فاکتور بوسیله اکسل
        /// پس از آنکه پرداختی های فاکتور بوسیله اکسل در جدول uploadedFactorPayment بارگذاری شد
        /// بلاید ابتدا مقادیر را بررسی کنیم که درست هستند یا خیر
        /// برای مثال چک کنیم که مقدار کد فاکتور معتبر است یا خیر
        /// این متد در فرم بارگذاری پرداختی های فاکتور بوسیله اکسل کاربرد دارد
        /// </summary>
        /// <returns></returns>
        Task<int> ChecksInvalidUploadedFactorPaymentFilesForLoggedInUser();
        /// <summary>
        /// بر اساس یوزری که به ان پاس داده شده است اطلاعاتی که در جدول uploadedfactorPaymetn وجود دارد را بارگذاری مکیند
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<UploadedFactorPayment> GetAllUploadedFactorPaymentsUploadedByUser(int userId);
        /// <summary>
        /// فایلهایی که کاربر لاگین شده آپلود کرده و مشکلی ندارد را به
        /// جدول factorPayment منتقل میکند
        /// </summary>
        /// <returns></returns>
        Task<int> TransferDataFromUploadedFactorPaymentsToFactorPaymentForLoggedInUserAsync();
    }
}