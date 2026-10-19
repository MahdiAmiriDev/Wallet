using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using Yashil.Core.Enums;

namespace YashilPayment.Core.Services
{
	public interface IPaymentTransactionItemRefundService : IGenericService<PaymentTransactionItemRefund, int>
    {
        IQueryable<PaymentTransactionItemRefund> GetByCustomFilter(int? refundPaymentTransactionItemRequestId);

        /// <summary>
        ///    ساخت انتیتی عودت آیتم تراکنش پرداخت
        /// </summary>
        /// <param name="paymentTransactionItemRefundRequestId">شناسه درخواست عودت وجه تراکنش پرداخت</param>
        /// <param name="yashilHekmatTerminalId">شناسه ترمینال درگاه </param>
        /// <param name="oldPaymentAmount">مبلغ قبلی پرداخت شده تراکنش</param>
        /// <param name="refundType">نوع عودت</param>
        /// <returns></returns>
        PaymentTransactionItemRefund CreateNewPaymentTransactionItemRefundEntity(
            int paymentTransactionItemRefundRequestId,
            int yashilHekmatTerminalId ,
            long oldPaymentAmount,
            RefundTypeEnum refundType);
    }
}
