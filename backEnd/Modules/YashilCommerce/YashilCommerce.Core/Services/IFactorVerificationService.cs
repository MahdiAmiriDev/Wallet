using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IFactorVerificationService : IGenericService<FactorVerification, int>
    {

        IQueryable<FactorVerification> GetByCustomFilter(int? factorId, int? factorConfirmationType, int? factorVerificationConfigurationRoleId);


        IQueryable<FactorVerification> GetFactorVerificationsForFactorByCustomFilterForList(int factorId);
        /// <summary>
        ///  دریافت لیست تمامی توضیحات مربوط به بررسی فاکتور
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        IQueryable<FactorVerification> GetFactorVerificationDescriptionForFactor(int factorId);

        /// <summary>
        /// در این متد بررسی های فاکتور قبلی و بعدی بر اساس ترتیب آنها یعنی factorVerificationOrder چک میشود
        /// برای بررسی های فاکتور بعد باید همه آنها در وضعیت ثبت اولیه باشند
        /// و برای بررسیهای فاکتور قبلی باید همه آنها در وضعیت تاید شده باشند
        /// اگر هر یک از شرایط برقرار نباشد مقدار False برمیگرداند
        /// </summary>
        /// <param name="factorVerification"></param>
        /// <returns></returns>
        bool AllowUpdateFactorConfirmationType(FactorVerification factorVerification);
        /// <summary>
        /// بررسی میکند که آیا بررسی فاکتور دیگری از یک ترتیب مشخص به بعد وجود دارد یا خیر
        /// بررسی به این گونه است که ترتیب ورودی دریافت شده برای شناسه فاکتور داده شده
        /// بررسی های فاکتور بزرگتی وجود دارد یا خیر
        /// </summary>
        /// <param name="verificationOrder"></param>
        /// <param name="factorVerificationFactorId"></param>
        /// <returns></returns>
        bool CheckExistenceSubsequentRecord(int verificationOrder, int factorId);
        IQueryable<FactorVerification> GetForConfirm();
        /// <summary>
        /// بررسی فاکتور رد شده مربوط به یک فاکتور را برمیگرداند
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        FactorVerification GetRejectedFactorVerification(int factorId, bool readOnly = false);

        IQueryable<FactorVerification> GetFactorVerificationsByFactorId(int factorId, bool withHistory = true, bool readOnly = false);

        /// <summary>
        /// بررسی می کند که آیا فاکتور را قبلا کسی بررسی کرده است یا نه
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        Task<bool> CheckFactorHasAnyFactorVerificationAsync(int factorId);

        /// <summary>
        /// مقدار فیلد
        /// IsHistory
        /// رکوردهای بررسی این فاکتور را با قدار درست مقدار دهی می کنیم
        /// </summary>
        /// <param name="factorId"></param>
        /// <param name="saveAfterAddOrUpdate"></param>
        Task<int> ArchiveOldFactorVerificationsAsync(int factorId, bool saveAfterAddOrUpdate);

        /// <summary>
        /// بیشترین مقدار فیلد
        /// TryNumber
        /// را در صورت وجود رکورد بررسی فاکتور انتخاب شده را بر می گرداند
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns>در صورتی که مقدا بازگشتی صفر باشد یعنی رکورد آشیو بررسی فاکتوری برای فاکتور انتخاب شده وجود ندارد</returns>
        Task<int> GetMaxFactorVerificationTryNumberAsync(int factorId);
        /// <summary>
        /// وضعیت تایید آخرین تایید کننده به وضعیت ثبت اولیه تغییر کند
        /// </summary>
        /// <param name="factorId">شناسه فاکتور</param>
        /// <returns></returns>
        Task<FactorVerification> ChangeFactorLastVerifierConfirmTypeToInitialAsync(int factorId);
    }
}
