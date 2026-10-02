namespace Yashil.Core.CustomDomainModels
{
    public class ProductDetailDomainModel
    {
        public int Id { get; set; }
        /// <summary>
        /// کد بسته
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// عنوان بسته
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// توضیحات یا شرح مختصر
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// نوع بسته
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// قیمت  پایه آزمون
        /// </summary>
        public decimal? TestBasePrice { get; set; }
        /// <summary>
        /// قیمت پایه کتاب
        /// </summary>
        public decimal? BookBasePrice { get; set; }
        /// <summary>
        /// درصد تخفیف طرح باقرالعلوم
        /// </summary>
        public decimal? TbaoCustomerClubDiscountPercentage { get; set; }
        /// <summary>
        /// درصد تخفیف سایر باشگاه : عمومی
        /// </summary>
        public decimal? OtherCustomerClubDiscountPercentage { get; set; }
        /// <summary>
        /// هزینه ارسال
        ///</summary>
        public decimal? DeliveryCost { get; set; }
        /// <summary>
        /// وضعیت انتشار
        /// </summary>
        public string Published { get; set; }
        /// <summary>
        /// دسته بندی که دسته اصلی / زیر دسته می باشد
        /// </summary>
        public string CategoryTitle { get; set; }
        /// <summary>
        /// انتخاب نوع ووچر
        /// </summary>
        public string VocherTypeTitle { get; set; }
        /// <summary>
        /// نمایش زیر نویس برنامه ریزی دوره
        /// </summary>
        public string ProductSalePlanningDisplaySubTitle { get; set; }

        /// <summary>
        /// گروه محصول برای فاکتور و کانورت
        /// برای حالت بر پایه آزمون
        /// </summary>
        public string ProductGroupIdForExam { get; set; }
        /// <summary>
        /// گروه محصول برای فاکتور و کانورت
        /// برای حالت بر پایه کتاب
        /// </summary>
        public string ProductGroupIdForBook { get; set; }
    }
}
