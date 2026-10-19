using System.Threading.Tasks;
using YashilPayment.Core.DTO;

namespace YashilPayment.Core.Services
{
    public interface IIranKishPaymentService
    {
        /// <summary>
        /// این متد عملیات ایجاد توکن برای درخواست پرداخت آنلاین با استفاده از درگاه
        /// ایران کیش را انجام می دهد و توکن ایجاد شده را بازمیگرداند
        /// </summary>
        /// <param name="amount">مبلغ پرداختی</param>
        /// <param name="resNumber">شماره درخواست سمت سرور که باید به ازای
        /// هر تراکنش ایکتا باشد و معرف درخواست ارسالی ما به درگاه بانکی است</param>
        /// <returns></returns>
        Task<string> GenerateTokenAsync(long amount, string resNumber);

        /// <summary>
        /// دریافت تنظیمات
        /// </summary>
        /// <returns></returns>
        IranKishPaymentConfiguration GetIranKishPaymentConfiguration();

        /// <summary>
        /// این متد جهت تایید تراکنش درگاه ایران کیش کاربرد دارد
        /// </summary>
        /// <param name="terminalId"></param>
        /// <param name="retrievalReferenceNumber"></param>
        /// <param name="systemTraceAuditNumber"></param>
        /// <param name="tokenIdentity"></param>
        /// <returns></returns>
        Task<bool> VerifyMerchantTransactionAsync(string terminalId, string retrievalReferenceNumber,
            string systemTraceAuditNumber, string tokenIdentity);
    }
}
