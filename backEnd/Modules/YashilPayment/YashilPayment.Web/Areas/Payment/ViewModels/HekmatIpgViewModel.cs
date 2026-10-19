using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Yashil.Common.Core.Attributes;

namespace YashilPayment.Web.Areas.Payment.ViewModels
{
    /// <summary>
    /// وقتی کاربر از سایت فروشگاهی به صفحه
    /// Ipg
    /// فرستاده می شود اطلاعاتی در قالب پارمتر های زیر به صفحه
    /// Ipg
    /// ارسال می شود
    /// </summary>
    public class HekmatIpgViewModel
    {
        /// <summary>
        /// مبلغ ارسالی به درگاه
        /// </summary>
        public long Price { get; set; }
        /// <summary>
        /// شماره کارت حکمت
        /// </summary>
        [StringValidation] public string CardNumber { get; set; }
        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public int InstallmentCount { get; set; }
        /// <summary>
        /// نام پذیرنده
        /// </summary>
        [StringValidation] public string MerchantName { get; set; }
        /// <summary>
        /// آدرس پذیرنده
        /// </summary>
        [StringValidation] public string MerchantUrl { get; set; }
        /// <summary>
        /// شماره پذیرنده
        /// </summary>
        public int MerchantId { get; set; }
        /// <summary>
        /// متن تولید شده تویط کپچا 
        /// </summary>
        [StringValidation] public string DNTCaptchaText { get; set; }
        /// <summary>
        /// توکن کپچا
        /// </summary>
        [StringValidation] public string DNTCaptchaToken { get; set; }
        /// <summary>
        /// متن ورودی کپچا
        /// </summary>
        /// <returns></returns>
        [DisplayName("کد امنیتی")]
        [Required(ErrorMessage = "لطفا کد امنیتی را وارد کنید")]
        [StringValidation] public string DNTCaptchaInputText { get; set; }
        /// <summary>
        /// توکن 
        /// </summary>
        [StringValidation] public string PaymentRequestToken { get; set; }
        /// <summary>
        /// شماره ترمینال
        /// </summary>
        public int TerminalId { get; set; }
        /// <summary>
        /// اطلاعات سفارشی که پرداخت حکمت انجام می شود  
        /// </summary>
        [StringValidation] public string? AdditionalDate { get; set; }
        /// <summary>
        /// آدرس بازگشتی به سایت پذیرنده
        /// </summary>
        [StringValidation] public string CallBackUrl { get; set; }
        /// <summary>
        /// شناسه پرداخت پذیرنده
        /// </summary>
        [StringValidation] public string ReservationNumber { get; set; }

        /// <summary>
        /// آدرس پیش فرض لوگوی ترکینال
        /// </summary>
        [StringValidation] public string TerminalDefaultLogoUrl { get; set; }

        /// <summary>
        /// آیا پرداخت نقدی اقساطی است
        /// </summary>
        public bool UseFormula { get; set; }
    }
    /// <summary>
    /// وقتی کاربر مشخصات مربوط به کارت را در صفحه
    /// Ipg
    /// پر می کند و دکمه پرداخت را می زند اطلاعات فرم در قالب پارمتر های زیر به سمت سرور ارسال می شود
    /// </summary>
    public class HekmatTransactionParam
    {
        /// <summary>
        /// توکن
        /// </summary>
        [StringValidation] public string PaymentRequestToken { get; set; }

        /// <summary>
        /// شماره کارت حکمت
        /// </summary>
        [StringValidation] public string CardNumber { get; set; }

        /// <summary>
        /// کد ملی
        /// </summary>
        [StringValidation] public string NationalCode { get; set; }

        /// <summary>
        /// شماره موبایل
        /// </summary>
        [StringValidation] public string Mobile { get; set; }

        /// <summary>
        /// تلفن ثابت
        /// </summary>
        [StringValidation] public string Tell { get; set; }
    }

}
