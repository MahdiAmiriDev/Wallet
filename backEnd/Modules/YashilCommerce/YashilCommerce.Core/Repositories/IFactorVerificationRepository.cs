using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IFactorVerificationRepository : IGenericRepository<FactorVerification, int>
    {
        IQueryable<FactorVerification> GetByCustomFilter(int? factorId, int? factorConfirmationType,
            int? factorVerificationConfigurationRoleId);

        IQueryable<FactorVerification> GetFactorVerificationsForFactorByCustomFilterForList(int factorId);


        /// <summary>
        /// برای ترتیب فاکتور داده شده آیا بررسی فاکتور با ترتیب بزرگتری وجود دارد یا خیر
        /// </summary>
        /// <param name="verificationOrder"></param>
        /// <param name="factorId"></param>
        /// <returns></returns>
        bool CheckExistenceSubsequentRecord(int verificationOrder, int factorId);

        IQueryable<FactorVerification> GetFactorVerificationDescriptionForFactor(int factorId);

        /// <summary>
        /// بررسی اجازه بروزرسانی بررسی فاکتور بر اساس نوبت
        /// در این متد بررسی های فاکتور قبلی و بعدی بر اساس ترتیب آنها یعنی factorVerificationOrder چک میشود
        /// برای بررسی های فاکتور بعد باید همه آنها در وضعیت ثبت اولیه باشند
        /// و برای بررسیهای فاکتور قبلی باید همه آنها در وضعیت تاید شده باشند
        /// اگر هر یک از شرایط برقرار نباشد مقدار False برمیگرداند
        /// 
        /// </summary>
        /// <param name="factorVerification"></param>
        /// <returns></returns>
        bool AllowUpdateFactorConfirmationType(FactorVerification factorVerification);

        /// <summary>
        /// لیست بررسی های فاکتور برای کاربر لاگین شده
        /// </summary>
        /// <returns></returns>
        IQueryable<FactorVerification> GetForConfirm();

        /// <summary>
        /// بررسی فاکتور رد شده مربوط به یک فاکتور را برمیگرداند
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        FactorVerification GetRejectedFactorVerification(int factorId, bool readOnly = false);

        /// <summary>
        /// دریافت لیست بررسی های یک فاکتور بوسیله ایدی ان فاکتور
        /// </summary>
        /// <param name="factorId"></param>
        /// <param name="withHistory">آیا تاریخچه های بررسی فاکتور هم دریافت کند یا خیر</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<FactorVerification> GetFactorVerificationsByFactorId(int factorId, bool withHistory = true, bool readOnly = false);

        Task<bool> CheckFactorHasAnyFactorVerificationAsync(int factorId);
        Task<int> ArchiveOldFactorVerificationsAsync(int factorId, bool saveAfterAddOrUpdate);
        Task<int> GetMaxFactorVerificationTryNumberAsync(int factorId);
        
    }
}