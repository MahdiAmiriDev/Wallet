using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities; 

namespace YashilPayment.Core.Repositories
{
	public interface IRefundPaymentTransactionItemRequestRepository : IGenericRepository<RefundPaymentTransactionItemRequest,int>
    {
        IQueryable<RefundPaymentTransactionItemRequest> GetByCustomFilter( int? parentId, int? yashilHekmatTerminalId, int? refundPaymentTransactionItemRequestType, int? refundPaymentTransactionItemRequestStatus);

        /// <summary>
        /// دریافت سفارش های کامل شده مربوط به سامانه ایزی بوک
        /// بر اساس فیلتر های فروشنده و بازه تاریخی
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="startDateInt"></param>
        /// <param name="endDateInt"></param>
        /// <returns></returns>
        IQueryable<IsiBookCompleteOrderDomainModel> GetIsiBookCompleteOrder(int vendorId, int startDateInt,
            int endDateInt);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="yashilHekmatTerminalId"></param>
        /// <param name="reservationNumber"></param>
        /// <returns></returns>
        Task<string> RefundTerminalPaymentTransaction(int yashilHekmatTerminalId, string reservationNumber);
    }
}      
 