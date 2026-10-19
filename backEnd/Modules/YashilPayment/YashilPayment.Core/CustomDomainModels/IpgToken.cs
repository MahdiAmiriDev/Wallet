using System;

namespace YashilPayment.Core.CustomDomainModels;
/// <summary>
/// مولی که برای تبدیل مقدار توکن به پروپرتی های مربوط به توکن
/// </summary>
public class IpgToken
{
    /// <summary>
    /// آی پی آدرس
    /// </summary>
    public string IpAddress { get; set; }
    /// <summary>
    /// زمان انقضا توکن
    /// </summary>
    public DateTime TokenExpireDateTime { get; set; }
    /// <summary>
    /// شناسه پذیرنده
    /// </summary>
    public int MerchantId { get; set; }
    /// <summary>
    /// شناسه ترمینال
    /// </summary>
    public int TerminalId { get; set; }
    /// <summary>
    /// شناسه کد رهگیری مشتری به ازای هر پرداخت
    /// </summary>
    public string ReservationNumber { get; set; }
    /// <summary>
    /// زمان درخواست پرداخت با حکمت یا ترکیبی
    /// </summary>
    public DateTime LocalDateTime { get; set; }
    /// <summary>
    /// آدرس بازگشت
    /// </summary>
    public string ReturnUrl { get; set; }
    /// <summary>
    /// مبلغ پرداخت با درگاه حکمت یا ترکیبی
    /// </summary>
    public long Amount { get; set; }
    /// <summary>
    /// تعداد اقساط
    /// </summary>
    public int InstallmentCount { get; set; }
    /// <summary>
    /// شماره کارت حکمت
    /// </summary>
    public string CardNumber { get; set; }
    /// <summary>
    /// اطلاعات مربوط به خرید
    /// </summary>
    public string AdditionalData { get; set; }
    /// <summary>
    /// درصد پرداخت نقدی
    /// </summary>
    public int? CashPaymentPercentage { get; set; }
    /// <summary>
    /// هزینه پست
    /// </summary>
    public int? ShippingAmount { get; set; }
    /// <summary>
    /// معاف از مالیات می باشد
    /// </summary>
    public bool IsTaxExempt { get; set; }
    /// <summary>
    /// آیا از فرمول استفاده کند؟
    /// فرمول مربوط به محاسبه مبلغ نقدی و اقساطی
    /// در سامانه ی باقرالعلوم نیاز 
    /// </summary>
    public bool UseFormula { get; set; }
}