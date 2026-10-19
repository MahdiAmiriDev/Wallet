using Yashil.Common.Core.Attributes;
using Yashil.Core.Enums;
using Yashil.Core.Protos;

namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;

public class VerifyHybridPaymentViewModel
{

    /// <summary>
    /// وضعیت انجام تراکنش با دو وضعیت " "Canceled By User" و یا ok" است
    /// </summary>
    [StringValidation] public string State { get; set; }
    /// <summary>
    ///  این پارامتر کدی وضعیت پرداخت را مشخص می کند 
    /// </summary>
    [StringValidation] public string? StateCode { get; set; }
    /// <summary>
    /// شماره پیگیری تراکنش انجامشده است.  
    /// </summary>
    [StringValidation] public string? RefrenceNumber { get; set; }
    /// <summary>
    /// کدی است که پذیرنده به کالای خود اختصاص داده و آن برای درگاه پرداخت اینترنتی فنآوا کارت ارسال میکند. 
    /// </summary>
    [StringValidation] public string? ReservationNumber { get; set; }
    /// <summary>
    /// شماره پیگیری خریدار -(شماره مرجع تراکنش -RNN
    /// </summary>
    [StringValidation] public string? CustomerRefNum { get; set; }
    /// <summary>
    ///   شماره کارت خریدار MasK
    /// </summary>
    [StringValidation] public string? CardMaskPan { get; set; }
    /// <summary>
    /// این پارامتر ش اره کارت خریدار با الزامات شاپرک می باشد.
    /// </summary>
    [StringValidation] public string SecurePan { get; set; }
    /// <summary>
    /// آدرس پست الکترونیک کاربر است که در صفحه پرداخت وارد نموده است
    /// </summary>
    [StringValidation] public string? Email { get; set; }
    /// <summary>
    /// شماره موبایل کاربر است که در صفحه پرداخت وارد نموده است
    /// </summary>
    [StringValidation] public string? Mobile { get; set; }
    /// <summary>
    /// پیام
    /// </summary>
    [StringValidation] public string? Message { get; set; }

    /// <summary>
    /// مبلغ پرداخت شده
    /// </summary>
    [StringValidation] public string? Amount { get; set; }

    /// <summary>
    /// شناسه پرداخت ترکیبی
    /// </summary>
    [StringValidation] public int HybridPaymentId { get; set; }
    /// <summary>
    /// شماره سفارش
    /// </summary>
    [StringValidation] public string HybridPaymentRequestTitle { get; set; }

    public PaymentMethodTypesEnum PaymentMethodType { get; set; }
    /// <summary>
    /// وضعیت پرداخت
    /// </summary>
    public HybridPaymentStatusTypeEnum HybridPaymentStatusTypeEnum { get; set; }

    /// <summary>
    /// آدرس بازگشت به سایت پذیرنده
    /// این پراپرتی برای پرداخت ترکیبی مورد استفاده قرار میگیرد
    /// زیرا در پرداخت ترکیبی از یک صفحه میانی به درگاه های پرداخت متصل میشویم و از درگاه ها به یک صفحه میانی دیگر میرویم که از آنجا باید به صفحه پذیرنده برویم
    /// </summary>
    [StringValidation] public string? CallBackUrl { get; set; }

    public HybridPaymentRequestStatusResultModel? HybridPaymentStatusResultModel { get; set; }

    /// <summary>
    /// مبلغ پایه
    /// </summary>
    public string? BasePrice { get; set; }

    /// <summary>
    /// آیا پرداخت نقدی اقساطی است
    /// در موارد مورد استفاده در سامانه ی باقر العلوم کاربرد دارد
    /// </summary>
    public string? UseFormula { get; set; }
}