using System;
using Yashil.Common.Core.Attributes;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    /// <summary>
    /// این مدل به منظظور اینکه تراکنش های عودت وجه حکمت را نمایش دهد استفاده میشود
    /// در واقع برای نمایش اطلاعات از جدول های
    ///[HekmatCanceledTransaction] 
    ///[HekmatEffectlessCanceledTransaction]
    ///[HekmatEffectlessModifiedTransaction]
    ///[HekmatModifiedTransaction]
    /// استفاده میشود
    /// این جدول ها با هم union میشوند
    /// و در یک لیست به نمایش گذاشته میشود
    /// در فرم گروه بندی عودت وجه کاربرد دارد
    /// در فسمت بررسی عودت وجه درون کامپوننت لیست تراکنشات عودت وجه حکمت
    /// </summary>
    public class HekmatCanceledOrModifiedTransactionViewModel
    {
        [StringValidation] public string Id => Guid.NewGuid().ToString();
        /// <summary>
        ///  مبلغ تراکنش ارسالی به حکمت
        /// در واقع این مبلغ، مبلغ اولیه ای است که به حکمت ارسال شده است و بعدا اصلاح یا عودت داده شده است
        /// </summary>
        public long? Price { get; set; }
        /// <summary>
        /// مبلغ عودت
        /// برای بدون تاثیر ها 0 میخورد
        /// </summary>
        public long? RefundPrice { get; set; }
        /// <summary>
        /// شماره کارت حکمت
        /// </summary>
        [StringValidation] public string CardNumber { get; set; }
        /// <summary>
        /// تاربخ تراکنش 
        /// </summary>
        [StringValidation] public string TransactionDate { get; set; }
    }
}
