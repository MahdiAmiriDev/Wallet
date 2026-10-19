using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilPayment.Core.Repositories
{
	public interface IHekmatPaymentGatewayTransactionBulkRequestDetailRepository : IGenericRepository<HekmatPaymentGatewayTransactionBulkRequestDetail,int>
    {
        IQueryable<HekmatPaymentGatewayTransactionBulkRequestDetail> GetByCustomFilter( int? appDocumentId, int? parentId, int? hekmatPaymentGatewayTransactionBulkRequestId);

        /// <summary>
        /// دریافت لیست تراکنش های Bulk
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="hekmatPaymentGatewayTransactionBulkRequestId">شناسه درخواست تراکنش انبوه</param>
        /// <param name="showInvalidItem">نمایش آیتم های نا معتبر</param>
        /// <returns></returns>
        IQueryable<HekmatPaymentGatewayTransactionBulkRequestDetail> GetInvalidHekmatBulkRequestDetail(
            int userId,
            int hekmatPaymentGatewayTransactionBulkRequestId,
            bool showInvalidItem);

        IQueryable<HekmatPaymentGatewayTransactionBulkRequestDetail> FindByUserId(int userId);
    }
}      
 