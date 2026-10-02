using System;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Repositories
{
	public interface IUploadedFactorInstallmentPaymentRepository : IGenericRepository<UploadedFactorInstallmentPayment,int>
    {
        IQueryable<UploadedFactorInstallmentPayment> GetByCustomFilter(int? YearMonth, int? userId);
        /// <summary>
        /// حذف دیتای فایل آپلود شده مربوط به اقساط پرداخت شده فاکتور 
        /// که دارای دیتای نامعتبر است
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="yearMonth">سال و ماه</param>
        void DeleteByYearMonth(int userId,int yearMonth);
        /// <summary>
        /// اعتبار سنجی مقادیر فایل اکسل قبل از درج در جدول
        /// FactorPaymentInstallment
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="yearMonth">سال و ماه اقساط</param>
        /// <returns></returns>
        Task<bool> ValidateUploadedFactorInstallmentPayment(
                 int userId,
                 int yearMonth);
        /// <summary>
        /// دریافت مرجع پرداخت اقساط فاکتور
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="yearMonth">سال و ماه اقساط</param>
        /// <returns></returns>
        IQueryable<UploadedFactorInstallmentPaymentReferenceGroupDto> GetFactorPaymentInstallmentReferenceGroup(int userId, int yearMonth);
        /// <summary>
        /// دریافت لیست شناسه مرجع پرداخت
        /// که در تاریخ مشخص در جدول 
        /// UploadedFactorInstallmentPayment
        /// ذخیره شده اند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="creationDate">زمان ایجاد دیتای جدول</param>
        /// <returns></returns>
        IQueryable<UploadedFactorInstallmentPaymentReferenceDto> GetFactorPaymentInstallmentReference(int userId,DateTime creationDate);
    }
}      
 