using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.PagedList;

namespace Yashil.Core.CustomViewModels.ECommerce
{
    /// <summary>
    /// مدل پایه برای نمایش لیست برنامه ریزی دوره
    /// </summary>
    public class ProductSalesPlanningBaseListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public string ProductSalesPlanningTitle { get; set; }
        public string ProductTitle { get; set; }
        public string RepresentationTitle { get; set; }
        public bool? Confirmed { get; set; }

        public bool? Published { get; set; }

        public decimal? Price { get; set; }

        public bool IsFreeShipping { get; set; }


        public int StockQuantity { get; set; }


        public string CompanyTitle { get; set; }

        public int? EndDate { get; set; }
        /// <summary>
        ///         /// تاریخ شروع
        ///</summary>
        public int? StartDate { get; set; }

        public string CourseStatusTitle { get; set; }


        public string ImplementationTypeTitle { get; set; }
        public string CourseTypeTitle { get; set; }
        public string RunTypeTitle { get; set; }
        public string CustomGenderTitle { get; set; }
        public int MaxCapacity { get; set; }
        public int Duration { get; set; }
        public string CategoryTitle { get; set; }

        public string Telephone { get; set; }
    }


    public class ProductSalesPlanningListViewModel : ProductSalesPlanningBaseListViewModel
    {

        /// <summary>
        /// درصد تخفیف
        ///</summary>
        public decimal? DiscountPercentage { get; set; }

        public decimal ProductCost { get; set; }

        public int? DisplayOrder { get; set; }


        public string CoursePlanningSchedule { get; set; }

        public string TeacherTitle { get; set; }
        public string AgeCategoryTitle { get; set; }

        public int? CourseId { get; set; }
        public int? CoursePlanningId { get; set; }
        /// <summary>
        /// درصد تخفیف مربوط به تنظیم فروش
        /// </summary>

        // TODO Fix This Remove From ProductSalePlanning ******
        public decimal? SaleConfigurationDiscountPercentage { get; set; }
        /// <summary>
        /// درصد نهایی : اگر برنامه ریزی دوره دارای درصد تخفیف باشد به عنوان درصد نهایی برمیگردد
        /// اگر برنامه ریزی دوره دارای درصد تخفیف نباشد ، درصد تخفیف قرار داد به عنوان درصد تخفیف نهایی بر میگردد
        /// </summary>
        ///
        /// TODO Fix This Remove From ProductSalePlanning ******
        public decimal? FinalDiscountPercentage { get; set; }
        /// <summary>
        /// نمایش دکمه ی فیلتر کردن ایتم های فاکتور بر اساس شناسه بر نامه ریزی دوره در هنگام صدور فاکتور
        /// </summary>
        public bool ShowFilterByProductSalesPlanningBtn { get; set; }

        /// <summary>
        /// شناسه کلاس
        /// منظور از کلاس مثلا اتاق های یک ساختمان آموزشی است
        /// </summary>
        public int? ClassRoomId { get; set; }
        /// <summary>
        /// برنامه ریزی دوره به آیتم گروه هی اساین شده است
        /// </summary>
        public bool ProductSalesPlanningAssignedToItemGroup { get; set; }
        /// <summary>
        /// عنوان آیتم گروه برنامه ریزی دوره
        /// </summary>
        public string ProductSalesPlanningItemGroupTitle { get; set; }

        public bool Deleted { get; set; }
        /// <summary>
        /// آیا کاربر جاری ادمین است؟
        /// </summary>
        public bool IsAdmin { get; set; }

    }
    public class ProductSalesPlanningSimpleListViewModel
    {
        public int Id { get; set; }

        public string ProductTitle { get; set; }

        [StringValidation] public string RepresentationTitle { get; set; }

        public bool Confirmed { get; set; }

        public bool? Published { get; set; }

        public decimal? Price { get; set; }

        public bool IsFreeShipping { get; set; }
        public string CategoryTitle { get; set; }
        public int? CoursePlanningId { get; set; }
    }

    /// <summary>
    /// ویومدل پایه برای انجام عملیات
    /// Crud
    /// در موجودیت ProductSalesPlanning
    /// </summary>
    public class ProductSalesPlanningBaseEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(200)]
        [StringValidation]
        public string Title { get; set; }

        [Range(0, int.MaxValue)]
        public int? RepresentationId { get; set; }

        /// <summary>
        ///         /// وضعیت دوره
        ///</summary>
        public int? CourseStatus { get; set; }

        [Range(0, int.MaxValue)]
        public int? ProductId { get; set; }

        public decimal? Price { get; set; }

        /// <summary>
        ///         /// رده سنی
        ///</summary>
        public int? AgeCategory { get; set; }

        /// <summary>
        ///         /// نوع برگزاری
        ///</summary>
        public int? ImplementationType { get; set; }

        /// <summary>
        ///         /// نوع دوره
        ///</summary>
        public string CourseTypeTitle { get; set; }

        /// <summary>
        ///         /// نوع دوره
        ///</summary>
        public int? CourseType { get; set; }

        /// <summary>
        ///         /// روش اجرای دوره
        ///</summary>
        public int RunType { get; set; }

        [Range(0, int.MaxValue)]
        [Required] public int StartDate { get; set; }

        public int? EndDate { get; set; }

        /// <summary>
        /// در نظر گرفتن تاریخ پایان جهت نمایش در سایت 
        /// </summary>
        public bool ConsiderEndDate { get; set; }

        /// <summary>
        ///         /// جنسیت
        ///</summary>
        public int? CustomGender { get; set; }

        /// <summary>
        ///         /// حداکثر ظرفیت
        ///</summary>
        public int MaxCapacity { get; set; }


        /// <summary>
        ///         /// زمانبندی تشکیل کلاس
        ///</summary>
        [StringValidation] public string? CoursePlanningSchedule { get; set; }

        public int? Duration { get; set; }

        public bool? Published { get; set; }

        /// <summary>
        ///         /// غیر فعال کردن دکمه خرید
        ///</summary>
        public bool DisableBuyButton { get; set; }

        /// <summary>
        /// آیا پیش ثبت نام است
        /// </summary>
        public bool IsPreRegistration { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///         /// دوره سازمانی
        ///</summary>
        public bool Organizational { get; set; }
        /// <summary>
        /// تایید برنامه ریزی دوره
        /// </summary>
        public bool? Confirmed { get; set; }
        /// <summary>
        /// آیا فقط برای کاربران مرکز جاری نمایش داده شود؟
        /// </summary>
        public bool IsPublicOnlyForCurrentCompanyUsers { get; set; }
        /// <summary>
        /// آیا عمومی است؟
        /// </summary>
        public bool? IsPublic { get; set; }

        /// <summary>
        /// شناسه کلاس حضوری
        /// </summary>
        public int? ClassRoomId { get; set; }
        /// <summary>
        /// تعداد جلسات دوره
        /// </summary>
        public short? SessionCount { get; set; }
        /// <summary>
        /// مدت زمان جلسه
        /// </summary>
        public short? SessionDuration { get; set; }
        [StringValidation]
        public string StartDateString { get; set; }
        [StringValidation]
        public string EndDateString { get; set; }

        // کاربر براساس اولویت امتیاز هر آیتم را تعیین میکند.
        //مجموع این امتیازات نباید بیشتر از 100 باشد،
        #region   CoursePointsForLms
        /// <summary>
        ///  امتیاز کلاس
        /// </summary>
        public int? ClassPoint { get; set; }
        /// <summary>
        ///  امتیاز آزمون
        /// </summary>
        public int? ExamPoint { get; set; }
        /// <summary>
        ///  امتیاز تکالیف
        /// </summary>
        public int? HomeWorkPoint { get; set; }
        #endregion

        /// <summary>
        /// تخصیص برنامه ریزی دوره به تمامی نمایندگی ها
        /// </summary>
        public bool? AssignProductSalesPlanningToAllRepresentation { get; set; }

        /// <summary>
        /// دلیل رده برنامه ریزی دوره
        /// </summary>
        [StringValidation]
        public string RejectReason { get; set; }
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int? DisplayOrder { get; set; }
    }

    public class ProductSalesPlanningEditModel : ProductSalesPlanningBaseEditModel
    {
        public ProductEditModel Product { get; set; }

        public bool SalesByAllRepresentations { get; set; }
        public bool SalesBySelectedRepresentations { get; set; }
        public bool? AllowSelectByRepresentation { get; set; }


        [StringValidation]
        public string Code { get; set; }

        /// <summary>
        /// درصد تخفیف
        ///</summary>
        public decimal? DiscountPercentage { get; set; }




        [StringLength(800)]
        [StringValidation]
        public string Sku { get; set; }


        public decimal? CalcedPrice { get; set; }
        public bool IsFreeShipping { get; set; }
        public decimal? ProductCost { get; set; }
        public decimal? OldPrice { get; set; }

        [Required]
        public decimal AdditionalShippingCharge { get; set; }
        public int? WarehouseId { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int StockQuantity { get; set; }
        public int? ManageInventoryMethodType { get; set; }
        public int? ProductAvailabilityRangeType { get; set; }
        public int? DeliveryDateType { get; set; }
        public int? DisplayOrder { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }






        /// <summary>
        ///         /// زمانبندی تشکیل کلاس
        ///</summary>
        [StringValidation] public string? CoursePlanningScheduleFormula { get; set; }



        /// <summary>
        /// شناسه تنظیم فروش
        /// </summary>
        public int? SaleConfigurationId { get; set; }
        /// <summary>
        ///         /// نمایش در صفحه اصلی
        ///</summary>
        public bool ShowOnHomepage { get; set; }
        /// <summary>
        ///         /// ارسال رایگان
        ///</summary>
        public bool ForceFreeShipping { get; set; }
        /// <summary>
        ///         /// نمایش تعداد موجودی
        ///</summary>
        public bool? DisplayStockQuantity { get; set; }
        /// <summary>
        ///         /// نمایش موجود بودن در انبار
        ///</summary>
        public bool? DisplayStockAvailability { get; set; }
        /// <summary>
        ///         /// حداقل تعداد کالا سبد
        ///</summary>
        public int OrderMinimumQuantity { get; set; }
        /// <summary>
        ///         /// حداکثر تعداد کالا در سبد
        ///</summary>
        public int OrderMaximumQuantity { get; set; }


        /// <summary>
        ///         /// غیر قابل استرداد
        ///</summary>
        public bool NotReturnable { get; set; }

        /// <summary>
        ///         /// غیر فعال کردن افزودن به لیست علاقه مندی
        ///</summary>
        public bool DisableWishlistButton { get; set; }
        /// <summary>
        ///         /// پیش سفارش
        ///</summary>
        public bool AvailableForPreOrder { get; set; }
        /// <summary>
        ///         /// تاریخ شروع پیش سفارش
        ///</summary>
        public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }
        /// <summary>
        ///         /// تماس برای قیمت
        ///</summary>
        public bool CallForPrice { get; set; }

        /// <summary>
        ///         /// مشتری قیمت را وارد کند
        ///</summary>
        public bool CustomerEntersPrice { get; set; }
        /// <summary>
        ///         /// حداقل مقدار مشتری
        ///</summary>
        public decimal MinimumCustomerEnteredPrice { get; set; }
        /// <summary>
        ///         /// حداکثر مقدار مشتری
        ///</summary>
        public decimal MaximumCustomerEnteredPrice { get; set; }
        /// <summary>
        ///         /// قیمت پایه فعال
        ///</summary>
        public bool? BasePriceEnabled { get; set; }

        /// <summary>
        ///         /// انتخاب به عنوان جدید
        ///</summary>
        public bool MarkAsNew { get; set; }
        /// <summary>
        ///         /// تاریخ شروع به عنوان محصول جدید
        ///</summary>
        public DateTime? MarkAsNewStartDateTime { get; set; }
        /// <summary>
        ///         /// تاریخ  انتها به عنوان محصول جدید
        ///</summary>
        public DateTime? MarkAsNewEndDateTime { get; set; }

        /// <summary>
        /// آیا درصد تخفیف نهایی تغییر کرده است؟
        /// اگر کاربر درصد تخفیف برای برنامه ریزی دروه ثبت کند
        /// درصد تخفیف نهایی از برنامه ریزی دوره خوانده می شود
        /// و دیگر از قرار داد نمی خواند
        /// در این صورت مقدار این فیلد True می باشد 
        /// </summary>
        public bool HasFinalDiscountPercentageChanged { get; set; }

        public int RemainingCapacity { get; set; }

        // کاربر براساس اولویت امتیاز هر آیتم را تعیین میکند.
        //مجموع این امتیازات نباید بیشتر از 100 باشد،
        #region   CoursePointsForLms
        /// <summary>
        ///  امتیاز کلاس
        /// </summary>
        public int? ClassPoint { get; set; }
        /// <summary>
        ///  امتیاز آزمون
        /// </summary>
        public int? ExamPoint { get; set; }
        /// <summary>
        ///  امتیاز تکالیف
        /// </summary>
        public int? HomeWorkPoint { get; set; }


        #endregion


    }


    /// <summary>
    /// مدل دوره در قسمت ویرایش اطلاعات دوره 
    /// این مدل در قسمت  فرم دانشجویان دوره و رمانی که دوره موردنظر برای دانشجو انتخاب شده در گرید نمایش داده می شود 
    /// </summary>
    public class ProductSalesPlanningInfoListViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// عنوان محصول
        /// </summary>
        [StringValidation] public string ProductTitle { get; set; }
        /// <summary>
        /// عنوان نمایندگی
        /// </summary>
        [StringValidation] public string RepresentationTitle { get; set; }
        /// <summary>
        /// عنوان مرکز
        /// </summary>
        [StringValidation] public string CompanyTitle { get; set; }
        /// <summary>
        /// مدت دوره
        /// </summary>
        public int? Duration { get; set; }
        /// <summary>
        /// زمانبندی تشکیل کلاس
        /// </summary>
        [StringValidation] public string CoursePlanningSchedule { get; set; }
        /// <summary>
        /// تاریخ شروع دوره
        /// </summary>
        public int? StartDate { get; set; }
        /// <summary>
        /// تاریخ پایان دوره
        /// </summary>
        public int? EndDate { get; set; }
        /// <summary>
        /// نام مدرس
        /// </summary>
        // [StringValidation] public string RepresentationTeacherTitle { get; set; }
        /// <summary>
        /// نوع برگزاری دوره
        /// </summary>
        [StringValidation] public string ImplementationTypeTitle { get; set; }
        /// <summary>
        /// نوع دوره
        /// </summary>
        [StringValidation] public string CourseTypeTitle { get; set; }
        /// <summary>
        /// روش اجرای دوره
        /// </summary>
        [StringValidation] public string RunTypeTitle { get; set; }
        /// <summary>
        /// جنسیت
        /// </summary>
        [StringValidation] public string CustomGenderTitle { get; set; }
    }

    /// <summary>
    /// مدل دوره انتخاب شده برای یک دانشجو 
    /// این مدل در قسمت  فرم دانشجویان دوره و رمانی که دوره موردنظر برای دانشجو انتخاب شده در کامپوننت این اطلاعات نمایش داده می شود 
    /// </summary>
    public class ProductSalesPlanningInfoViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// عنوان برنامه ریزی دوره
        /// </summary>
        public string ProductSalesPlanningTitle { get; set; }
        /// <summary>
        /// عنوان محصول
        /// </summary>
        [StringValidation] public string ProductTitle { get; set; }

        [StringValidation] public string CategoryTitle { get; set; }

        /// <summary>
        /// عنوان نمایندگی
        /// </summary>
        [StringValidation] public string RepresentationTitle { get; set; }
        /// <summary>
        /// عنوان مرکز
        /// </summary>
        [StringValidation] public string CompanyTitle { get; set; }
        /// <summary>
        /// مدت دوره
        /// </summary>
        public int? Duration { get; set; }
        /// <summary>
        /// زمانبندی تشکیل کلاس
        /// </summary>
        [StringValidation] public string CoursePlanningSchedule { get; set; }
        /// <summary>
        /// تاریخ شروع دوره
        /// </summary>
        public int? StartDate { get; set; }
        /// <summary>
        /// تاریخ پایان دوره
        /// </summary>
        public int? EndDate { get; set; }
        /// <summary>
        /// نام مدرس
        /// </summary>
        // [StringValidation] public string RepresentationTeacherTitle { get; set; }
        /// <summary>
        /// نوع برگزاری دوره
        /// </summary>
        [StringValidation] public string ImplementationTypeTitle { get; set; }
        /// <summary>
        /// نوع دوره
        /// </summary>
        [StringValidation] public string CourseTypeTitle { get; set; }
        /// <summary>
        /// روش اجرای دوره
        /// </summary>
        [StringValidation] public string RunTypeTitle { get; set; }
        /// <summary>
        /// جنسیت
        /// </summary>
        [StringValidation] public string CustomGenderTitle { get; set; }

    }

    public class ProductSalesPlanningSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
        public int? RepresentationId { get; set; }
        public int? CoursePlanningId { get; set; }
    }
    public class ProductSalesPlanningCustomViewModel
    {
        public int? RepresentationId { get; set; }
        [StringValidation] public string RepresentationTitle { get; set; }
        public decimal SataSubsidy { get; set; }
        public decimal VezaratSubsidy { get; set; }
        [StringValidation] public string CityTitle { get; set; }
        [StringValidation] public string StateTitle { get; set; }
        public int? CoursePlanningId { get; set; }
        public int? Duration { get; set; }
        public int? EndDate { get; set; }
        /// <summary>
        ///         /// تاریخ شروع
        ///</summary>
        public int? StartDate { get; set; }
        [StringValidation] public string CoursePlanningSchedule { get; set; }
        public int? InstallmentCount { get; set; }
        /// <summary>
        /// شناسه جنسیت
        /// </summary>
        public int CustomGender { get; set; }
        /// <summary>
        /// عنوان جنسیت
        /// </summary>
        [StringValidation] public string CustomGenderTitle { get; set; }
        /// <summary>
        // ظرفیت کل
        /// </summary>
        public int Capacity { get; set; }
        /// <summary>
        /// ظرفیت باقی مانده
        /// </summary>
        public int? RemainingCapacity { get; set; }
    }

    public class ProductSalesPlanningCustomSimpleListViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// عنوان محصول
        /// </summary>
        [StringValidation] public string ProductTitle { get; set; }
        /// <summary>
        /// عنوان نمایندگی
        /// </summary>
        [StringValidation] public string RepresentationTitle { get; set; }

        public int RepresentationId { get; set; }
        /// <summary>
        /// عنوان مرکز
        /// </summary>
        public string CompanyTitle { get; set; }

        public int CompanyId { get; set; }
        public string CategoryTitle { get; set; }
        public string ApplicationUrl { get; set; }
        public int ApplicationId { get; set; }
        public decimal? Price { get; set; }
        public decimal? CalcedPrice { get; set; }
        public int? MaxSataSubsidy { get; set; }
        public int? MaxVezaratSubsidy { get; set; }
        public string CityTitle { get; set; }
        public string StateTitle { get; set; }
    }

    public class ProductSalesPlanningSimpleEditModel
    {
        public int Id { get; set; }
    }

    public class GetProductSalePlanningSimpleListByFilterViewModel
    {
        public List<ProductSalesPlanningCustomSimpleListViewModel> productSalesPlannings { get; set; }
        public Page Page { get; set; }
    }

    /// <summary>
    /// ویو مدل مورد استفاده برای دریافت دوره های در حال برگذاری
    /// </summary>
    public class OngoingProductSalesPlanningViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        /// <summary>
        /// شناسه productSalesPlanning
        /// </summary>
        public int Id { get; set; }
        public int? RepresentationId { get; set; }
        public string CompanyTitle { get; set; }
        public string RepresentationTitle { get; set; }
        /// <summary>
        /// تعداد دوره های برگزار شده
        /// </summary>
        public int OngoingProductSalesPlanningCount { get; set; }
        public int CompanyId { get; set; }
        /// <summary>
        /// تعداد کل آزمون دوره
        /// </summary>
        public int ExamCount { get; set; }
        /// <summary>
        /// تعداد کل شرکت کننده در آزمون ها
        /// </summary>
        public int ParticipatedUserExamCount { get; set; }
        /// <summary>
        /// تعداد قبول شده ها در آزمون
        /// </summary>
        public int PassedCount { get; set; }
        /// <summary>
        /// تعداد رد شده ها در آزمون
        /// </summary>
        public int FailedCount { get; set; }

        /// <summary>
        /// تعداد آزمون های تعیین تکلیف نشده یا نامشخص
        /// </summary>
        public int UnknownExamCount
        {
            get
            {
                if (ParticipatedUserExamCount == 0)
                    return 0;

                return ParticipatedUserExamCount - (PassedCount + FailedCount);
            }

        }

    }


    /// <summiry>
    /// مورد استفاده در GroupBy
    /// بر اساس شناسه مرکز و نمایندگی
    /// برای دریافت دوره های در حال برگزاری
    /// </summary>
    public class OngoingProductSalesPlanningGroupKey
    {
        public int CompanyId { get; set; }
        public int? RepresentationId { get; set; }
    }

    /// <summary>
    /// مدل گزارش جامع
    ///  دوره های مربوط به یک مرکز
    /// </summary>
    public class CompanyProductSalesPlanningComprehensiveReportViewModel
    {
        /// <summary>
        /// شناسه برنامه ریزی دوره
        /// </summary>
        public int Id { get; set; }
        public string ProductTitle { get; set; }
        public int ProductSalesPlanningStudentCount { get; set; }
        public int VirtualClassCount { get; set; }
        public int ExamCount { get; set; }
        public int HomeWorkCount { get; set; }
    }


    public class ProductSalesPlanningAssignedViewModel
    {
        /// <summary>
        /// شناسه دوره
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// عنوان دوره
        /// </summary>
        public string ProductTitle { get; set; }
    }

    /// <summary>
    ///  مدل داده های مورد نیاز برای نمایش در داشبورد تحلیلی
    /// </summary>
    public class LastestProductSalesPlaningForShowInAnalyticsDashboard
    {
        /// <summary>
        /// عنوان دوره
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// عنوان مرکز
        /// </summary>
        public string CompanyTitle { get; set; }
        /// <summary>
        /// عنوان نمایندگی
        /// </summary>
        public string RepresentationTitle { get; set; }

        /// <summary>
        /// آیا کاربر نقش استاد دارد؟
        /// </summary>
        public bool IsTeacher { get; set; }
    }

    public class StudentProductSalesPlanningViewModel
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// نام دوره
        /// </summary>
        [StringValidation] public string ProductTitle { get; set; }
        /// <summary>
        /// تاریخ شروع دوره
        /// </summary>
        public int? StartDate { get; set; }
        /// <summary>
        /// تاریخ پایان دوره
        /// </summary>
        public int? EndDate { get; set; }
        /// <summary>
        /// عنوان مرکز
        /// </summary>
        public string CompanyTitle { get; set; }
        /// <summary>
        /// عنوان نمایندگی
        /// </summary>
        public string RepresentationTitle { get; set; }
    }
}
