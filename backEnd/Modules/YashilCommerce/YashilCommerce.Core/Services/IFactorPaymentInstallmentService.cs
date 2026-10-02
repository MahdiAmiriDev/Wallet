using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface IFactorPaymentInstallmentService : IGenericService<FactorPaymentInstallment, int>
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
        /// <param name="paymentReferenceId">شناسه مرجع پرداخت</param>
        /// <returns></returns>
        IQueryable<FactorPaymentInstallment> GetFactorPaymentInstallment(int? yearMonth, int? categoryGroupId, int? installmentPaymentStatusType);

        /// <summary>
        /// دریافت لیست فقط خواندنی اقساط بر اساس سال و ماه
        /// </summary>
        /// <param name="ids">شناسه اقساط پرداختی فاکتور</param>
        /// <param name="yearMonth">سال ماه</param>
        /// <returns></returns>
        Task<IReadOnlyList<FactorPaymentInstallmentPerMonthDto>> GetPaymentListPerMonthAsync(int yearMonth,List<int> ids);

        /// <summary>
        /// دریافت اکسل اقساط فاکتور بر اساس سال ماه 
        /// </summary>
        /// <param name="yearMonth">ماه و سال</param>
        /// <param name="ids">شناسه اقساط پرداختی فاکتور</param>
        /// <returns></returns>
        Task<MemoryStream> ExportFactorPaymentInstallmentAsync(int yearMonth, List<int> ids);
        /// <summary>
        /// بروزرسانی اقساط پرداخت شده فاکتور از جدول
        /// UploadedFactorInstallmentPayment
        /// </summary>
        /// <param name="userId">شناسه کاربر لاگین کرده</param>
        /// <returns></returns>
        Task<int> UpdateFactorInstallmentPaymentFromUploadedFileAsync(int userId);
        /// <summary>
        /// ثبت گروهی شناسه مرجع اقساط پرداختی
        /// </summary>
        /// <param name="registerPaymentInstallmentReference">اطلاعات مرجع پرداخت</param>
        /// <returns></returns>
        Task<int> BulkInsertPaymentReferencesAsync(RegisterPaymentInstallmentReferencParamsDto registerPaymentInstallmentReference);

    }
}
