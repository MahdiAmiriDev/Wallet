using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
	public interface IUploadedCertificateService : IGenericService<UploadedCertificate, int>
    {
        /// <summary>
        /// این متد جهت حذف کردن تمامی
        /// رکورد های مربوط به جدول
        /// UploadedCertificate
        /// که توسط کاربری که شناسه آن ارسال شده است کاربرد دارد.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteByUserIdAsync(int userId);

        /// <summary>
        /// این متد جهت درج اطلاعات گواهینامه ها در
        /// دیتابیس به صورت
        /// bulk
        /// کاربرد دارد
        /// </summary>
        /// <param name="tempCertificateFile"></param>
        /// <returns></returns>
        Task<ResultStatus<int>> BulkInsertFromExcelAsync(IFormFile tempCertificateFile);

        /// <summary>
        /// فایل بارگذاری را اعتبار سنجی می کند و تعداد خطا ها را بر می گرداند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        Task<int> GetInvalidUploadedCertificateCountAsync(int userId);

        /// <summary>
        /// این متد داده ستون های که نیاز است
        /// رمزنگاری شوند را
        /// hash
        /// می کند
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> HashRequiredColumnsAsync(int userId);

        /// <summary>
        /// این متد جهت درج گواهینامه ها
        /// از جدول
        /// uploadedCertificate
        /// به جدول
        /// Certificate
        /// کاربرد دارد
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> AddCertificateFromUploadedCertificateAsync(int userId);

        /// <summary>
        /// این متد بررسی می کند که آیا در جدول
        /// uploadedCertificate
        /// گواهینامه ای توسط کاربر درج شده است یا نه؟
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> HasUserUploadAnyCertificateAsync(int userId);

        /// <summary>
        /// دانلود نمونه اکسل بارگذاری گواهینامه
        /// </summary>
        /// <returns></returns>
        MemoryStream CopySampleCertificateFileToWoorkbook();

        /// <summary>
        /// دریافت
        /// uploadedCertificate
        /// بر اساس شناسه کاربری
        /// </summary>
        /// <param name="userId">شناسه کاربری</param>
        /// <param name="showInvalidItem">ایتم های نامعتبر نیز آورده شود؟</param>
        /// <returns></returns>
        IQueryable<UploadedCertificate> GetByUserId(int userId, bool showInvalidItem);
    }
}
