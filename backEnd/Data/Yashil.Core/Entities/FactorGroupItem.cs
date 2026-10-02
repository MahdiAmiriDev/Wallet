using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class FactorGroupItem  :IBaseEntity<int>,IApplicationBasedEntity
    {
public FactorGroupItem()
{
    FactorPaymentInstallment = new HashSet<FactorPaymentInstallment>();
    InverseParent = new HashSet<FactorGroupItem>();
 }

public int Id { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// شناسه گروه فاکتور
///</summary>
public int FactorGroupId { get; set; }
/// <summary>
///         /// شناسه فاکتور
///</summary>
public int FactorId { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// کد نمایندگی یا مرکز
///</summary>
public string CompanyOrRepresentationCode { get; set; }
/// <summary>
///         /// نام مرکز
///</summary>
public string CompanyTitle { get; set; }
/// <summary>
///         /// نام نمایندگی
///</summary>
public string RepresentationTitle { get; set; }
/// <summary>
///         /// کد فاکتور
///</summary>
public string FactorCode { get; set; }
/// <summary>
///         /// شماره حساب
///</summary>
public string AccountNumber { get; set; }
/// <summary>
///         /// شماره شبا
///</summary>
public string ShebaNumber { get; set; }
/// <summary>
///         /// نام  صاحب حساب
///</summary>
public string PersonBankAccountName { get; set; }
/// <summary>
///         /// نام خانوادگی صاحب حساب
///</summary>
public string PersonBankAccountLastName { get; set; }
/// <summary>
///         /// کد ملی صاحب حساب
///</summary>
public string PersonBankAccountNationalCode { get; set; }
/// <summary>
///         /// نام بانک
///</summary>
public string BankTypeTitle { get; set; }
/// <summary>
///         /// کد تفصیلی  اگر شخصیت حقوقی باشد باید کد اقتصادی باشد و اگر حقیقی باشد کد ملی موسس قرار میگیرد
///</summary>
public string DetailedCode { get; set; }
/// <summary>
///         /// نوع پرداخت
///</summary>
public string PaymentType { get; set; }
/// <summary>
///         /// دسته بندی مالیاتی
///</summary>
public string TaxCategoryTitle { get; set; }
/// <summary>
///         /// قیمت پایه آیتم سفارش : در این قیمت تعداد لحاظ شده است
///</summary>
public decimal? SumTotal { get; set; }
/// <summary>
///         /// قیمت پایه منهای تخفیف بعلاوه هزینه ارسال
///</summary>
public decimal? SumPriceIncDiscountWithAdditionalShippingCharge { get; set; }
/// <summary>
///         /// یارانه ساتا
///</summary>
public decimal? SumSataSubsidyPrice { get; set; }
/// <summary>
///         /// یارانه ساتا با سود
///</summary>
public decimal? SumSataSataSubsidyPriceIncProfit { get; set; }
/// <summary>
///         /// سهم ساتا : یارانه ساتا با سود و مالیات
///</summary>
public decimal? SataPortion { get; set; }
/// <summary>
///         /// یارانه وزارت
///</summary>
public decimal? SumVezaratSubsidy { get; set; }
/// <summary>
///         /// یارانه وزارت با سود
///</summary>
public decimal? SumVezaratSubsidyPriceIncProfit { get; set; }
/// <summary>
///         /// سهم وزارت :  یارانه وزارت با سود و مالیات
///</summary>
public decimal? SumVezarat { get; set; }
/// <summary>
///         /// قیمت پایه منهای تخفیف بعلاوه هزینه ارسال منهای یارانه وزارت منهای یارانه ساتا
///</summary>
public decimal? SumPriceIncDiscountWithAdditionalShippingChargeExclVezaratSubsidyExclSataSubsidy { get; set; }
/// <summary>
///         /// قیمت پایه منهای تخفیف بعلاوه هزینه ارسال منهای یارانه وزارت منهای یارانه ساتا بعلاوه کارمزد
///</summary>
public decimal? SumPriceIncDiscountWithAdditionalShippingChargeExclVezaratSubsidyExclSataSubsidyWithWageAmount { get; set; }
/// <summary>
///         /// سهم خانوار:مبلغ حکمت و آنلاین و ناجا با کارمزد و مالیات
///</summary>
public decimal? FamilyPortion { get; set; }
/// <summary>
///         /// مبلغ کل منهای تخفیف منهای سود + هزینه ارسال
///</summary>
public decimal? SumNetPrice { get; set; }
/// <summary>
///         /// مبلغ که باید به تامین کننده پرداخت شود
///</summary>
public decimal? SumAmountPaidToSupplier { get; set; }
/// <summary>
///         /// جمع هزینه ارسال
///</summary>
public decimal? SumAdditionalShippingCharge { get; set; }
/// <summary>
///         /// مجموع مبلغ کارمزد سایت
///</summary>
public decimal? SumWageAmountExcTax { get; set; }
/// <summary>
///         /// جمع مالیات خرید
///</summary>
public decimal? SumTaxAmount { get; set; }
/// <summary>
///         /// جمع مبلغ بیمه
///</summary>
public decimal? SumInsuranceAmount { get; set; }
/// <summary>
///         /// مجموع تخفیف
///</summary>
public decimal? SumDiscountAmountExclTax { get; set; }
/// <summary>
///         /// مجموع سود
///</summary>
public decimal? SumProfitPrice { get; set; }
/// <summary>
///         /// مجموع سود + تخفیف
///</summary>
public decimal? SumDiscountWithProfitPrice { get; set; }
/// <summary>
///         /// مجموع سود + تخفیف + هزینه تبلیغات
///</summary>
public decimal? SumDiscountWithProfitPriceWithOtherDeductionsPrice { get; set; }
/// <summary>
///         /// قیمت منهای سود منهای تخفیف
///</summary>
public decimal? SumPriceExclDiscountExclProfit { get; set; }
/// <summary>
///         /// قیمت منهای سود منهای تخفیف منهای هزینه تبلیغات
///</summary>
public decimal? SumPriceExclDiscountExclProfitExclOtherDeductionsPrice { get; set; }
/// <summary>
///         /// مجموع سایر کسورات: مجموع هزینه تبلیغات
///</summary>
public decimal? SumOtherDeductionsPrice { get; set; }
/// <summary>
///         /// مجموع سایر کسورات با مالیات: مجموع هزینه تبلیغات با مالیات
///</summary>
public decimal? SumOtherDeductionsPriceIncTax { get; set; }
/// <summary>
///         /// مالیات + قیمت خرید+هزینه ارسال:شرکت این مبلغ را به تامین کننده می دهد:قیمت خرید انستیتو
///</summary>
public decimal? SumDiscountedPriceSumTaxAmount { get; set; }
/// <summary>
///         /// درصد تخفیف
///</summary>
public decimal? DiscountPercentage { get; set; }
/// <summary>
///         /// درصد سود
///</summary>
public decimal? ProfitPercentage { get; set; }
/// <summary>
///         /// درصد فاکتور: در صد تخفیف + سود
///</summary>
public decimal? ProfitDiscountPercentage { get; set; }
/// <summary>
///         /// تعداد آیتم های فاکتور
///</summary>
public int? FactorItemCount { get; set; }
/// <summary>
///         /// تاریخ تایید فاکتور
///</summary>
public int? FactorConfirmDateInt { get; set; }
/// <summary>
///         /// تاریخ اولین قسط
///</summary>
public int? FactorFirstInstallmentPayDateInt { get; set; }
/// <summary>
///         /// تعداد اقساط تسویه فاکتور
///</summary>
public int? FactorInstallmentCount { get; set; }
/// <summary>
///         /// تاریخ اولین خرید
///</summary>
public int? FirstFactorItemDateInt { get; set; }
/// <summary>
///         /// تاریخ آخرین خرید
///</summary>
public int? LastFactorItemDateInt { get; set; }
/// <summary>
///         /// تاریخ گروه بندی
///</summary>
public int? FactorGroupConfirmDateInt { get; set; }
/// <summary>
///         /// آیا هزینه تبلیغات در فاکتور تامین کننده لحاظ شود
///</summary>
public int? IsOtherDeductionsIncludeInSupplierInvoice { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
public string CodePath { get; set; }
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
///         /// سال و ماه اولین قسط پرداخت فاکتور
///</summary>
public int? FactorFirstInstallmentPayYearMonth { get; set; }
public int? NumberOfInstallmentsPaid { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual Factor Factor { get; set; }
    public virtual PersianDateMonth FactorFirstInstallmentPayYearMonthNavigation { get; set; }
    public virtual FactorGroup FactorGroup { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual FactorGroupItem Parent { get; set; }
public virtual ICollection<FactorPaymentInstallment>
    FactorPaymentInstallment { get; set; }
public virtual ICollection<FactorGroupItem>
    InverseParent { get; set; }
    }
 }
