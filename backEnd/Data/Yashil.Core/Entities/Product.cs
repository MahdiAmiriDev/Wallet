using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Product  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Product()
{
    ProductCategory = new HashSet<ProductCategory>();
    ProductSalesPlanning = new HashSet<ProductSalesPlanning>();
    ProductSpecificationOption = new HashSet<ProductSpecificationOption>();
    ProductTags = new HashSet<ProductTags>();
    QuestionCategory = new HashSet<QuestionCategory>();
    WalletScope = new HashSet<WalletScope>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// مدل فروش
///</summary>
public int SalesModelType { get; set; }
/// <summary>
///         /// ورود قیمت توسط نمایندگی
///</summary>
public bool AllowSetPriceByRepresentation { get; set; }
/// <summary>
///         /// مدیریت موجودی توسط نمایندگی
///</summary>
public bool AllowManageInventoryByRepresentation { get; set; }
/// <summary>
///         /// مدیریت تخفیف توسط نمایندگی
///</summary>
public bool AllowManageDiscountByRepresentation { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// ارسال رایگان
///</summary>
public bool IsFreeShipping { get; set; }
/// <summary>
///         /// گروه کالای فروش گروهی
///</summary>
public bool IsGroupProduct { get; set; }
/// <summary>
///         /// فروش توسط چند نماینده
///</summary>
public bool AllowMultiVendor { get; set; }
/// <summary>
///         /// مرکز
///</summary>
public int CompanyId { get; set; }
/// <summary>
///         /// نوع محصول
///</summary>
public int AppEntityId { get; set; }
/// <summary>
///         /// قابل مشاهده به صورت جداگانه
///</summary>
public bool VisibleIndividually { get; set; }
/// <summary>
///         /// توضیحات کوتاه
///</summary>
public string ShortDescription { get; set; }
/// <summary>
///         /// توضیحات کامل
///</summary>
public string FullDescription { get; set; }
/// <summary>
///         /// توضیح مدیر
///</summary>
public string AdminComment { get; set; }
public int? ProductTemplateId { get; set; }
/// <summary>
///         /// کلمات کلیدی
///</summary>
public string MetaKeywords { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string MetaDescription { get; set; }
public string MetaTitle { get; set; }
/// <summary>
///         /// اجازه ثبت نظر
///</summary>
public bool AllowCustomerReviews { get; set; }
/// <summary>
///         /// امتیاز مشتری به محصول
///</summary>
public int ApprovedRatingSum { get; set; }
/// <summary>
///         /// مجموع امتیازات منفی مشتریان
///</summary>
public int NotApprovedRatingSum { get; set; }
public bool SubjectToAcl { get; set; }
/// <summary>
///         /// محدود به فروشگاه
///</summary>
public bool LimitedToStores { get; set; }
/// <summary>
///         /// شماره تجاری جهانی
///</summary>
public string Gtin { get; set; }
/// <summary>
///         /// کد کارخانه
///</summary>
public string ManufacturerPartNumber { get; set; }
/// <summary>
///         /// کارت هدیه
///</summary>
public bool IsGiftCard { get; set; }
/// <summary>
///         /// نوع کارت هدیه
///</summary>
public int? GiftCardTypeId { get; set; }
public decimal? OverriddenGiftCardAmount { get; set; }
public bool RequireOtherProducts { get; set; }
public string RequiredProductIds { get; set; }
public bool AutomaticallyAddRequiredProducts { get; set; }
/// <summary>
///         /// محصولات قابل دانلود
///</summary>
public bool IsDownload { get; set; }
/// <summary>
///         /// محصول دانلودی
///</summary>
public int? DownloadId { get; set; }
/// <summary>
///         /// دانلود نامحدود
///</summary>
public bool UnlimitedDownloads { get; set; }
/// <summary>
///         /// حداکثر دانلود ها
///</summary>
public int MaxNumberOfDownloads { get; set; }
/// <summary>
///         /// تعداد روز دانلود
///</summary>
public int? DownloadExpirationDays { get; set; }
/// <summary>
///         /// نوع فعال سازی دانلود
///</summary>
public int? DownloadActivationType { get; set; }
/// <summary>
///         /// دارای فایل نمونه
///</summary>
public bool HasSampleDownload { get; set; }
/// <summary>
///         /// فایل نمونه
///</summary>
public int? SampleDownloadId { get; set; }
/// <summary>
///         /// دارای توافقنامه کاربری
///</summary>
public bool HasUserAgreement { get; set; }
/// <summary>
///         /// متن توافقنامه کاربری
///</summary>
public string UserAgreementText { get; set; }
/// <summary>
///         /// دوره ای
///</summary>
public bool IsRecurring { get; set; }
public int RecurringCycleLength { get; set; }
public int? RecurringCyclePeriodType { get; set; }
public int RecurringTotalCycles { get; set; }
/// <summary>
///         /// اجاره ای
///</summary>
public bool IsRental { get; set; }
/// <summary>
///         /// طول دوره اجاره
///</summary>
public int RentalPriceLength { get; set; }
/// <summary>
///         /// دوره اجاره
///</summary>
public int? RentalPricePeriodId { get; set; }
/// <summary>
///         /// ارسال فعال شده
///</summary>
public bool IsShipEnabled { get; set; }
/// <summary>
///         /// ارسال به صورت جداگانه
///</summary>
public bool ShipSeparately { get; set; }
/// <summary>
///         /// معاف از مالیات
///</summary>
public bool IsTaxExempt { get; set; }
/// <summary>
///         /// دسته مالیات
///</summary>
public int TaxCategoryId { get; set; }
/// <summary>
///         /// ارتباطات، پخش و خدمات الکترونیکی
///</summary>
public bool IsTelecommunicationsOrBroadcastingOrElectronicServices { get; set; }
public int? MinStockQuantity { get; set; }
public int? LowStockActivityId { get; set; }
public int? NotifyAdminForQuantityBelow { get; set; }
public int? BackorderModeId { get; set; }
public bool AllowBackInStockSubscriptions { get; set; }
/// <summary>
///         /// استفاده از چند انبار
///</summary>
public bool UseMultipleWarehouses { get; set; }
/// <summary>
///         /// بسته تعداد
///</summary>
public string AllowedQuantities { get; set; }
public bool AllowAddingOnlyExistingAttributeCombinations { get; set; }
public decimal? BasePriceAmount { get; set; }
public int? BasePriceUnitType { get; set; }
public decimal? BasePriceBaseAmount { get; set; }
public int? BasePriceBaseUnitType { get; set; }
public bool HasTierPrices { get; set; }
public bool HasDiscountsApplied { get; set; }
/// <summary>
///         /// وزن
///</summary>
public decimal Weight { get; set; }
/// <summary>
///         /// طول
///</summary>
public decimal Length { get; set; }
/// <summary>
///         /// عرض
///</summary>
public decimal Width { get; set; }
/// <summary>
///         /// ارتفاع
///</summary>
public decimal Height { get; set; }
/// <summary>
///         /// تاریخ شروع در دسترس بودن
///</summary>
public DateTime? AvailableStartDate { get; set; }
/// <summary>
///         /// تاریخ انتهای در دسترس بودن
///</summary>
public DateTime? AvailableEndDate { get; set; }
/// <summary>
///         /// ترتیب نمایش
///</summary>
public int DisplayOrder { get; set; }
/// <summary>
///         /// انتشار داده شده
///</summary>
public bool Published { get; set; }
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
public int? FanId { get; set; }
/// <summary>
///         /// ضریب اعمال یارانه ساتا
///</summary>
public decimal SataSubsidyFactor { get; set; }
/// <summary>
///         /// ضریب اعمال یارانه وزارت
///</summary>
public decimal VezaratSubsidyFactor { get; set; }
/// <summary>
///         /// مدت دوره(ساعت)
///</summary>
public int? Duration { get; set; }
/// <summary>
///         /// کد دوره
///</summary>
public int? CourseId { get; set; }
/// <summary>
///         /// نوع ووچر
///</summary>
public int? VocherTypeId { get; set; }
/// <summary>
///         /// حداکثر قیمت مجاز برای فروش
///</summary>
public decimal MaxPrice { get; set; }
/// <summary>
///         /// حداقل قیمت مجاز برای فروش
///</summary>
public decimal MinPrice { get; set; }
public string MainCode { get; set; }
public string ExternalLink { get; set; }
public int DefaultCategoryId { get; set; }
public int ProductType { get; set; }
/// <summary>
///         /// ????? ???? ??????
///</summary>
public string TitleForFactor { get; set; }
/// <summary>
///         /// برای دریافت محصول نیاز به ارجاع به سایت مرکز می باشد؟
///</summary>
public bool? NeedRedirectToCompanySiteForReceive { get; set; }
/// <summary>
///         /// متن دکمه نیاز به تایید تامین کننده قبل از پرداخت در صفحه پیش فاکتور
///</summary>
public string ButtonTextRequireSupplierApprovalBeforePayment { get; set; }
/// <summary>
///         /// پیام به کاربر در تایید تامین کننده قبل از پرداخت در صفحه پیش فاکتور
///</summary>
public string MessageToUserInRequireSupplierApprovalBeforePayment { get; set; }
/// <summary>
///         /// اگر این فیلد مقدارتایید داشته باشد : در صفحه پیش فاکتور کاربر دکمه خرید را نمی بیند و دکمه ای را جهت تایید تامین کننده قبل از پرداخت می بیند و وقتی روی این دکمه کلیک کند منتظر تایید تامین کننده برای پرداخت می ماند
///</summary>
public bool RequireSupplierApprovalBeforePayment { get; set; }
public int? ProductGroupId { get; set; }
public int? ProductGroupIdIncludeTax { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual AppEntity AppEntity { get; set; }
    public virtual Application Application { get; set; }
    public virtual CommonBaseData BasePriceBaseUnitTypeNavigation { get; set; }
    public virtual CommonBaseData BasePriceUnitTypeNavigation { get; set; }
    public virtual Company Company { get; set; }
    public virtual Course Course { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual Category DefaultCategory { get; set; }
    public virtual CommonBaseData DownloadActivationTypeNavigation { get; set; }
    public virtual GiftCardType GiftCardType { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual ProductGroup ProductGroup { get; set; }
    public virtual ProductGroup ProductGroupIdIncludeTaxNavigation { get; set; }
    public virtual ProductTemplate ProductTemplate { get; set; }
    public virtual CommonBaseData ProductTypeNavigation { get; set; }
    public virtual CommonBaseData RecurringCyclePeriodTypeNavigation { get; set; }
    public virtual CommonBaseData SalesModelTypeNavigation { get; set; }
    public virtual TaxCategory TaxCategory { get; set; }
    public virtual VocherType VocherType { get; set; }
public virtual ICollection<ProductCategory>
    ProductCategory { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanning { get; set; }
public virtual ICollection<ProductSpecificationOption>
    ProductSpecificationOption { get; set; }
public virtual ICollection<ProductTags>
    ProductTags { get; set; }
public virtual ICollection<QuestionCategory>
    QuestionCategory { get; set; }
public virtual ICollection<WalletScope>
    WalletScope { get; set; }
    }
 }
