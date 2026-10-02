using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IFactorService : IGenericService<Factor, int>
    {

        IQueryable<Factor> GetByCustomFilter(int? taxCategoryId, int? companyId, int? representationId, int? factorReviewStatusType);


        /// <summary>
        /// بررسی میکند که برای کاربر با companyId و RepresentationId
        /// فاکتور تایید نشده ای وجود دارد یا خیر
        /// </summary>
        /// <returns></returns>
        bool CheckExistenceUnconfirmedFactor(int companyId, int? representationId);
        /// <summary>
        /// جهت ثبت نهایی فاکتور
        /// در این متد با بررسی شروط اجازه ثبت فاکتور نظیر
        /// اپلود شدن فایل فاکتور و وضعیت فاکتور
        /// مبتنی بر قرارداد بودن و قرار داد فعال داشتن
        /// وجود تنظیم بررسی فاکتور
        /// و درصورت برقرار بودن این شروط به ازای تنظیم فاکتور موجود
        /// بررسی های فاکتور برای فاکتور در جدول factor verification با وضعیت ثبت اولیه ثبت میشود
        /// و همچنین خود فاکتور در حالت در حال بررسی بروزرسانی میشود تا کاربر تامین کننده اجازه ویرایش مجدد آن را نداشته باشد
        /// در نهایت نیز کد فاکتور برای فاکتور نیز ایجاد میشود که این کد فاکتور بر اساس کد مرکز و کد نمایندگی و آیدی فاکتور و و تاریخ ایجاد میشود.
        /// </summary>
        /// <param name="factorId">کد فاکتور</param>
        /// <param name="saveAfterAddOrUpdate">ذخیره بعد از اضافه یا بروزرسانی</param>
        /// <returns></returns>
        Task<string> FinalFactorRegistration(int factorId, bool saveAfterAddOrUpdate = false);

        /// <summary>
        /// متد فیلتر نمایش در فرم نمایشی با مجوز حذف
        /// این متد در فرم فاکتوری کاربرد دارد که تنها اجازه حذف در آن وجود دارد
        /// </summary>
        /// <param name="taxCategoryId"></param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="factorReviewStatusType"></param>
        /// <returns></returns>
        IQueryable<Factor> GetByCustomFilterForDelete(int? taxCategoryId, int? companyId, int? representationId,
            int? factorReviewStatusType);

        /// <summary>
        /// تایید بررسی فاکتور
        /// این متد هنگامی فراخوانی میشود که کاربر دارای نقش بررسی فاکتور بخواهد فاکتور را تایید کند
        /// هنگامی بررسی فاکتوری تایید میشود که بررسی های فاکتور قبلی مربوط به آن فاکتور در صورت موجود بودن تایید شده باشند
        /// و بررسی های فاکتور بعدی آن در صورت موجود بودن در حالت ثبت اولیه باشند
        /// </summary>
        /// <param name="factorVerificationId">آیدی بررسی فاکتور</param>
        /// <param name="saveAfterAddOrUpdate"></param>
        /// <returns></returns>
        Task<FactorVerification> ConfirmFactorVerification(int factorVerificationId, bool saveAfterAddOrUpdate = false);

        /// <summary>
        /// این متد هنگامی فراخوانی میشود که کاربر دارای نقش بررسی فاکتور بخواهد فاکتور را رد کند
        /// هنگامی بررسی فاکتوری رد میشود که بررسی های فاکتور قبلی مربوط به آن فاکتور در صورت موجود بودن تایید شده باشند
        /// و بررسی های فاکتور بعدی آن در صورت موجود بودن در حالت ثبت اولیه باشند
        /// </summary>
        /// <param name="factorVerification"></param>
        /// <param name="saveAfterAddOrUpdate"></param>
        /// <returns></returns>
        Task<string> RejectFactorVerification(FactorVerification factorVerification,
            bool saveAfterAddOrUpdate = false);

        /// <summary>
        /// برای نمایش لیست همه فاکتور ها بر اساس فیلتر مرکز و نمایندگی
        /// اگر companyId و representationId خالی بودند باید همه فاکتور ها را نمایش دهد
        /// اگر 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="taxCategoryId"></param>
        /// <param name="factorReviewStatusType"></param>
        /// <returns></returns>
        IQueryable<Factor> GetSupervisorFactorList(int? companyId, int? representationId, int? taxCategoryId, int? factorReviewStatusType);

        /// <summary>
        /// برای ایجاد فاکتور
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="productSalePlanningIds">شناسه های برنامه ریزی دوره </param>
        /// <param name="saveAfterAdd"></param>
        /// <returns></returns>
        Task<bool> CreateFactorAsync(Factor factor,int[] productSalePlanningIds, bool saveAfterAdd = false);
        /// <summary>
        /// جهت دریافت لیست فاکتورهای یک گروه فاکتور
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <param name="taxCategoryId"></param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="factorReviewStatusType"></param>
        /// <returns></returns>
        IQueryable<Factor> GetByCustomFilterForFactorGroupList(int factorGroupId, int? taxCategoryId, int? companyId, int? representationId, int? factorReviewStatusType);

        /// <summary>
        /// جهت دریافت لیست فاکتور های متعلق به یک گروه بندی فاکتور
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        IQueryable<Factor> GetFactorsAssignedToFactorGroupAsync(int factorGroupId);
        /// <summary>
        /// جهت دریافت لیست فاکتورهایی که متعلق به هیچ گروه بندی فاکتوری نباشند.
        /// </summary>
        /// <returns></returns>
        IQueryable<Factor> GetFactorsNotAssignedToFactorGroupAsync(int? categoryGroupId);

        /// <summary>
        /// جهت قراردادن لیستی از فاکتورهای انتخاب شده در یک گروه بندی 
        /// </summary>
        /// <param name="selectedFactors"></param>
        /// <param name="groupId"></param>
        /// <param name="assign"></param>
        /// <returns></returns>
        Task<bool> AssignSelectedItemsToFactorGroup(List<int> selectedFactors, int groupId, bool assign);
        /// <summary>
        /// گزارش لیست فاکتورهای گروه
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        IQueryable<FactorGroupItemReport> GetFactorGroupItemList(int factorGroupId);
        Factor GetFactorIfAccess(int factorId, bool readOnly);
        /// <summary>
        /// دریافت لیست فاکتورهای گروه بندی شده تایید نهایی شده
        /// این لیست برای نمایش در فرم پرداختی های فاکتور کاربرد دارد
        /// فاکتورهایی که گروه بندی شده اند و گروه آن ثبت نهایی شده است
        /// میتوانند به کاربر نشان داده شوند تا پرداختی برای آن ثبت شود.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        IQueryable<Factor> GetGroupedFactors(int? companyId, int? representationId);

        /// <summary>
        /// بررسی میکند که برای
        /// SaleconfigurationId داده شده تا کنون فاکتوری صادر شده است یا خیر
        /// این متد زمانی استفاده میشود که هنگام بروزرسانی قرارداد یا تنظیم فروش
        /// اگر برخی پارامترهای مهم تغییر کردند باید چک شود که تاکنون فاکتوری صادر شده است یا خیر
        /// اگر فاکتوری صادر شده باشد نباید اجازه ویرایش به کاربر داده شود.
        /// </summary>
        /// <returns></returns>
        bool CheckExistenceFactorForSaleConfiguration(int saleConfigurationId);
        /// <summary>
        /// برای ارزیابی زمانبندی صدور فاکتور مورد استفاده قرار میگیرد.
        /// </summary>
        /// <returns></returns>
        string ValidateFactorYashilScheduler();
        /// <summary>
        /// برای دریافت تعداد دفعاتی که کاربر دکمه ثبت فاکتور را زده است
        /// دقت کنید که این متد تعداد فاکتور ها را برنمیگرداند و تعداد فاکتور ها لزوما با تعداد درخواست ها برابر
        ///و ممکن است با هر درخواست صدور فاکتور تعداد بیشتر از یکی فاکتور صادر شود.
        /// </summary>
        int? GetUserFactorRequestCountInEra(DateTime startDate, DateTime endDate);

        /// <summary>
        /// بررسی وجود فاکتور برای آیدی اطلاعات حساب شخص داده شده
        /// این متد در هنگام ویرایش اطلاعات حساب شخص مورد استفاده قرار مگیرد
        /// و اگر برای اطلاعات حساب شخص داده شده فاکتوری وجود داشت اجازه ویرایش به ان نمیدهد
        /// </summary>
        /// <param name="personBankaccountId"></param>
        /// <returns></returns>
        Task<bool> CheckExistAnyFactorByPersonBankAccountIdAsync(int personBankaccountId);
        /// <summary>
        /// برای دریافت لیست ریز پرداختی های فاکتورهای گروه بندی شده یک گروه
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        IQueryable<FactorItemsSoldToTheSataAndVezaratAndHekmatPaymentsView> GetFactorItemPaymentFullInfoViewList(int factorGroupId);
        /// <summary>
        /// مقایسه پرداخت های حکمت مربوط به کد فاکتور با دیتای  تراکنش های تایید شده حکمت که از طریق سی دی از شرکت اتکا گرفته میشود
        /// </summary>
        /// <param name="factorId">آیدی فاکتور</param>
        /// <returns></returns>
        IQueryable<PaymentComparisonWithHekmatApprovedTransactionDomainModel> GetPaymentComparisonWithHekmatApprovedTransactionByFactorCode(int factorId, bool showContrast);
        /// <summary>
        /// دریافت اطلاعات فاکتور
        /// نظیر نام مرکز و نمایندگی . اطلاعات پرداخت  تاریخ صدور و ....
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        FactorStatisticDomainModel GetFactorStatistic(int factorId);
        /// <summary>
        /// بررسی میکند که برای مرکز یا نمایندگی داده شده تاکنون فاکتوری صادر شده است یا خیر
        /// این متد زکانی استفاده میشود که بخواهخیم مرکز یا نمایندگی را بروزرسانی کنیم و چک میکنیم که اگر اطلاعات حساس مرکز یا نمایندگی تغییر کرده باشد تنها زمانی
        /// اجازه بروزرسانی دارد که تاکنون فاکتوری برای ان مرکز و یا نمایندگی صادر نشده باشد
        /// </summary>
        /// <param name="companyId">کد مرکز</param>
        /// <param name="representationId">کد نمایندگی</param>
        /// <returns></returns>
        bool CheckExistenceFactorForCompanyAndRepresentation(int companyId, int? representationId);

        /// <summary>
        /// آیا کاربر به این فاکتور دسترسی دارد یا نه؟
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        Task<bool> CheckUserAccessForFactorId(int factorId);
        /// <summary>
        /// فاکتور های انتخابی را برای اساین شدن به گروه بررسی می کند
        /// </summary>
        /// <param name="factorIds">شناسه فاکتور های انتخابی</param>
        /// <param name="factorGroupId">شناسه گروه فاکتور</param>
        /// <returns></returns>
        Task<(string message,bool isSuccess)> ValidateAssignSelectedItemsToFactorGroup
            (List<int> factorIds, int factorGroupId);
        /// <summary>
        /// دریافت فاکتور بر اساس شناسه فاکتور
        /// </summary>
        /// <param name="id">شناسه فاکتور</param>
        /// <param name="readOnly">خواندنی</param>
        /// <returns></returns>
        IQueryable<Factor> FindById(int id, bool readOnly = false);
        /// <summary>
        /// دریافت فاکتور ها بر اساس شناسه گروه فاکتور
        /// </summary>
        /// <param name="factorGroupId">شناسه گروه فاکتور</param>
        /// <returns></returns>
        IQueryable<Factor> GetByFactorGroupId(int factorGroupId);
        /// <summary>
        /// تغییر وضعیت فاکتور تایید شده به وضعیت در حال بررسی
        /// </summary>
        /// <param name="id">شناسه فاکتور</param>
        /// <returns></returns>
        Task<(bool IsSucces ,string Message)> ChangeFactorStatusToProcessingStatusAsync(int id);

        /// <summary>
        /// دریافت فاکتور کانورت شده
        /// </summary>
        /// <param name="factorGroupId">اعمال فیلتر براساس شناسه گروه فاکتور</param>
        /// <returns></returns>
        IQueryable<Factor> GetConvertedFactor(int? factorGroupId);

        /// <summary>
        /// بروزرسانی مقدار تعداد اقساط فاکتور از تنظیم فروش
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        Task<int> UpdateFactorInstallmentCountFromSaleConfiguration(int factorId);
    }

}
