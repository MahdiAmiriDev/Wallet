using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Repositories
{
    public interface IFactorPaymentInstallmentRepository : IGenericRepository<FactorPaymentInstallment, int>
    {
        IQueryable<FactorPaymentInstallment> GetByCustomFilter(int? factorGroupItemId, int? installmentPaymentStatusType);
        /// <summary>
        /// تاریخ سال و ماه اقساط فاکتور رو بر می گرداند
        /// جهت خروجی گرفتن اقساط فاکتور در سال و ماه مشخص کاربرد دارد
        /// </summary>
        /// <returns></returns>
        IQueryable<FactorPaymentInstallmentInfoDto> GetInstallmentMonthYearList();

        /// <summary>
        /// دریافت لیست اقساط
        /// </summary>
        /// <param name="yearMonth">سال ماه</param>
        ///<param name="categoryGroupId">شناسه دسته بندی گروه</param>
        ///<param name="installmentPaymentStatusType">وضعیت پرداخت قسط</param>
        /// <param name="readOnly">خواندنی</param>
        /// <returns></returns>
        IQueryable<FactorPaymentInstallment> GetFactorPaymentInstallment(
            int? yearMonth,
            int? categoryGroupId,
            int? installmentPaymentStatusType,
            bool readOnly = true);
        /// <summary>
        /// دریافت لیست اقساط جهت ثبت شناسه مرجع پرداخت
        /// </summary>
        /// <param name="yearMonth">سال ماه</param>
        ///<param name="categoryGroupId">شناسه دسته بندی گروه</param>
        ///<param name="installmentPaymentStatusType">وضعیت پرداخت قسط</param>
        ///<param name="creationData">UploadedFactorInstallmentPayment تاریخ ثبت فایل اکسل در جدول</param>
        /// <returns></returns>
        IQueryable<FactorPaymentInstallment> GetFactorPaymentInstallmentForRegisterReference(
           int? yearMonth,
           int? categoryGroupId,
           int? installmentPaymentStatusType,
           DateTime creationData);

        /// <summary>
        /// دریافت لیست اقساط بر اساس سال و ماه
        /// </summary>
        /// <param name="yearMonth">سال ماه</param>
        /// <param name="ids">شناسه اقساط پرداختی فاکتور</param>
        /// <param name="readOnly">خواندنی</param>
        /// <returns></returns>
        Task<IReadOnlyList<FactorPaymentInstallmentPerMonthDto>> GetPaymentListPerMonthAsync(int yearMonth, List<int> ids, bool readOnly = true);
        /// <summary>
        /// بروزرسانی اقساط پرداخت شده فاکتور از جدول
        /// UploadedFactorInstallmentPayment
        /// </summary>
        /// <param name="userId">شناسه کاربر لاگین کرده</param>
        /// <returns></returns>
        public Task<int> UpdateFactorInstallmentPaymentFromUploadedFileAsync(int userId);
    }
}
