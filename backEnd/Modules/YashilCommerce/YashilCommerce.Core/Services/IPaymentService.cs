using System;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using Yashil.Core.Enums;

namespace YashilCommerce.Core.Services
{
    public interface IPaymentService : IGenericService<Payment, int>
    {

        IQueryable<Payment> GetByCustomFilter(int? cardType, int? saleType, int? paymentStatusType, int? paymentMethodType, int? orderId);

        Task<Payment> UpdatePaymentStatusAsync(int paymentId, PaymentStatusTypesEnum paymentStatusType,
            PaymentMethodTypesEnum paymentMethodType, string message, bool saveAfterUpdate = false);
        Task<Payment> UpdatePaymentStatusAsync(int paymentId, PaymentStatusTypesEnum paymentStatusType,
            PaymentMethodTypesEnum paymentMethodType, string message, string referenceNumber,
            bool saveAfterUpdate = false);

        /// <summary>
        /// این متد عمدتا در هنگام پرداخت فن اوا کارت یا انصار مورد استفاده قرار میگیرد
        /// در هنگام پرداخت با فن آوا کارت
        /// پارامترهای بیشتری توسط بانک ارسال میشود که این پارامتر ها نیز ذخیره می شوند
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="paymentStatusType"></param>
        /// <param name="paymentMethodType"></param>
        /// <param name="message"></param>
        /// <param name="referenceNumber"></param>
        /// <param name="customerRefNum"></param>
        /// <param name="goodReferenceId"></param>
        /// <param name="merchantData"></param>
        /// <param name="token">توکن دریافتی از سمت درگاه</param>
        /// <param name="saveAfterUpdate"></param>
        /// <param name="retrievalReferenceNumber">(شماره مرجع بازیابی تراکنش / کد مرجع تراکنش)</param>
        /// <param name="systemTraceAuditNumber">(شماره پیگیری/ردیابی سیستمی تراکنش)</param>
        /// <returns></returns>
        Task<Payment> UpdatePaymentStatusAsync(int paymentId, PaymentStatusTypesEnum paymentStatusType,
            PaymentMethodTypesEnum paymentMethodType, string message, string referenceNumber, string customerRefNum,
            string goodReferenceId, string merchantData, bool saveAfterUpdate = false, string token = null,
            string retrievalReferenceNumber = null,
            string systemTraceAuditNumber = default);


        Task<Payment> GetByReservationNumberIncludeOrderAsync(string resNum, bool readOnly = false);
        IQueryable<Payment> GetOrderPaymentsByStatusType(int orderId, PaymentStatusTypesEnum paymentStatusType);
        /// <summary>
        /// دریافت آخرین رکورد پرداخت برای  سفارش
        /// </summary>
        /// <param name="oId"></param>
        /// <returns></returns>
        Payment GetOrderLastPayment(int oId);
        /// <summary>
        /// Get Paymnets Order By OrderId
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns></returns>
        IQueryable<Payment> GetPaymentsByOrderId(int orderId);


        /// <summary>
        /// لیست تراکنش های در حال بررسی و منتظر پرداخت کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IQueryable<Payment> GetUserPendingHekmatPayment(int userId);
        /// <summary>
        /// جهت دریافت لیست پرداخت های کیف پول باشگاه مشتریان یک گروه
        /// </summary>
        /// <param name="paymentGroupId"></param>
        /// <returns></returns>
        IQueryable<Payment> GetCustomerClubWalletPaymentsAssignedToPaymentGroup(int paymentGroupId);
        /// <summary>
        /// جهت انتصاب پرداختهای کیف پول باشگاه مشتریانی که تاکنون گروه بندی نشده اند به یک گروه انتخابی و ثبت نهایی گروه
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<bool> AssignAndConfirmCustomerClubWalletPaymentGroup(int groupId);

        /// <summary>
        /// جهت دریافت لیست پرداختی هایی که هنوز گروه بندی نشده اند
        /// </summary>
        /// <returns></returns>
        IQueryable<Payment> GetCustomerClubWalletPaymentsNotAssignedToPaymentGroup(int? customerClubId, bool readOnly = false);

        decimal GetOrderTotalPayment(int orderId, PaymentStatusTypesEnum paid);
        /// <summary>
        /// آیا پرداختی با گروه بندی داده شده وجود دارد یا نه
        /// </summary>
        /// <param name="itemGroupId">گروه </param>
        /// <returns></returns>
        Task<bool> ExistsAnyPaymentWithThisItemGroupIdAsync(int itemGroupId);

        /// <summary>
        /// جستجوی پرداخت با رسید دیجیتالی ارسالی از سمت بانک
        /// با توجه به متد پرداخت
        /// وجود دارد یا خیر؟
        /// </summary>
        /// <param name="paymentMethodType"></param>
        /// <param name="refNum">رسید دیجیتالی</param>
        Task<bool> CheckExitsByRefNumberAsync(PaymentMethodTypesEnum paymentMethodType, string refNum);
        /// <summary>
        /// بررسی وجود پرداخت بر اساس
        /// شماره مرجع بازیابی تراکنش / کد مرجع تراکنش
        /// و نوع پرداخت
        /// </summary>
        /// <param name="paymentMethodType">نوع پرداخت</param>
        /// <param name="retrievalReferenceNumber"></param>
        /// <returns></returns>
        Task<bool> CheckExitsByRetrievalReferenceNumberAsync(PaymentMethodTypesEnum paymentMethodType, string retrievalReferenceNumber);

        /// <summary>
        ///  دریافت پرداخت بر اساس توکن
        /// </summary>
        /// <param name="token">توکن</param>
        /// <param name="paymentMethodType">نوع متد پرداخت</param>
        /// <param name="readOnly">فقط خواندنی</param>
        /// <param name="applicationId">برنامه</param>
        /// <returns></returns>
        Task<Payment> GetByTokenAsync(string token, int paymentMethodType, bool readOnly = false, int? applicationId = null);

        /// <summary>
        /// بررسی وجود توکن برای متد پرداخت ارسالی
        /// </summary>
        /// <param name="token">توکن</param>
        /// <param name="paymentMethodType">نوع متد پرداخت</param>
        /// <returns></returns>
        Task<bool> CheckExistByTokenAsync(string token, int paymentMethodType);

        /// <summary>
        /// بروزرسانی مقدار
        /// token
        /// مربوط به یک
        /// payment
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id">شناسه پرداخت</param>
        /// <returns></returns>
        Task<int> UpdateTokenAsync(string token,int id);

        /// <summary>
        /// دریافت payment
        /// بر اساس مقدار توکن
        /// </summary>
        /// <param name="token"></param>
        /// <param name="readOnly"></param>
        /// <param name="paymentMethodType">نوع درگاه پرداخت</param>
        /// <returns></returns>
        IQueryable<Payment> FindByToken(string token,PaymentMethodTypesEnum paymentMethodType,bool readOnly);
    }
}
