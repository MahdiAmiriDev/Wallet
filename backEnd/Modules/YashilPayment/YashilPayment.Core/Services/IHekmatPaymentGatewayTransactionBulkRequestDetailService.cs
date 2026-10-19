using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilPayment.Core.Services
{
	public interface IHekmatPaymentGatewayTransactionBulkRequestDetailService : IGenericService<HekmatPaymentGatewayTransactionBulkRequestDetail, int>
    {
        IQueryable<HekmatPaymentGatewayTransactionBulkRequestDetail> GetByCustomFilter( int? appDocumentId, int? parentId, int? hekmatPaymentGatewayTransactionBulkRequestId);

        /// <summary>
        /// دریافت لیست تراکنش های Bulk
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="hekmatPaymentGatewayTransactionBulkRequestId">شناسه درخواست تراکنش انبوه</param>
        /// <param name="showInvalidItem">نمایش آیتم های نا معتبر</param>
        /// <returns></returns>
        IQueryable<HekmatPaymentGatewayTransactionBulkRequestDetail> GetInvalidHekmatBulkRequest(
            int userId,
            int hekmatPaymentGatewayTransactionBulkRequestId,
            bool showInvalidItem
            );

        IQueryable<HekmatPaymentGatewayTransactionBulkRequestDetail> FindByUserId(int userId);

        /// <summary>
        /// تراکنش های که به صورت انبوه درخواست داده شده است به همراه جزییات تراکنش
        /// در صورتی که دارای وضعیت نباشند
        /// یعنی
        /// Status = Null
        /// باشند را حذف می کنیم
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        Task<int> DeleteStatusNotDeterminedAsync(int userId);
        /// <summary>
        /// تراکنش های که به صورت انبوه درخواست داده شده است به همراه جزییات تراکنش
        /// در صورتی که دارای وضعیت نباشند
        /// یعنی
        /// Status = Null
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IQueryable<HekmatPaymentGatewayTransactionBulkRequestDetail> GetTransactionBulkRequestDetailStatusNotDeterminedByUserId(int userId);
    }
}
