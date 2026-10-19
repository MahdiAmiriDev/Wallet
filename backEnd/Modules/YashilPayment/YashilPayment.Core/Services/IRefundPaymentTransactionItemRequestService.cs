using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;
using YashilPayment.Core.DTO;
using YashilPayment.Core.Enums;

namespace YashilPayment.Core.Services
{
	public interface IRefundPaymentTransactionItemRequestService : IGenericService<RefundPaymentTransactionItemRequest, int>
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
        /// این متد جهت ایجاد یک رکورد جدید در جدول
        /// RefundPaymentTransactionItemRequest
        /// کاربرد دارد
        /// </summary>
        /// <returns></returns>
        Task AddRefundPaymentTransactionItemRequestAsync(AddRefundPaymentTransactionItemRequestDto dto);

        /// <summary>
        /// بررسی می کند که آیا رکوردی با برای داده های فرستاده شده درج شده است یا نه؟
        /// </summary>
        /// <param name="yashilHekmatTerminalId">ترمینال های درگاه پرداخت</param>
        /// <param name="reservationNumber">شناسه تراکنش پرداخت یا سفارش سمت ترمینال پذیرنده</param>
        /// <returns></returns>
        Task<bool> HasUserInsertRecordBeforeFroPaymentTransactionAsync(int yashilHekmatTerminalId, string reservationNumber);
        /// <summary>
        /// رد یا تایید درخواست عودت تراکنش مربوط به ترمینال ها
        /// </summary>
        /// <param name="paymentTransactionItemRefundRequestId">شناسه درخواست عودت تراکنش ترمینال</param>
        /// <param name="rejectRequest">درخواست کاربر رد شود ؟؟</param>
        /// <returns></returns>
        Task<(bool isSucces, string message)> ApplyRefundPaymentTransactionItemRequestActionAsync(int paymentTransactionItemRefundRequestId, bool? rejectRequest);
        /// <summary>
        ///  تراکنش های مروبط به سایت ترمینال پذیرنده را عودت می زند
        /// </summary>
        /// <param name="yashilHekmatTerminalId">ترمینال های درگاه پرداخت</param>
        /// <param name="reservationNumber">شناسه تراکنش پرداخت یا سفارش سمت ترمینال پذیرنده</param>
        /// <returns></returns>
        Task<string> RefundTerminalPaymentTransaction(int yashilHekmatTerminalId, string reservationNumber);
        /// <summary>
        /// دریافت درخواست عودت تراکنش هایی با وضعیت ثبت اولیه برای عملیات 
        /// </summary>
        /// <returns></returns>
        IQueryable<RefundPaymentTransactionItemRequest> GetInitialRegistrationPaymentTransactionItemRequestsForActions();
        /// <summary>
        /// دریافت درخواست عودت تراکنش هایی بر اساس وضعیت درخواست ارسالی در پارمتر متد 
        /// </summary>
        /// <param name="refundPaymentTransactionItemRequestStatus">وضعیت درخواست عودت تراکنش پرداخت</param>
        /// <param name="readOnly">آیا فقط خواندنی باشد</param>
        /// <returns></returns>
        IQueryable<RefundPaymentTransactionItemRequest> GetPaymentTransactionItemRefundRequestsByStatus(
            RefundPaymentTransactionItemRequestStatusEnum refundPaymentTransactionItemRequestStatus,
            bool readOnly = true);
    }
}
