using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilPayment.Core.Repositories
{
    public interface IHekmatPaymentGatewayTransactionRepository : IGenericRepository<HekmatPaymentGatewayTransaction, int>
    {

        IQueryable<HekmatPaymentGatewayTransaction> GetByCustomFilter(int? merchantId,int? hekmatTerminalId, int? fromDate, int? toDate,int? statusCode, string orderId = null);

        /// <summary>
        /// دریافت اطلاعات خرید مربوط به پرداخت حکمت بر اساس کد رهگیری مشتری و شناسه ترمینال 
        /// </summary>
        /// <param name="terminalId">شناسه ترمینال</param>
        /// <param name="reservationNumber">کد رهگیری مشتری</param>
        /// <returns></returns>
        Task<HekmatPaymentGatewayTransaction> GetByReservationNumberAndTerminalIdAsync(string reservationNumber, int terminalId);
        /// <summary>
        ///  دریافت تراکنش ها برای ارسال به حکمت یعنی تراکنش هایی که در وضعیت ارسال به حکمت می باشند
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        IQueryable<HekmatPaymentGatewayTransaction> GetTransactionsThatAreInStatusSendToHekmat(int applicationId);

        /// <summary>
        /// تراکنش های که در وضعیت ثبت اولیه می باشند
        ///  یعنی هنوز به وضعیت ارسال به حمکمت نرفته اند را بر می گرداند 
        /// </summary>
        IQueryable<HekmatPaymentGatewayTransaction> GetTransactionsThatAreInStatusInitialRegistration();

        /// <summary>
        /// تغییر وضعیت تراکنش ها به وضعیت ارسال به حکمت
        /// </summary>
        /// <returns></returns>
        Task<int> UpdateTransactionsStatusToSendToHekmatAsync();

        /// <summary>
        /// بررسی می کند که آیا تراکنشی در وضعیت ارسال به حکمت وجود دارد یا خیر
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckExistTransactionsThatAreInStatusSendToHekmat();

        /// <summary>
        /// ولیدیشن پیش از ثبت تراکنش پرداخت اقساطی در دیتابیس
        /// یکی از مراحل چک کردن لیست سیاه شماره موبایل ها جهت خرید میباشد
        /// </summary>
        /// <param name="hekmatPaymentGatewayTransaction"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        string ValidateBeforeHekmatPaymentPurchase(HekmatPaymentGatewayTransaction hekmatPaymentGatewayTransaction,
            int merchantId);

        /// <summary>
        /// دریافت رکورد تراکنش حکمت بر اساس شنتسه ترمینال و شناسه پرداخت سمت ترمینال پذیرنده
        /// </summary>
        /// <param name="yashilHekmatTerminalId">ترمینال های درگاه پرداخت</param>
        /// <param name="reservationNumber">شناسه تراکنش پرداخت یا سفارش سمت ترمینال پذیرنده</param>
        /// <param name="readOnly">فقط خواندنی</param>
        /// <returns></returns>
        Task<HekmatPaymentGatewayTransaction> GetTerminalHekmatTransactionAsync(
            int yashilHekmatTerminalId,
            string reservationNumber,
            bool readOnly = true);

        Task<HekmatPaymentGatewayTransaction> GetTerminalHekmatTransactionAsync(
    int yashilHekmatTerminalId,
    string reservationNumber,
    bool fromDbSet,
    bool readOnly = true);
    }
}
