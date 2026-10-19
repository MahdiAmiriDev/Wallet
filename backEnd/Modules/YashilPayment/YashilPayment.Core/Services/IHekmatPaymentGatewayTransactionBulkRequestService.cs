using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilPayment.Core.Services
{
    public interface IHekmatPaymentGatewayTransactionBulkRequestService : IGenericService<HekmatPaymentGatewayTransactionBulkRequest, int>
    {
        IQueryable<HekmatPaymentGatewayTransactionBulkRequest> GetByCustomFilter(int? parentId, int? hekmatPaymentGatewayTransactionBulkRequestCategoryId);
        /// <summary>
        /// فایل نمونه تراکنش ها به جهت  بارگذاری تراکتش های به صورا انبوه از طریق اکسل
        /// </summary>
        /// <returns></returns>
        MemoryStream SampleTransactionFileToWorkBook();

        /// <summary>
        /// درج تراکنش های حکمت به صورت بالک
        /// </summary>
        /// <param name="transactionFile"></param>
        /// <param name="hekmatTransactionBulkRequestCategoryId">شناسه دسته بندی درخواست انبوه تراکنش حکمت </param>
        /// <returns></returns>
        Task<ResultStatus<int>> BulkInsertFromExcelAsync(IFormFile transactionFile, int hekmatTransactionBulkRequestCategoryId);

        Task<int> GetInvalidUploadedHekmatTransactionBulkCountAsync(int userId);

        /// <summary>
        /// تراکنش های که به صورت انبوه درخواست داده شده است به همراه جزییات تراکنش
        /// در صورتی که دارای وضعیت نباشند
        /// یعنی
        /// Status = Null
        /// باشند را حذف می کنیم
        /// سپس رک.رد درخواست را حذف می کنیم
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        Task DeleteStatusNotDeterminedBulkRequestWithDetailAsync(int userId);

        /// <summary>
        /// از درخواست بالک کاربر جاری
        /// تراکنش های در حال بررسی حکمت درخ می کند.
        /// </summary>
        /// <param name="hekmatPaymentGatewayTransactionBulkRequestId">شناسه درخواست بالک کاربر</param>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        Task<int> AddHekmatPaymentGatewayTransactionFromBulkRequestAsync(int hekmatPaymentGatewayTransactionBulkRequestId, int userId);
    }
}
