using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilPayment.Core.Repositories
{
    public interface IHekmatPaymentGatewayTransactionBulkRequestRepository : IGenericRepository<HekmatPaymentGatewayTransactionBulkRequest, int>
    {
        IQueryable<HekmatPaymentGatewayTransactionBulkRequest> GetByCustomFilter(int? parentId, int? hekmatPaymentGatewayTransactionBulkRequestCategoryId);
        /// <summary>
        /// اعتبار سنجی تراکنش های بارگذاری شده کاربر
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        Task<int> GetInvalidUploadedHekmatTransactionBulkCountAsync(int userId);

        /// <summary>
        /// درخواست تراکنش ها بر اساس شناسه آن حذف می کند
        /// </summary>
        /// <param name="hekmatPaymentGatewayTransactionBulkRequestId">شناسه درخواست تراکنش انبوه</param>
        /// <returns></returns>
        Task DeleteByHekmatPaymentGatewayTransactionBulkRequestAsync(int hekmatPaymentGatewayTransactionBulkRequestId);

        Task<int> AddHekmatPaymentGatewayTransactionFromBulkRequestAsync(
            int hekmatPaymentGatewayTransactionBulkRequestId, int userId);
    }
}
