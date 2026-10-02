using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IUploadedFactorPaymentService : IGenericService<UploadedFactorPayment, int>
    {
        IQueryable<UploadedFactorPayment> GetByCustomFilter(int? appDocumentId);

        /// <summary>
        /// بارگذاری پرداختی های فاکتور بوسیله فایل اکسل
        /// در فایل اکسل کد فاکتور، تعداد اقساط،مبلغ وایزی، و تاریخ واریز وجود دارد
        /// با این متد فایل اکسل دریافت و در جدول uploadedFactorPayment ریخته میشود
        /// این متد در فرم بارگذاری پرداختی های فاکتور بوسیله اکسل کاربرد دارد
        /// </summary>
        /// <param name="factorPaymentFile"></param>
        /// <returns></returns>
        Task<bool> BulkInsertFactorPaymentsDataAsync(IFormFile factorPaymentFile);

        /// <summary>
        /// بررسی پارامترهای بارگذاری شده پرداختی های فاکتور بوسیله اکسل
        /// پس از آنکه پرداختی های فاکتور بوسیله اکسل در جدول uploadedFactorPayment بارگذاری شد
        /// بلاید ابتدا مقادیر را بررسی کنیم که درست هستند یا خیر
        /// برای مثال چک کنیم که مقدار کد فاکتور معتبر است یا خیر
        /// این متد در فرم بارگذاری پرداختی های فاکتور بوسیله اکسل کاربرد دارد
        /// توجه داشته باشید که این متد تنها فایلهایی که کاربر لاگین شده بارگذاری کرده است را بررسی میکند و به بقیه بارگذاری ها کاری ندارد
        /// </summary>
        /// <returns></returns>
        Task<int> ChecksInvalidUploadedFactorPaymentFilesForLoggedInUserAsync();

        /// <summary>
        /// متدی که در ایتدای بارگذاری فایل اکسل پرداختی های فاکتور صدا زده میشود و ابتدا
        /// رکورد هایی که قبلا کاربر لاگین شده در جدول uploadedfactorPayment بارگذاری کرده را پاک میکنیم
        /// </summary>
        /// <param name="userPrincipalId"></param>
        void DeleteByUserId(int userPrincipalId);
        /// <summary>
        /// فایلهایی که کاربر لاگین شده آپلود کرده و مشکلی ندارد را به
        /// جدول factorPayment منتقل میکند
        /// </summary>
        /// <returns></returns>
        Task<int> TransferDataFromUploadedFactorPaymentsToFactorPaymentForLoggedInUserAsync();
    }
}