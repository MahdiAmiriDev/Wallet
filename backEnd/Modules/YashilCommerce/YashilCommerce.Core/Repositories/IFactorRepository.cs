using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IFactorRepository : IGenericRepository<Factor, int>
    {
        IQueryable<Factor> GetByCustomFilter(int? taxCategoryId, int? companyId, int? representationId,
            int? factorReviewStatusType);

        /// <summary>
        /// check existence any unconfirmed Factor
        ///  return true if any factorReviewStatus in initial reject processing
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        bool CheckExistenceUnconfirmedFactor(int companyId, int? representationId);

        /// <summary>
        /// لیست تمام فاکتورهایی را برمیگرداند که تایید نشده اند
        /// </summary>
        /// <param name="taxCategoryId"></param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="factorReviewStatusType"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<Factor> GetByCustomFilterForDelete(int? taxCategoryId, int? companyId, int? representationId,
            int? factorReviewStatusType, bool readOnly = false);

        IQueryable<Factor> GetSupervisorFactorList(int? companyId, int? representationId, int? taxCategoryId,
            int? factorReviewStatusType);

        Task<bool> CreateFactorAsync(Factor factor,int[] productSalePlanningIds, string prefixFactorNumber, bool saveAfterAdd = false);
        bool CheckFactorGroupHasFactor(FactorGroup factorGroup);

        /// <summary>
        /// جهت دریافت لیست فاکتورهای یک گروه فاکتور
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <param name="taxCategoryId"></param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="factorReviewStatusType"></param>
        /// <returns></returns>
        IQueryable<Factor> GetByCustomFilterForFactorGroupList(int factorGroupId, int? taxCategoryId, int? companyId,
            int? representationId, int? factorReviewStatusType);

        /// <summary>
        /// جهت دریافت لیست فاکتورهای متعلق به گروه بندی فاکتور داده شده
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        IQueryable<Factor> GetFactorsAssignedToFactorGroupAsync(int factorGroupId);

        /// <summary>
        /// جهت دریافت لیست فاکتورهایی که متعلق به هیچ گروه بندی فاکتوری نیستند
        /// </summary>
        /// <returns></returns>
        IQueryable<Factor> GetFactorsNotAssignedToFactorGroupAsync(int? categoryGroupId);

        /// <summary>
        /// اساین کردن فاکتورهای انتخابی به یک گروه
        /// یا حذف فاکورهای انتخابی از یک گروه فاکتور
        /// </summary>
        /// <param name="selectedFactors"></param>
        /// <param name="groupId"></param>
        /// <param name="assign"></param>
        /// <returns></returns>
        Task AssignSelectedItemsToFactorGroup(List<int> selectedFactors, int groupId, bool assign);

        /// <summary>
        /// دریافت گزارش آیتمهای یک گروه فاکتور
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        IQueryable<FactorGroupItemReport> GetFactorGroupItemList(int factorGroupId);

        /// <summary>
        /// دریافت لیست فاکتور های یک کاربر
        /// </summary>
        /// <param name="factorId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        Factor GetUserFactor(int factorId, bool readOnly);

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
        /// بررسی میکند که آیا برای تنظیم فروش داده شده
        /// تا کنون فاکتوری صادر شده است یا خیر
        /// این متد هنگام بروزرسانی تنظیم فروش یا قرارداد کاربرد دارد
        /// و هنگام بروزرسانی انها باید پارامترهای مهم اگر تغییر کردند چک شود که آیا تاکنون فاکتوری برای ان صادر شده استیا خیر
        /// و اگر برای تنظیم فروش یا قرارداد فاکتوری صادر شده باشد
        /// نباید اجاز بروزرسانی را بدهد.
        /// </summary>
        /// <param name="saleConfigurationId"></param>
        /// <returns></returns>
        bool CheckExistenceFactorForSaleConfiguration(int saleConfigurationId);

        /// <summary>
        /// برای دریافت تعداد دفعاتی که کاربر دکمه ثبت فاکتور را زده است
        /// دقت کنید که این متد تعداد فاکتور ها را برنمیگرداند و تعداد فاکتور ها لزوما با تعداد درخواست ها برابر
        ///و ممکن است با هر درخواست صدور فاکتور تعداد بیشتر از یکی فاکتور صادر شود.
        /// </summary>
        int? GetUserFactorRequestCountInEra(DateTime startDate, DateTime endDate);

        /// <summary>
        /// بررسی وجود فاکتور برای آیدی اطلاعات حساب شخص داده شده
        /// </summary>
        /// <param name="personBankaccountId"></param>
        /// <returns></returns>
        Task<bool> CheckExistAnyFactorByPersonBankAccountIdAsync(int personBankaccountId);

        /// <summary>
        /// برای دریافت لیست ریز پرداختی های فاکتورهای گروه بندی شده یک گروه
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        IQueryable<FactorItemsSoldToTheSataAndVezaratAndHekmatPaymentsView> GetFactorItemPaymentFullInfoViewList(
            int factorGroupId);

        /// <summary>
        /// مقایسه پرداخت های حکمت مربوط به کد فاکتور با دیتای  تراکنش های تایید شده حکمت که از طریق سی دی از شرکت اتکا گرفته میشود
        /// </summary>
        /// <param name="factorId">کد فاکتور</param>
        /// <param name="showContrast"></param>
        /// <returns></returns>
        IQueryable<PaymentComparisonWithHekmatApprovedTransactionDomainModel>
            GetPaymentComparisonWithHekmatApprovedTransactionByFactorCode(int factorId, bool showContrast);
        /// <summary>
        /// دریافت اطلاعات فاکتور
        /// نظیر نام مرکز و نمایندگی و تاریخ صدور و اطلاعات پرداخت ان
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
        /// بررسی می کند که آیا گروه دسته بندی متفاوت در فاکتور ها وجود دارد یا خیر  
        /// </summary>
        /// <param name="factorIds">شناسه فاکتور ها</param>
        /// <returns></returns>
        Task<bool> IsExistDifferentCategoryGroupInSelectedFactorsAsync(List<int> factorIds);
        /// <summary>
        /// دریافت فاکتور بر اساس شناسه فاکتور
        /// </summary>
        /// <param name="factorId">شناسه فاکتور</param>
        /// <param name="readOnly">خواندنی</param>
        /// <returns></returns>
        IQueryable<Factor> FindById(int factorId, bool readOnly);
        /// <summary>
        /// دریافت فاکتور ها بر اساس شناسه گروه فاکتور
        /// </summary>
        /// <param name="factorGroupId">شناسه گروه فاکتور</param>
        /// <returns></returns>
        IQueryable<Factor> GetByFactorGroupId(int factorGroupId);
    }
}