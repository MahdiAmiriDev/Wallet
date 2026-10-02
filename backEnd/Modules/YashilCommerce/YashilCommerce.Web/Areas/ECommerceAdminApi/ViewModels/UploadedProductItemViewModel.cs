using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class UploadedProductItemListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
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
        /// توضیحات
        ///</summary>
        [StringValidation] public string Description { get; set; }

        /// <summary>
        /// نوع
        ///</summary>
        [StringValidation] public string Type { get; set; }
        /// <summary>
        /// دسته اصلی(پایه)
        ///</summary>
        [StringValidation] public string ParentCategory { get; set; }
        /// <summary>
        /// زیر دسته (رشته)
        ///</summary>
        [StringValidation] public string ChildCategory { get; set; }
        /// <summary>
        /// مبلغ با تخفیف آزمون
        ///</summary>
        [StringValidation] public string TestBasePrice { get; set; }
        /// <summary>
        /// آیا مبلغ با تخفیف آزمون اشتباه میباشد
        ///</summary>
        public bool? TestBasePriceIsInValid { get; set; }
        /// <summary>
        /// درصد تخفیف طرح باقرالعلوم
        ///</summary>
        [StringValidation] public string TbaoCustomerClubDiscountPercentage { get; set; }
        /// <summary>
        /// درصد تخفیف طرح باقرالعلوم معتبر نمی باشد
        ///</summary>
        public string TbaoCustomerClubDiscountPercentageIsInValid { get; set; }
        /// <summary>
        /// درصد تخفیف سایر باشگاه ها
        ///</summary>
        [StringValidation] public string OtherCustomerClubDiscountPercentage { get; set; }
        /// <summary>
        /// درصد تخفیف سایر باشگاه ها معتبر نمی باشد
        ///</summary>
        public string OtherCustomerClubDiscountPercentageIsInValid { get; set; }
        
        [StringValidation] public string Published { get; set; }

        /// <summary>
        /// هزینه ارسال
        ///</summary>
        [StringValidation] public string DeliveryCost { get; set; }
        /// <summary>
        /// آیا قیمت پایه  اشتباه میباشد
        ///</summary>
        public bool? DeliveryCostIsInValid { get; set; }
        /// <summary>
        /// پیام خطا
        ///</summary>
        [StringValidation] public string ErrorMessage { get; set; }
        /// <summary>
        /// قیمت با تخفیف کتاب
        ///</summary>
        [StringValidation] public string BookBasePrice { get; set; }
        /// <summary>
        /// آیا قیمت با تخفیف کتاب اشتباه می باشد 
        ///</summary>
        public bool? BookBasePriceIsInValid { get; set; }
        /// <summary>
        /// آیا وضعیت انتشار مقدار اشتباه دارد 
        ///</summary>
        public bool? PublishedIsInValid { get; set; }
        /// <summary>
        /// دسته اصلی / زیر دسته
        ///</summary>
        [StringValidation] public string CategoryTitle { get; set; }
        /// <summary>
        /// دسته اصلی / زیر دسته اشتباه می باشد
        /// </summary>
        public bool? CategoryTitleIsInValid { get; set; }
        /// <summary>
        /// شناسه نوع ووچر
        ///</summary>
        public int? VocherTypeId { get; set; }
        /// <summary>
        /// نوع ووچر
        ///</summary>
        [StringValidation] public string VocherTypeTitle { get; set; }
        /// <summary>
        /// نمایش زیر نویس برنامه ریزی محصول
        /// </summary>
        public string ProductSalePlanningDisplaySubTitle { get; set; }

        /// <summary>
        ///         /// شناسه گروه محصول برای فاکتور و کانورت
        ///</summary>
        public string ProductGroupId { get; set; }
        public bool? ProductGroupIdIsInvalid { get; set; }


    }


    public class UploadedProductItemEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(600)]
        [StringValidation]
        [Required]
        public string Title { get; set; }
        [StringValidation] public string Description { get; set; }
        [StringLength(100)][StringValidation] public string Type { get; set; }
        [StringLength(100)][StringValidation] public string ParentCategory { get; set; }
        [StringLength(100)][StringValidation] public string ChildCategory { get; set; }
        public Int64? Price { get; set; }
        public Int64? DiscountAmount { get; set; }
        public Int64? PriceInclDiscount { get; set; }
        public Int64? TestPriceInclDiscount { get; set; }
        public Int64? BookPriceInclDiscount { get; set; }
        public int? DiscountPercentage { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
    }

    public class UploadedProductItemSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
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
        /// انتشار یافته
        /// </summary>
        public string Published { get; set; }
        /// <summary>
        /// پیام خطا
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// عنوان دسته بندی
        /// </summary>
        public string CategoryTitle { get; set; }
        /// <summary>
        /// نام کاربری مربی
        /// </summary>
        public string TeacherUserName { get; set; }
        /// <summary>
        /// نام کاربری دانشجو یا فراگیر
        /// </summary>
        public string StudentUserName { get; set; }
        /// <summary>
        /// نمایندگی یا گروه علمی
        /// </summary>
        public string Representation { get; set; }
        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public string EndDate { get; set; }

    }

    public class SampleUploadProductListViewModel
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
        /// نام دسته بندی اصلی
        /// </summary>
        [StringValidation] public string ParentCategory { get; set; }
        /// <summary>
        /// نام زیر دسته 
        /// </summary>
        [StringValidation] public string ChildCategory { get; set; }
        /// <summary>
        /// قیمت پایه
        /// </summary>
        [StringValidation] public string Price { get; set; }
        /// <summary>
        /// آیا قیمت پایه اشتباه میباشد
        ///</summary>
        public bool? PriceIsInValid { get; set; }
        /// <summary>
        /// مبلغ تخفیف
        /// </summary>
        [StringValidation] public string DiscountAmount { get; set; }
        /// <summary>
        /// آیا مبلغ تخفیف اشتباه میباشد
        ///</summary>
        public bool? DiscountAmountIsInValid { get; set; }
        /// <summary>
        /// قیمت با تخفیف آزمون
        /// </summary>
        [StringValidation] public string TestPriceInclDiscount { get; set; }
        /// <summary>
        /// قیمت با تخفیف کتاب
        /// </summary>
        [StringValidation] public string BookPriceInclDiscount { get; set; }
        /// <summary>
        /// آیا مبلغ با تخفیف آزمون اشتباه میباشد
        ///</summary>
        public bool? TestPriceInclDiscountIsInValid { get; set; }
        /// <summary>
        /// درصد تخفیف طرح باقرالعلوم
        ///</summary>
        [StringValidation] public string TbaoCustomerClubDiscountPercentage { get; set; }
        /// <summary>
        /// درصد تخفیف طرح باقرالعلوم معتبر نمی باشد
        ///</summary>
        public bool? TbaoCustomerClubDiscountPercentageIsInValid { get; set; }
        /// <summary>
        /// درصد تخفیف سایر باشگاه ها
        ///</summary>
        [StringValidation] public string OtherCustomerClubDiscountPercentage { get; set; }
        /// <summary>
        /// درصد تخفیف سایر باشگاه ها معتبر نمی باشد
        ///</summary>
        public bool? OtherCustomerClubDiscountPercentageIsInValid { get; set; }
        /// <summary>
        /// هزینه ارسال
        ///</summary>
        [StringValidation] public string DeliveryCost { get; set; }
        /// <summary>
        /// آیا هزینه ارسال اشتباه میباشد
        ///</summary>
        public bool? DeliveryCostIsInValid { get; set; }
        /// <summary>
        /// پیام خطا
        ///</summary>
        [StringValidation] public string ErrorMessage { get; set; }
        /// <summary>
        /// وضعیت حذف
        /// </summary>
        [StringValidation] public string Published { get; set; }
        /// <summary>
        ///         /// دسته اصلی / زیر دسته
        ///</summary>
        [StringValidation] public string Category { get; set; }
        /// <summary>
        ///         /// شناسه نوع ووچر
        ///</summary>
        public int? VocherTypeId { get; set; }
        /// <summary>
        ///         /// نوع ووچر
        ///</summary>
        [StringValidation] public string VocherTypeTitle { get; set; }

    }

    public class UploadProductListViewModel
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

    }

}
