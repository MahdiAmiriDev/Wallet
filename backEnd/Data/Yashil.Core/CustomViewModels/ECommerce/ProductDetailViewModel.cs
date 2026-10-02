using Yashil.Common.Core.Attributes;

namespace Yashil.Core.CustomViewModels.ECommerce
{
    public class ProductDetailViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// کد بسته
        /// </summary>
        [StringValidation] public string Code { get; set; }
        /// <summary>
        /// عنوان بسته
        /// </summary>
        [StringValidation] public string Title { get; set; }

        /// <summary>
        /// توضیحات یا شرح مختصر
        /// </summary>
        [StringValidation] public string Description { get; set; }
        /// <summary>
        /// نوع بسته
        /// </summary>
        [StringValidation] public string Type { get; set; }

        /// <summary>
        /// قیمت  پایه آزمون
        /// </summary>
        [StringValidation] public string? TestBasePrice { get; set; }
        /// <summary>
        /// قیمت پایه کتاب
        /// </summary>
        [StringValidation] public string? BookBasePrice { get; set; }
        /// <summary>
        /// درصد تخفیف طرح باقرالعلوم
        /// </summary>
        [StringValidation] public string? TbaoCustomerClubDiscountPercentage { get; set; }
        /// <summary>
        /// درصد تخفیف سایر باشگاه : عمومی
        /// </summary>
        [StringValidation] public string? OtherCustomerClubDiscountPercentage { get; set; }
        /// <summary>
        /// هزینه ارسال
        ///</summary>
        [StringValidation] public string? DeliveryCost { get; set; }
        /// <summary>
        /// وضعیت انتشار
        /// </summary>
        [StringValidation] public string Published { get; set; }
        /// <summary>
        /// دسته بندی که دسته اصلی / زیر دسته می باشد
        /// </summary>
        [StringValidation] public string CategoryTitle { get; set; }
        /// <summary>
        /// انتخاب نوع ووچر
        /// </summary>
        [StringValidation] public string VocherTypeTitle { get; set; }
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

        /// <summary>
        /// توضیحات کامل 
        /// </summary>
        public string FullDescription { get; set; }

        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// حداکثر ظرفیت
        /// </summary>
        public string MaxCapacity { get; set; }
        /// <summary>
        /// گروه عملی یا نمایندگی
        /// </summary>
        public string RepresentationTitle { get; set; }
        /// <summary>
        /// نام کاربری دانشجو
        /// </summary>
        public string StudentUserName { get; set; }
        /// <summary>
        /// نام کاربری مربی یا استاد
        /// </summary>
        public string TeacherUserName { get; set; }
    }
}
