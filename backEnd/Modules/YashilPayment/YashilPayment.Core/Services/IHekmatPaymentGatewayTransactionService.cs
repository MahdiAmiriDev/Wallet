using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilPayment.Core.Services
{
    public interface IHekmatPaymentGatewayTransactionService : IGenericService<HekmatPaymentGatewayTransaction, int>
    {

        IQueryable<HekmatPaymentGatewayTransaction> GetByCustomFilter(int? merchantId, int? hekmatTerminalId, int? fromDate, int? toDate,int? statusCode, string orderId = null);
        /// <summary>
        /// دریافت تراکنش ها برای ارسال به حکمت:
        ///   ابتدا  بررسی می کند که آیا تراکنش هایی در وضعیت ارسال به حکمت وجود دارد یا خیر
        /// 1 - اگر وجود داشت
        /// تراکنش هایی که در وضعیت ارسال به حکمت می باشند را بر می گرداند.
        /// 2 - در غیر اینصورت
        /// تراکنش هایی که در وضعیت ثبت اولیه می باشند را به وضعیت ارسال به حکمت تغییر می دهد، 
        /// سپس تراکنش هایی که در وضعیت ارسال به حکمت می باشند را بر می گرداند. 
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        Task<IQueryable<HekmatPaymentGatewayTransaction>> GetTransactionsForSendToHekmatAsync(int applicationId);

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
        /// دریافت اطلاعات خرید مربوط به پرداخت حکمت بر اساس کد رهگیری مشتری و شناسه ترمینال 
        /// </summary>
        /// <param name="terminalId">شناسه ترمینال</param>
        /// <param name="reservationNumber">کد رهگیری مشتری</param>
        /// <returns></returns>
        Task<HekmatPaymentGatewayTransaction> GetByReservationNumberAndTerminalIdAsync(string reservationNumber, int terminalId);
        /// <summary>
        /// برای تراکنش پرداخت یا سفارش سمت ترمینال پذیرنده
        /// تراکنش حکمت را بافته و به وضعیت لغو تغییر می دهیم
        /// </summary>
        /// <param name="yashilHekmatTerminalId">ترمینال های درگاه پرداخت</param>
        /// <param name="reservationNumber">شناسه تراکنش پرداخت یا سفارش سمت ترمینال پذیرنده</param>
        /// <returns></returns>
        Task<(bool isSucces, string message)> RefundHekmatTransactionAsync(int yashilHekmatTerminalId, string reservationNumber);

        /// <summary>
        /// از این متد برای لغو تراکنشی که هنوز به سمت درگاه حکمت ارسال نشده است میتوان استفاده کرده
        /// قبل از این که کاربران سی ار ام آپدیت حکمت را انجام دهند و وضعیت تراکنش 
        /// error code = 0 && status code = 0
        /// باشد وضعیت را به کنسل 
        /// status code = 2 and error code = 6
        /// تغییر میدهد
        /// </summary>
        /// <param name="yashilHekmatTerminalId"></param>
        /// <param name="reservationNumber"></param>
        /// <returns></returns>
        Task<(bool isSuccess, string message)> CancelHekmatTransactionAsync(int yashilHekmatTerminalId, string reservationNumber,long amount);
    }
}
