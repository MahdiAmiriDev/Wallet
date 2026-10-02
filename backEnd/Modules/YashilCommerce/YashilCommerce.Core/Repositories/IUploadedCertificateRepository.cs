using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IUploadedCertificateRepository : IGenericRepository<UploadedCertificate, int>
    {
        /// <summary>
        /// فایل بارگذاری را اعتبار سنجی می کند و تعداد خطا ها را بر می گرداند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        Task<int> GetInvalidUploadedCertificateCountAsync(int userId);

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
        Task<int> AddCertificateFromUploadedCertificate(int userId);

        /// <summary>
        /// دریافت
        /// uploadedCertificate
        /// بر اساس شناسه کاربری
        /// </summary>
        /// <param name="userId">شناسه کاربری</param>
        /// <param name="showInvalidItem">ایتم های نامعتبر نیز آورده شود؟</param>
        /// <returns></returns>
        IQueryable<UploadedCertificate> GetByUserId(int userId, bool showInvalidItem);


        /// <summary>
        /// این متد جهت حذف کردن تمامی
        /// رکورد های مربوط به جدول
        /// UploadedCertificate
        /// که توسط کاربری که شناسه آن ارسال شده است کاربرد دارد.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteByUserIdAsync(int userId);
    }
}
