using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IUploadedVoucherService : IGenericService<UploadedVoucher, int>
    {

        IQueryable<UploadedVoucher> GetByCustomFilter(int? appDocumentId);

        /// <summary>
        /// حذف کوپن های کاربر
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        void DeleteByUserId(int userId);
        /// <summary>
        /// بارگذاری کوپن از طریق فایل اکسل در جدول UploadVoucher
        /// </summary>
        /// <param name="tempVoucherFile">فایل ارسالی ار سمت کاربر</param>
        /// <param name="companyId">سناسه مرکز</param>
        /// <param name="voucherTypeId">نوع کوپن</param>
        /// <returns></returns>
        Task<ResultStatus<int>> BulkInsertFromExcelAsync(IFormFile tempVoucherFile, int companyId, int voucherTypeId);
        /// <summary>
        /// نمونه فایل کد کوپن را در قالب
        /// MemoryStream
        /// بر می گرداند
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <param name="voucherTypeId">شناسه نوع کوپن</param>
        /// <returns></returns>
        Task<MemoryStream> CopySampleVoucherCodeFileToWorkBook(int companyId, int voucherTypeId);
        /// <summary>
        /// دریافت کد کوپن های بارگذاری شده توسط کاربر
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        IQueryable<UploadedVoucher> GetByUserId(int userId);
        /// <summary>
        /// کد کوپن بارگذاری شده توسط کاربر را بعد از بارگذاری موفق رمز می کند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        Task<bool> EncryptVoucherCode(int userId);
    }
}
