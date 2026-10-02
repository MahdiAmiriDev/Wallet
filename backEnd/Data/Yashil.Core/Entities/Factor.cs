using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Factor  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Factor()
{
    FactorDeduction = new HashSet<FactorDeduction>();
    FactorGroupItem = new HashSet<FactorGroupItem>();
    FactorItem = new HashSet<FactorItem>();
    FactorPayment = new HashSet<FactorPayment>();
    FactorVerification = new HashSet<FactorVerification>();
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
///         /// تنظیم بررسی تایید فاکتور
///</summary>
public int FactorVerificationConfigurationId { get; set; }
/// <summary>
///         /// دسته مالیاتی
///</summary>
public int TaxCategoryId { get; set; }
/// <summary>
///         /// مرکز
///</summary>
public int CompanyId { get; set; }
/// <summary>
///         /// نمایندگی
///</summary>
public int? RepresentationId { get; set; }
/// <summary>
///         /// تاریخ صدور
///</summary>
public int IssueDate { get; set; }
/// <summary>
///         /// وضعیت فاکتور-ثبت اولیه،تایید شده، رد شده و ..
///</summary>
public int FactorReviewStatusType { get; set; }
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
///         /// تنظیم فروش
///</summary>
public int SaleConfigurationId { get; set; }
/// <summary>
///         /// گروه فاکتور
///</summary>
public int? FactorGroupId { get; set; }
/// <summary>
///         /// تاریخ تایید
///</summary>
public DateTime? ConfirmDate { get; set; }
/// <summary>
///         /// تایید کننده
///</summary>
public int? ConfirmBy { get; set; }
public decimal? DiscountPercentage { get; set; }
public decimal? ProfitPercentage { get; set; }
/// <summary>
///         /// اطلاعات حساب شخص
///</summary>
public int? PersonBankAccountId { get; set; }
public string FactorItemPrefix { get; set; }
public bool IsGroupedByTaxCategory { get; set; }
public int FactorReportType { get; set; }
public string FullAddress { get; set; }
public string Telephone { get; set; }
public string FaxNumber { get; set; }
/// <summary>
///         /// کد ملی موسس یا کد اقتصادی--کد تفصیل
///</summary>
public string EconomicCode { get; set; }
/// <summary>
///         /// کد ملی
///</summary>
public string NationalCode { get; set; }
/// <summary>
///         /// آیا شخص حقوقی است
///</summary>
public bool IsLegalPerson { get; set; }
/// <summary>
///         /// کد فاکتور برای کانورت
///</summary>
public string CodeForConvert { get; set; }
/// <summary>
///         /// نام دسته بندی مالیاتی برای گزارش بدون جزئیات فاکتور و (نام کالا) کانورت بر اساس دسته بندی مالیاتی
///</summary>
public string TaxCategoryTitle { get; set; }
/// <summary>
///         /// کد کالا دسته بندی مالیاتی
///</summary>
public string TaxCategoryCode { get; set; }
/// <summary>
///         /// واحد سنجش دسته بندی مالیاتی
///</summary>
public string TaxCategoryUnitOfMeasurement { get; set; }
/// <summary>
///         /// نام مرکز
///</summary>
public string CompanyTitle { get; set; }
/// <summary>
///         /// تعداد اقساط جهت تسویه فاکتور
///</summary>
public int FactorInstallmentCount { get; set; }
public int? FactorConfirmPersianDateInt { get; set; }
public int? ConfirmDateInt { get; set; }
public int? FactorFirstInstallmentPayDateInt { get; set; }
/// <summary>
///         /// شناسه باشگاه مشتریان
///</summary>
public int? CustomerClubId { get; set; }
/// <summary>
///         /// شناسه گروه دسته بندی
///</summary>
public int CategoryGroupId { get; set; }
/// <summary>
///         /// درصد مالیات دسته بندی فاکتور
///</summary>
public decimal? TaxPercentage { get; set; }
/// <summary>
///         /// درصد مالیات پیش فرض دوره
///</summary>
public decimal? DefaultCourseTaxPercentage { get; set; }
/// <summary>
///         /// آیا استفاده از گروه محصولات برای صدور فاکتور انجام شده است؟
///</summary>
public bool? UseProductGroupForFactor { get; set; }
/// <summary>
///         /// آیا استفاده از گروه محصولات برای گزارش کانورت انجام شده است؟
///</summary>
public bool? UseProductGroupForConvert { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual CategoryGroup CategoryGroup { get; set; }
    public virtual Company Company { get; set; }
    public virtual User ConfirmByNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CustomerClub CustomerClub { get; set; }
    public virtual FactorGroup FactorGroup { get; set; }
    public virtual CommonBaseData FactorReportTypeNavigation { get; set; }
    public virtual CommonBaseData FactorReviewStatusTypeNavigation { get; set; }
    public virtual FactorVerificationConfiguration FactorVerificationConfiguration { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual PersonBankAccount PersonBankAccount { get; set; }
    public virtual Representation Representation { get; set; }
    public virtual SaleConfiguration SaleConfiguration { get; set; }
    public virtual TaxCategory TaxCategory { get; set; }
public virtual ICollection<FactorDeduction>
    FactorDeduction { get; set; }
public virtual ICollection<FactorGroupItem>
    FactorGroupItem { get; set; }
public virtual ICollection<FactorItem>
    FactorItem { get; set; }
public virtual ICollection<FactorPayment>
    FactorPayment { get; set; }
public virtual ICollection<FactorVerification>
    FactorVerification { get; set; }
    }
 }
