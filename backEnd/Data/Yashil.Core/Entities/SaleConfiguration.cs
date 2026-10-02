using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class SaleConfiguration  :IBaseEntity<int>,IApplicationBasedEntity
    {
public SaleConfiguration()
{
    Contract = new HashSet<Contract>();
    Factor = new HashSet<Factor>();
    OrderItem = new HashSet<OrderItem>();
    ProductSalesPlanning = new HashSet<ProductSalesPlanning>();
    SaleConfigurationAttribute = new HashSet<SaleConfigurationAttribute>();
    TaxCategorySaleConfiguration = new HashSet<TaxCategorySaleConfiguration>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// فعال
///</summary>
public bool IsActive { get; set; }
/// <summary>
///         /// شماره مجوز
///</summary>
public string LicenseNumber { get; set; }
/// <summary>
///         /// تاریخ شروع
///</summary>
public int FromDate { get; set; }
/// <summary>
///         /// تاریخ پایان
///</summary>
public int ToDate { get; set; }
/// <summary>
///         /// تاریخ انقاضای مجوز
///</summary>
public int? LicenseExpireDate { get; set; }
/// <summary>
///         /// نمایندگی
///</summary>
public int? RepresentationId { get; set; }
/// <summary>
///         /// مرکز
///</summary>
public int CompanyId { get; set; }
/// <summary>
///         /// اجازه تعیین مالیات در تعریف محصول
///</summary>
public bool AllowSetTaxInProduct { get; set; }
/// <summary>
///         /// معاف از مالیات
///</summary>
public bool IsTaxExempt { get; set; }
/// <summary>
///         /// مؤسس
///</summary>
public int? FounderId { get; set; }
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
///         /// نوع گزارش فتکتور-کلی یا با جزییات
///</summary>
public int? FactorReportType { get; set; }
public int? TbaoId { get; set; }
/// <summary>
///         /// درصد کسورات بیمه
///</summary>
public decimal InsurancePercentage { get; set; }
/// <summary>
///         /// دارای گواهی مالیات بر ارزش افزوده
///</summary>
public bool HasTaxCertificate { get; set; }
public int? YashilSchedulerId { get; set; }
/// <summary>
///         /// اطلاعات حساب جهت تسویه صورت حساب
///</summary>
public int? PersonBankAccountId { get; set; }
public string FactorItemPrefix { get; set; }
public bool? ForcedToGenerateFactorByTaxCategory { get; set; }
public bool IsGroupedByTaxCategory { get; set; }
/// <summary>
///         /// آیا سایر کسورات (هزینه تبلیغات)در فاکتور تامین کننده آورده شود
///</summary>
public bool IsOtherDeductionsIncludeInSupplierInvoice { get; set; }
/// <summary>
///         /// بیمه شامل تمامی محصولات می شود
///</summary>
public bool AllProductsIncludeInsurance { get; set; }
/// <summary>
///         /// کد دسته بندی معاف از مالیات برای فاکتور
///</summary>
public string TaxExemptCodeForFactor { get; set; }
/// <summary>
///         /// کد دسته بندی مالیات برای فاکتور
///</summary>
public string TaxCodeForFactor { get; set; }
/// <summary>
///         /// واحد سنجش دسته بندی معاف از مالیات برای فاکتور
///</summary>
public string TaxExemptUnitOfMeasurementForFactor { get; set; }
/// <summary>
///         /// واحد سنجش دسته بندی مالیات برای فاکتور
///</summary>
public string TaxUnitOfMeasurementForFactor { get; set; }
/// <summary>
///         /// عنوان دسته بندی معاف از مالیات برای فاکتور
///</summary>
public string TaxExemptTitleForFactor { get; set; }
/// <summary>
///         /// عنوان دسته بندی مالیات برای فاکتور
///</summary>
public string TaxTitleForFactor { get; set; }
/// <summary>
///         /// نیاز به ویرایش فاکتور بعد از تولید
///</summary>
public bool NeedModifyAfterGenerateFactor { get; set; }
/// <summary>
///         /// اجبار به تولید فاکتور بر اساس شناسه باشگاه مشتریان
///</summary>
public bool ForcedToGenerateFactorByCustomerClubId { get; set; }
public bool AllowFilterByProductSalesPlanningIdInGenerateFactor { get; set; }
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
public bool UseProductGroupForConvert { get; set; }
public bool UseProductGroupForFactor { get; set; }
public decimal? MinimumPurchasePrice { get; set; }
/// <summary>
///         /// فرمول محاسبه هزینه ارسال بر اساس تعداد آیتم های هر تنظیم فروش در سبد خرید
///</summary>
public string AdditionalShippingChargeExpression { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual Company Company { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CommonBaseData FactorReportTypeNavigation { get; set; }
    public virtual PersonEconomicProfile Founder { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual PersonBankAccount PersonBankAccount { get; set; }
    public virtual Representation Representation { get; set; }
    public virtual YashilScheduler YashilScheduler { get; set; }
public virtual ICollection<Contract>
    Contract { get; set; }
public virtual ICollection<Factor>
    Factor { get; set; }
public virtual ICollection<OrderItem>
    OrderItem { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanning { get; set; }
public virtual ICollection<SaleConfigurationAttribute>
    SaleConfigurationAttribute { get; set; }
public virtual ICollection<TaxCategorySaleConfiguration>
    TaxCategorySaleConfiguration { get; set; }
    }
 }
