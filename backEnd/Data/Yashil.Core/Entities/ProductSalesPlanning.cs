using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ProductSalesPlanning  :IBaseEntity<int>,IApplicationBasedEntity
    {
public ProductSalesPlanning()
{
    ClassRoomReservation = new HashSet<ClassRoomReservation>();
    ClassSession = new HashSet<ClassSession>();
    DependentProductSaleMainProductSalePlanning = new HashSet<DependentProductSale>();
    DependentProductSaleRelatedProductSalePlanning = new HashSet<DependentProductSale>();
    ExamCategory = new HashSet<ExamCategory>();
    HomeWorkCategory = new HashSet<HomeWorkCategory>();
    MediaCategory = new HashSet<MediaCategory>();
    OrderItem = new HashSet<OrderItem>();
    OrderItemDependentProductSale = new HashSet<OrderItemDependentProductSale>();
    PreRegistration = new HashSet<PreRegistration>();
    ProductSalesPlanningConversation = new HashSet<ProductSalesPlanningConversation>();
    ProductSalesPlanningDetail = new HashSet<ProductSalesPlanningDetail>();
    ProductSalesPlanningDiscount = new HashSet<ProductSalesPlanningDiscount>();
    ProductSalesPlanningItemGroup = new HashSet<ProductSalesPlanningItemGroup>();
    ProductSalesPlanningReview = new HashSet<ProductSalesPlanningReview>();
    ProductSalesPlanningStudent = new HashSet<ProductSalesPlanningStudent>();
    ProductSalesPlanningTeacher = new HashSet<ProductSalesPlanningTeacher>();
    ShoppingCartItem = new HashSet<ShoppingCartItem>();
    VirtualClass = new HashSet<VirtualClass>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// محصول
///</summary>
public int ProductId { get; set; }
/// <summary>
///         /// نمایندگی
///</summary>
public int? RepresentationId { get; set; }
/// <summary>
///         /// شناسه کالا در انبار
///</summary>
public string Sku { get; set; }
/// <summary>
///         /// تایید شده
///</summary>
public bool? Confirmed { get; set; }
/// <summary>
///         /// دلیل عدم تایید
///</summary>
public string RejectReason { get; set; }
/// <summary>
///         /// منتشر شده
///</summary>
public bool Published { get; set; }
/// <summary>
///         /// قیمت
///</summary>
public decimal? Price { get; set; }
/// <summary>
///         /// ارسال رایگان
///</summary>
public bool IsFreeShipping { get; set; }
/// <summary>
///         /// هزینه محصول
///</summary>
public decimal? ProductCost { get; set; }
/// <summary>
///         /// قیمت قدیم
///</summary>
public decimal? OldPrice { get; set; }
/// <summary>
///         /// شارژ اضافه شده به هزینه ارسال
///</summary>
public decimal AdditionalShippingCharge { get; set; }
/// <summary>
///         /// انبار
///</summary>
public int? WarehouseId { get; set; }
/// <summary>
///         /// موجودی کالا
///</summary>
public int StockQuantity { get; set; }
/// <summary>
///         /// مدیریت روش موجودی
///</summary>
public int? ManageInventoryMethodType { get; set; }
/// <summary>
///         /// زمان تامین کالا
///</summary>
public int? ProductAvailabilityRangeType { get; set; }
/// <summary>
///         /// تخمین زمان تحویل کالا
///</summary>
public int? DeliveryDateType { get; set; }
/// <summary>
///         /// ترتیب نمایش
///</summary>
public int? DisplayOrder { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// ایجاد کننده
///</summary>
public int CreateBy { get; set; }
/// <summary>
///         /// ویرایش کننده
///</summary>
public int? ModifyBy { get; set; }
/// <summary>
///         /// زمان ایجاد
///</summary>
public DateTime CreationDate { get; set; }
/// <summary>
///         /// زمان تغییر
///</summary>
public DateTime? ModificationDate { get; set; }
/// <summary>
///         /// برنامه
///</summary>
public int ApplicationId { get; set; }
/// <summary>
///         /// سطح دسترسی
///</summary>
public int AccessLevelId { get; set; }
/// <summary>
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
/// <summary>
///         /// سازمان ایجاد کننده رکورد
///</summary>
public int CreatorOrganizationId { get; set; }
/// <summary>
///         /// بیشترین یارانه به ازای این کالا
///</summary>
public decimal? CalcedMaxSubsidySata { get; set; }
/// <summary>
///         /// بیشترین یارانه به ازای این کالا
///</summary>
public decimal? CalcedMaxSubsidyVezarat { get; set; }
public int? MaxSataSubsidy { get; set; }
public int? MaxVezaratSubsidy { get; set; }
public string EndDateString { get; set; }
public string StartDateString { get; set; }
/// <summary>
///         /// تاریخ پایان
///</summary>
public int? EndDate { get; set; }
/// <summary>
///         /// تاریخ شروع
///</summary>
public int? StartDate { get; set; }
/// <summary>
///         /// حداکثر ظرفیت
///</summary>
public int MaxCapacity { get; set; }
/// <summary>
///         /// دوره سازمانی
///</summary>
public bool Organizational { get; set; }
/// <summary>
///         /// وضعیت دوره
///</summary>
public int? CourseStatus { get; set; }
/// <summary>
///         /// زمانبندی تشکیل کلاس
///</summary>
public string CoursePlanningScheduleFormula { get; set; }
/// <summary>
///         /// زمانبندی تشکیل کلاس
///</summary>
public string CoursePlanningSchedule { get; set; }
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
public int? CourseType { get; set; }
/// <summary>
///         /// روش اجرای دوره
///</summary>
public int RunType { get; set; }
/// <summary>
///         /// جنسیت
///</summary>
public int? CustomGender { get; set; }
public int? Duration { get; set; }
/// <summary>
///         /// غیر فعال کردن افزودن به لیست علاقه مندی
///</summary>
public bool DisableWishlistButton { get; set; }
/// <summary>
///         /// غیر فعال کردن دکمه خرید
///</summary>
public bool DisableBuyButton { get; set; }
/// <summary>
///         /// نمایش تعداد موجودی
///</summary>
public bool DisplayStockQuantity { get; set; }
/// <summary>
///         /// نمایش موجود بودن در انبار
///</summary>
public bool DisplayStockAvailability { get; set; }
/// <summary>
///         /// حداقل تعداد کالا سبد
///</summary>
public int OrderMinimumQuantity { get; set; }
/// <summary>
///         /// حداکثر تعداد کالا در سبد
///</summary>
public int OrderMaximumQuantity { get; set; }
/// <summary>
///         /// ارسال رایگان
///</summary>
public bool ForceFreeShipping { get; set; }
/// <summary>
///         /// تماس برای قیمت
///</summary>
public bool CallForPrice { get; set; }
/// <summary>
///         /// تاریخ  انتها به عنوان محصول جدید
///</summary>
public DateTime? MarkAsNewEndDateTime { get; set; }
/// <summary>
///         /// غیر قابل استرداد
///</summary>
public bool NotReturnable { get; set; }
/// <summary>
///         /// پیش سفارش
///</summary>
public bool AvailableForPreOrder { get; set; }
/// <summary>
///         /// تاریخ شروع پیش سفارش
///</summary>
public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }
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
public bool BasePriceEnabled { get; set; }
/// <summary>
///         /// انتخاب به عنوان جدید
///</summary>
public bool MarkAsNew { get; set; }
/// <summary>
///         /// تاریخ شروع به عنوان محصول جدید
///</summary>
public DateTime? MarkAsNewStartDateTime { get; set; }
/// <summary>
///         /// نمایش در صفحه اصلی
///</summary>
public bool ShowOnHomepage { get; set; }
public int? TbaoId { get; set; }
public int SaleConfigurationId { get; set; }
/// <summary>
///         /// نمایش قیمت
///</summary>
public bool? ShowPrice { get; set; }
/// <summary>
///         /// نیازمندی های سخت افزاری
///</summary>
public string HardwareRequirement { get; set; }
/// <summary>
///         /// نیازمندی های نرم افزاری
///</summary>
public string SoftwareRequirement { get; set; }
public string UsedFor { get; set; }
/// <summary>
///         /// سطح و طبقه بندی دوره
///</summary>
public int? CourseLevel { get; set; }
/// <summary>
///         /// اجازه به اخذ توسط دیگر دانشکده ها
///</summary>
public bool? AllowedToTakeByOther { get; set; }
/// <summary>
///         /// نمایش دوره ها برای تمام مراکز
///</summary>
public bool IsPublic { get; set; }
public bool ConsiderEndDate { get; set; }
/// <summary>
///         /// شناسه گروه
///</summary>
public int? ProductSalesPlanningGroupId { get; set; }
public int? RemainingCapacity { get; set; }
/// <summary>
///         /// فرمت زیرنویس
///</summary>
public string SubTitleFormat { get; set; }
/// <summary>
///         /// نمایش زیرنویس
///</summary>
public bool DisplaySubTitle { get; set; }
public bool IsPreRegistration { get; set; }
public string DownloadUrl { get; set; }
/// <summary>
///         /// سقف یارانه ساتا طرح اشتغال
///</summary>
public int? MaxEmploymentSataSubsidy { get; set; }
/// <summary>
///         /// سقف یارانه وزارت طرح اشتغال
///</summary>
public int? MaxEmploymentVezaratSubsidy { get; set; }
/// <summary>
///         /// نمایش دوره ها فقط برای کاربران مرکز جاری
///</summary>
public bool IsPublicOnlyForCurrentCompanyUsers { get; set; }
public int? ClassRoomId { get; set; }
public short? SessionCount { get; set; }
public short? SessionDuration { get; set; }
public int? ClassPoint { get; set; }
public int? ExamPoint { get; set; }
public int? HomeWorkPoint { get; set; }
public bool? AssignProductSalesPlanningToAllRepresentation { get; set; }
public string Title { get; set; }

    public virtual CommonBaseData AgeCategoryNavigation { get; set; }
    public virtual ClassRoom ClassRoom { get; set; }
    public virtual CommonBaseData CourseLevelNavigation { get; set; }
    public virtual CommonBaseData CourseStatusNavigation { get; set; }
    public virtual CommonBaseData CourseTypeNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual CommonBaseData CustomGenderNavigation { get; set; }
    public virtual CommonBaseData DeliveryDateTypeNavigation { get; set; }
    public virtual CommonBaseData ImplementationTypeNavigation { get; set; }
    public virtual CommonBaseData ManageInventoryMethodTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Product Product { get; set; }
    public virtual CommonBaseData ProductAvailabilityRangeTypeNavigation { get; set; }
    public virtual ItemGroup ProductSalesPlanningGroup { get; set; }
    public virtual Representation Representation { get; set; }
    public virtual CommonBaseData RunTypeNavigation { get; set; }
    public virtual SaleConfiguration SaleConfiguration { get; set; }
public virtual ICollection<ClassRoomReservation>
    ClassRoomReservation { get; set; }
public virtual ICollection<ClassSession>
    ClassSession { get; set; }
public virtual ICollection<DependentProductSale>
    DependentProductSaleMainProductSalePlanning { get; set; }
public virtual ICollection<DependentProductSale>
    DependentProductSaleRelatedProductSalePlanning { get; set; }
public virtual ICollection<ExamCategory>
    ExamCategory { get; set; }
public virtual ICollection<HomeWorkCategory>
    HomeWorkCategory { get; set; }
public virtual ICollection<MediaCategory>
    MediaCategory { get; set; }
public virtual ICollection<OrderItem>
    OrderItem { get; set; }
public virtual ICollection<OrderItemDependentProductSale>
    OrderItemDependentProductSale { get; set; }
public virtual ICollection<PreRegistration>
    PreRegistration { get; set; }
public virtual ICollection<ProductSalesPlanningConversation>
    ProductSalesPlanningConversation { get; set; }
public virtual ICollection<ProductSalesPlanningDetail>
    ProductSalesPlanningDetail { get; set; }
public virtual ICollection<ProductSalesPlanningDiscount>
    ProductSalesPlanningDiscount { get; set; }
public virtual ICollection<ProductSalesPlanningItemGroup>
    ProductSalesPlanningItemGroup { get; set; }
public virtual ICollection<ProductSalesPlanningReview>
    ProductSalesPlanningReview { get; set; }
public virtual ICollection<ProductSalesPlanningStudent>
    ProductSalesPlanningStudent { get; set; }
public virtual ICollection<ProductSalesPlanningTeacher>
    ProductSalesPlanningTeacher { get; set; }
public virtual ICollection<ShoppingCartItem>
    ShoppingCartItem { get; set; }
public virtual ICollection<VirtualClass>
    VirtualClass { get; set; }
    }
 }
