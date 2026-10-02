using JetBrains.Annotations;

namespace Yashil.Core.CustomDomainModels.Commerce
{
    /// <summary>
    /// مربوط به گزارش کانورت
    /// </summary>
    public class FactorGroupConvertReport
    {
        /// <summary>
        /// نام کالا
        /// </summary>
        public string MainCategoryOrTaxTitle { get; set; }
        /// <summary>
        /// کد کالا
        /// </summary>
        public string CategoryOrTaxId { get; set; }
        /// <summary>
        /// واحد سنجش
        /// </summary>
        public string UnitOfMeasurement { get; set; }
        /// <summary>
        /// کد فاکتور
        /// </summary>
        public string FactorCode { get; set; }
        /// <summary>
        /// factorId ایدی فاکتور
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// تاریخ تایید فاکتور
        /// </summary>
        public int ConfirmDate { get; set; }
        /// <summary>
        /// تعداد وزارت
        /// </summary>
        public int VezaratCount { get; set; }
        /// <summary>
        /// تعداد ساتا
        /// </summary>
        public int SataCount { get; set; }
        /// <summary>
        /// فی وزارت
        /// </summary>
        public decimal Feevezarat { get; set; }
        /// <summary>
        /// فی ساتا
        /// </summary>
        public decimal FeeSata { get; set; }
        /// <summary>
        /// تعداد پرداختی
        /// </summary>
        public int TotalOrderItemCount { get; set; }
        /// <summary>
        /// فی خرید
        /// </summary>
        public decimal FeePurchaseAmount { get; set; }
        /// <summary>
        /// فی پرداختی
        /// </summary>
        public decimal FeePayment { get; set; }
        public string CompanyTitle { get; set; }
        [CanBeNull] public string RepresentationTitle { get; set; }
        /// <summary>
        /// کد ملی یا کد تفصیلی مشتری
        /// اگر موسس حقوقی باشد کد اقتصادی بعنوان کد ملی در نظر گرفته میشود
        /// اگر موسس حقیقی باشد کد ملی موسس بعنوان کد تفصیلی در نظر گرفته میشود
        /// </summary>
        public string DetailedCode { get; set; }


    }
}
