using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Dtos;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    /// <summary>
    /// پارامتر های مربوط به تایید پرداخت با کیف پول
    /// </summary>
    public class PayWithWalletParam : ICustomDntCaptchaBase
    {
        /// <summary>
        /// Order Id
        /// </summary>
        public int? OId { get; set; }
        /// <summary>
        /// کد مشتری این کد به عنوان رمز کاربر استفاده می شود
        /// </summary>
        [DisplayName("کدملی")]
        [Required(ErrorMessage ="لطفا کد ملی را وارد کنید")]
        public string Code { get; set; }
        /// <summary>
        /// شماره کارت
        /// </summary>
        public string CardNumber { get; set; }

        public string DNTCaptchaText { get; set; }
        public string DNTCaptchaToken { get; set; }
        [DisplayName("کد امنیتی")]
        [Required(ErrorMessage = "لطفا کد امنیتی را وارد کنید")]
        public string DNTCaptchaInputText { get; set; }
    }
    /// <summary>
    /// پارامترهای ارسالی جهت کنسل کردن پارامتر
    /// </summary>
    public class CancelWithWalletParam
    {
        /// <summary>
        /// Order Id
        /// </summary>
        public int? OId { get; set; }
    }
}
