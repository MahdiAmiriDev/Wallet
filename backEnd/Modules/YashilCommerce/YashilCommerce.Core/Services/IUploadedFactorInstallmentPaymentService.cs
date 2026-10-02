using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
	public interface IUploadedFactorInstallmentPaymentService : IGenericService<UploadedFactorInstallmentPayment, int>
    {
        IQueryable<UploadedFactorInstallmentPayment> GetByCustomFilter(int? YearMonth,int? userId);
        /// <summary>
        /// در FactorPaymentInstallment این متد جهت درج اطلاعات  
        /// دیتابیس به صورت
        /// bulk
        /// کاربرد دارد
        /// </summary>
        /// <param name="tempFile"></param>
        /// <returns></returns>
        Task<ResultStatus<int>> BulkInsertFromExcelAsync(IFormFile tempFile,int yearMonth);

        /// <summary>
        /// بروزرسانی اقساط پرداخت شده فاکتور از جدول
        /// UploadedFactorInstallmentPayment
        /// </summary>
        /// <param name="userId">شناسه کاربر لاگین کرده</param>
        /// <returns></returns>
        Task<int> UpdateFactorInstallmentPaymentFromUploadedFileAsync(int userId);
        /// <summary>
        /// دریافت لیست شناسه مرجع پرداخت
        /// براساس تاریخ ثبت اکسل در جدول
        /// UploadedFactorInstallmentPayment
        /// گروه بندی می شود
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        Task<List<UploadedFactorInstallmentPaymentReferenceGroupDto>> GetFactorPaymentInstallmentReferenceGroupAsync(int yearMonth);
        /// <summary>
        /// دریافت لیست شناسه مرجع پرداخت
        /// که در تاریخ مشخص در جدول 
        /// UploadedFactorInstallmentPayment
        /// ذخیره شده اند
        /// </summary>
        /// <param name="creationDate"></param>
        /// <returns></returns>
        IQueryable<UploadedFactorInstallmentPaymentReferenceDto> GetFactorPaymentInstallmentReferenceAsync(DateTime creationDate);

    }
}
