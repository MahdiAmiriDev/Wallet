using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class SaleConfigurationAttribute  :IBaseEntity<int>,IApplicationBasedEntity
    {
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
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// نوع تخفیف
///</summary>
public int DiscountType { get; set; }
/// <summary>
///         /// استفاده از درصد برای تخفیف
///</summary>
public bool UsePercentage { get; set; }
/// <summary>
///         /// درصد تخفیف
///</summary>
public decimal DiscountPercentage { get; set; }
/// <summary>
///         /// مقدار تخفیف
///</summary>
public decimal? DiscountAmount { get; set; }
/// <summary>
///         /// حداکثر مقدار تخفیف برای حالت درصد تخفیف
///</summary>
public decimal? MaximumDiscountAmount { get; set; }
/// <summary>
///         /// درصد سود از تامین کننده
///</summary>
public decimal ProfitPercentage { get; set; }
/// <summary>
///         /// سایر کسورات
///</summary>
public decimal? OtherDeductionsPercentage { get; set; }
/// <summary>
///         /// تاریخ شروع تخفیف
///</summary>
public DateTime? StartDate { get; set; }
/// <summary>
///         /// تاریخ پایان تخفیف
///</summary>
public DateTime? EndDate { get; set; }
/// <summary>
///         /// نیاز به کد تخفیف
///</summary>
public bool RequiresCouponCode { get; set; }
/// <summary>
///         /// مقدار کد تخفیف
///</summary>
public string CouponCode { get; set; }
/// <summary>
///         /// امکان تجمیع با دیگر تخفیف ها وجود دارد ؟
///</summary>
public bool IsCumulative { get; set; }
/// <summary>
///         /// نوع محدودیت تخفیف
///</summary>
public int DiscountLimitationType { get; set; }
/// <summary>
///         /// تعداد تخفیف
///</summary>
public int? LimitationTimes { get; set; }
/// <summary>
///         /// حداکثر تعداد محصولاتی که می توان به آن تخفیف داد اگر خالی باشد به همه اعمال می شود
///</summary>
public int? MaximumDiscountedQuantity { get; set; }
/// <summary>
///         /// فرزندان دسته بندی نیز از این تخفیف بتوانند استفاده کنند
///</summary>
public bool AppliedToSubCategories { get; set; }
/// <summary>
///         /// توضیحات مدیر
///</summary>
public string AdminComment { get; set; }
/// <summary>
///         /// در شگفت انگیز 1باشد
///</summary>
public bool? IsAmazingOffer { get; set; }
/// <summary>
///         /// در شگفت انگیز 2باشد
///</summary>
public bool? IsAmazingOffer2 { get; set; }
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
///         /// شناسه تنظیم فروش
///</summary>
public int SaleConfigurationId { get; set; }
public decimal WagePercentage { get; set; }
public bool DisplayDiscountInProductDetial { get; set; }
public bool DisplaySubsidyInProductDetial { get; set; }
public bool DisplayDiscountInOrderSummaryForPay { get; set; }
public bool DisplayTaxInOrderSummaryForPay { get; set; }
public bool DisplayWageInOrderSummaryForPay { get; set; }
public bool DisplaySubsidyInOrderSummaryForPay { get; set; }
/// <summary>
///         /// شناسه باشگاه مشتریان
///</summary>
public int CustomerClubId { get; set; }
/// <summary>
///         /// نوع فروش : نقدی - اقساط یا هر دو مبتنی بر قرارداد با فروشنده
///</summary>
public int SaleType { get; set; }
public bool IsActive { get; set; }
/// <summary>
///         /// فرمول محاسبه قسط
///</summary>
public string InstallmentExpression { get; set; }
/// <summary>
///         /// تعداد اقساط
///</summary>
public int InstallmentCount { get; set; }
/// <summary>
///         /// محاسبه اقساط بر اساس فرمول
///</summary>
public bool ExpressionBasedInstallmentCount { get; set; }
/// <summary>
///         /// اجازه تعیین اقساط در تعریف محصول
///</summary>
public bool AllowSetInstallmentInProduct { get; set; }
/// <summary>
///         /// تعداد اقساط جهت تسویه فاکتور
///</summary>
public int? FactorInstallmentCount { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CustomerClub CustomerClub { get; set; }
    public virtual CommonBaseData DiscountLimitationTypeNavigation { get; set; }
    public virtual CommonBaseData DiscountTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual SaleConfiguration SaleConfiguration { get; set; }
    public virtual CommonBaseData SaleTypeNavigation { get; set; }
    }
 }
