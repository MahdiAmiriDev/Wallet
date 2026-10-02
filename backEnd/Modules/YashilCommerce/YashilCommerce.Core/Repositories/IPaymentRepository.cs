using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using Yashil.Core.Enums;

namespace YashilCommerce.Core.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment, int>
    {
        /// <summary>
        /// اقساط پرداختی های باشگاه مشتریان-ناجا-محاسبه و در جدول مربوطه درج می کند
        /// shp.CustomerClubPaymentInstallment
        /// </summary>
        /// <returns>تعداد رکورد های درج شده</returns>
        Task<int> InsertCustomerClubPaymentInstallmentAsync();
        IQueryable<Payment> GetByCustomFilter(int? cardType, int? saleType, int? paymentStatusType,
            int? paymentMethodType, int? orderId);

        Payment GetOrderLastPayment(int oId);
        Task<Payment> GetByReservationNumberIncludeOrderAsync(string reservationNumber, bool readOnly = false);
        IQueryable<Payment> GetOrderPaymentsByStatusType(int orderId, PaymentStatusTypesEnum paymentStatusType);

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
        /// جهت دریافت لیست پرداختیهای کیف پول باشگاه مشتریان یک گروه
        /// </summary>
        /// <param name="paymentGroupId"></param>
        /// <returns></returns>
        IQueryable<Payment> GetCustomerClubPaymentsAssignedToPaymentGroup(int paymentGroupId);

        /// <summary>
        /// جهت دریافت لیست پرداختی هایی که تا کنون گروه بندی نشده اند
        /// </summary>
        /// <returns></returns>
        IQueryable<Payment> GetCustomerClubWalletPaymentsNotAssignedToPaymentGroup(int? customerClubId, bool readOnly = false);

        Task<bool> ExistsAnyPaymentWithThisItemGroupIdAsync(int itemGroupId);
        Task<int> AssignAndConfirmCustomerClubWalletPaymentGroup(int customerClubId, int paymentGroupId);

        /// <summary>
        /// جستجوی پرداخت با رسید دیجیتالی ارسالی از سمت بانک
        /// با توجه به متد پرداخت
        /// وجود دارد یا خیر؟
        /// </summary>
        /// <param name="paymentMethodType"></param>
        /// <param name="refNum">رسید دیجیتالی</param>
        Task<bool> CheckExitsByRefNumberAsync(PaymentMethodTypesEnum paymentMethodType, string refNum);

        IQueryable<Payment> FindById(int paymentId);
        /// <summary>
        /// بررسی وجود پرداخت بر اساس
        /// شماره مرجع بازیابی تراکنش / کد مرجع تراکنش
        /// و نوع پرداخت
        /// </summary>
        /// <param name="paymentMethodType">نوع متد پرداخت</param>
        /// <param name="retrievalReferenceNumber">کد رهگیری تراکنش</param>
        /// <returns></returns>
        Task<bool> CheckExitsByRetrievalReferenceNumberAsync(PaymentMethodTypesEnum paymentMethodType, string retrievalReferenceNumber);

        /// <summary>
        /// دریافت پرداخت بر اساس توکن
        /// </summary>
        /// <param name="token">توکن</param>
        /// <param name="paymentMethodType">نوع متد پرداخت</param>
        /// <param name="readOnly">فقط خواندنی</param>
        /// <param name="applicationId">برنامه</param>
        /// <returns></returns>
        Task<Payment> GetByTokenAsync(string token,int paymentMethodType, bool readOnly = false, int? applicationId = null);
        /// <summary>
        /// بررسی وجود توکن برای متد پرداخت ارسالی
        /// </summary>
        /// <param name="token">توکن</param>
        /// <param name="paymentMethodType">نوع متد پرداخت</param>
        /// <returns></returns>                 
        Task<bool> CheckExistByTokenAsync(string token, int paymentMethodType);

        /// <summary>
        /// دریافت payment
        /// بر اساس مقدار توکن
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        IQueryable<Payment> FindByToken(string token);
    }
}