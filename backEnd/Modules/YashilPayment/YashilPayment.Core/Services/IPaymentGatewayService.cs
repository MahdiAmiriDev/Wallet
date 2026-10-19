using System.Threading.Tasks;
using Yashil.Core.CustomDtos.Payment;
using Yashil.Core.Entities;
using YashilPayment.Core.CustomDomainModels;

namespace YashilPayment.Core.Services;

/// <summary>
/// این سرویس به این خاطر ایجاد شده است که در
/// </summary>
public interface IPaymentGatewayService
{
    /// <summary>
    /// وقتی درخواست پرداخت از سمت پذیرنده ارسال می شود این متد جهت ساخت توکن فراخوانی می شود
    /// 
    /// </summary>
    /// <param name="merchantId">شناسه پذیرنده</param>
    /// <param name="amount">مبلغ پرداختی</param>
    /// <param name="returnUrl">آدرس بازگشتی</param>
    /// <param name="additionalData">اطلاعات کالایی که پرداخت می شود</param>
    /// <param name="reservationNumber">شناسه پرداخت پذیرنده</param>
    /// <param name="terminalId">شناسه ترمینال</param>
    /// <param name="installmentCount">تعداد اقساط</param>
    /// <param name="signData">امضاء</param>
    /// <param name="localDateTime">زمان درخواست</param>
    /// <param name="isHybrid">آیا پرداخت ترکیبی است یا خیر</param>
    /// <param name="isTaxExempt">معاف از مالیات می باشد</param>
    /// <param name="cardNumber">شماره کارت</param>
    /// <param name="cashPaymentPercentage">درصد پرداخت نقدی</param>
    /// <param name="shippingAmount">هزینه ارسال</param>
    /// <param name="useFormula">آیا از فرمول برای محاسبه قیمت استفاده کند
    /// در حالتی که از درگاه نقدی اقساطی طرح باقرالعلوم اقدام به پرداخت می کنیم
    /// نیازی به استفاده از فرمول برای محاسبه قیمت نیست</param>
    /// <returns></returns>
    Task<PaymentRequestResultDto> GenerateTokenForPaymentRequestAsync(int merchantId, long amount, string returnUrl,
        string additionalData, string reservationNumber, int terminalId,
        int installmentCount, string signData, string localDateTime, bool isHybrid,
        bool isTaxExempt = false,
        string? cardNumber = null,
        int? cashPaymentPercentage = null,
        int? shippingAmount = null,
        bool useFormula = true);

    /// <summary>
    /// وضعیت پرداخت ترکیبی را بر می گرداند،
    /// </summary>
    /// <param name="merchantId">شناسه پذیرنده</param>
    /// <param name="terminalId">شناسه ترمینال</param>
    /// <param name="reservationNumber">شناسه پرداخت پذیرنده</param>
    /// <param name="signData">امضاء</param>
    /// <param name="useFormula">برای محاسبه قیمت از فرمول استفاده شود؟</param>
    /// <returns></returns>
    public Task<HybridPaymentStatusResultDto> ValidateAndGetHybridPaymentStatusAsync(int merchantId, int terminalId,
        string reservationNumber, string signData, bool useFormula);



    /// <summary>
    /// توکن رمز شده ارسالی را دریافت میکند و در صورت مجاز بودن مقادیر و زمان توکن اطلاعات مربوط به آن را بر می گرداند
    /// </summary>
    /// <param name="encryptedToken"></param>
    /// <returns></returns>
    public Task<(bool succeed, string message, int status, IpgToken token, YashilHekmatTerminalInfoDomainModel terminalInfo)> ValidateAndExtractEncryptedTokenData(string encryptedToken);
    /// <summary>
    /// پرداخت اقساطی - حکمت - برای مدل نقدی اقساطی
    /// </summary>
    /// <param name="paymentRequestToken">توکن درخواست پرداخت</param>
    /// <param name="cashPaymentPercentage">درصد نقدی</param>
    /// <returns></returns>
    public Task<(bool succeeded, string message, int status, HybridPayment hybridPayment, IpgToken ipgToken)>
        ValidateAndCreateHekmatHybridPaymentAsync(string paymentRequestToken, int? cashPaymentPercentage);
    /// <summary>
    /// مبلغ کل اقساطی باید از مبلغ تنظیم شده در appConfig
    /// بیشتر باشد
    /// </summary>
    /// <param name="price">مبلغ کل اقساطی</param>
    /// <returns></returns>
    Task<(bool isValid, string message)> ValidateHekmatPriceAsync(long price);

    public Task<(bool succeeded, string message, int status, HybridPayment hybridPayment, IpgToken ipgToken)>
        ValidateAndCreateCashHybridPaymentAsync(string paymentRequestToken, int? cashPaymentPercentage);

    /// <summary>
    /// وضعیت پرداخت حکمت را بر می گرداند، نتیجه پرداخت حکمت به همراه پیام نتیجه پرداخت را بر می گرداند
    /// </summary>
    /// <param name="merchantId">شناسه پذیرنده</param>
    /// <param name="terminalId">شناسه ترمینال</param>
    /// <param name="reservationNumber">شناسه پرداخت پذیرنده</param>
    /// <param name="signData">امضاء</param>
    /// <returns></returns>
    public Task<HekmatPaymentStatusResultDto> GetHekmatPaymentStatusAsync(int merchantId, int terminalId,
        string reservationNumber, string signData);
}