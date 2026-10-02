namespace YashilCommerce.Core.Classes
{
    public class StoreConfiguration
    {
        /// <summary>
        /// مبتنی بر دوره
        /// </summary>
        public bool CourseBased { get; set; }
        /// <summary>
        /// مبتنی بر یارانه
        /// </summary>
        public bool SubsidyBased { get; set; }
        ///// <summary>
        ///// مبتنی بر قسط
        ///// </summary>
        //public int InstallmentBased { get; set; }
        /// <summary>
        /// پیشوند شماره سفارش
        /// </summary>
        public string OrderReferenceNumberPrefix { get; set; }
        /// <summary>
        /// حداکثر تعداد آیتم های سبد خرید
        /// </summary>
        public int MaxShoppingCartItem { get; set; }
        /// <summary>
        /// پیشوند فاکتور
        /// </summary>
        public string FactorNumberPrefix { get; set; }
        /// <summary>
        /// شماره کارت Recipient اجباری باشد یا خیر
        /// این فیلد در وضعیت MemberShipType=سایر استفاده میشود
        /// </summary>
        public bool RequireRecipientCardNumber { get; set; }
        /// <summary>
        /// تنظیم ارتفاع نمایش محصول در لیست محصولات
        /// 0 285px
        /// 1 200px
        /// 2 reserve
        /// 3 reserve
        /// </summary>
        public int ProductListLayout { get; set; }
        /// <summary>
        /// the maximum ?Recipient for each user can create or have 
        /// </summary>
        public int MaximumRecipientNumber { get; set; }
        /// <summary>
        /// نام پیش فرض عکس محصول
        /// </summary>
        public string ProductDefaultPictureName { get; set; }
        /// <summary>
        /// لوگوی سایت
        /// </summary>
        public string SiteLogo { get; set; }
        /// <summary>
        /// محدود به اعضای سایت
        /// در این حالت قبل از ثبت نام در سایت عضویت کاربر در باشگاه مشتریان بررسی می شود
        /// </summary>
        public bool LimitedToCustomerClub { get; set; }
        ///// <summary>
        ///// مبتنی بر حکمت است یا خیر
        ///// اگر باشد در اطلاعات دریافت کننده شماره حکمت را دریافت میکنیم
        ///// </summary>
        //public bool HekmatBased { get; set; }
        /// <summary>
        /// نمایش فیلتر بر اساس شهر
        /// </summary>
        public bool AllowFilterByCity { get; set; }
        /// <summary>
        /// حداکثر تعداد آیتم های سبد خرید
        /// </summary>
        public int MaxShoppingCardItemQuantity { get; set; }

        /// <summary>
        /// در صورتی که مقدار این متغیر true
        /// باشد، سفارش های در حال ثبت اولیه قبلی کاربر را قبل از محاسبه یارانه برای 
        /// سفارش جدید به حالت انصراف سیستمی می برد
        /// </summary>
        public bool CancelInitialRegistrationOrdersForSubsidyBasedSales { get; set; }
        /// <summary>
        /// آیا کد محصول در هنگام تعریف محصول ضروری است یا خیر
        /// </summary>
        public bool RequireProductCode { get; set; }
        /// <summary>
        /// آدرس فایلهای توضیحات مربوط به جزئیات محصول
        /// </summary>
        public string ProductDetailPagesPath { get; set; }
        /// <summary>
        /// ارسال پیامک دوره ای هنگامی که وضعیت سفارش تغییر پیدا میکند
        /// </summary>
        public bool SendMessageOnOrderStateChange { get; set; }
        /// <summary>
        /// بروزرسانی دوره ای وضعیت سفارش ها
        /// </summary>
        public bool UpdateExpiredOrderStatus { get; set; }
    }
}
