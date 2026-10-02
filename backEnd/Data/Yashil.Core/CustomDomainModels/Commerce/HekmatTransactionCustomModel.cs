namespace Yashil.Core.CustomDomainModels.Commerce
{
    public class HekmatTransactionCustomModel
    {
        /// <summary>
        /// ردیف گردش ترمینال - کد رهگیری پذیرنده
        /// </summary>
        public long InstallmentTrackingCode { get; set; }
        /// <summary>
        /// کد ملی صاحب حکمت
        /// </summary>
        public string NationalCode { get; set; }
        /// <summary>
        /// شماره کارت
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// مبلغ تراکنش
        /// </summary>
        public long Price { get; set; }
        /// <summary>
        ///تعداد اقساط 
        /// </summary>
        public int InstallmentCount { get; set; }
        /// <summary>
        /// وضعیت سفارش
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// کد خطا
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// آیا سفارش آموزشی می باشد یا خیر ؟ 
        /// </summary>
        public bool IsLearn { get; set; }
    }
}
