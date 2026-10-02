using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class FactorPaymentInstallment  :IBaseEntity<int>,IApplicationBasedEntity
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// شناسه گروه فاکتور
///</summary>
public int FactorGroupItemId { get; set; }
/// <summary>
///         /// مبلغ کل پرداختی فاکتور
///</summary>
public decimal SumAmountPaymentToSupplier { get; set; }
/// <summary>
///         /// تعداد اقساط تسویه فاکتور
///</summary>
public int FactorInstallmentCount { get; set; }
/// <summary>
///         /// سال و ماه سر رسید قسط
///</summary>
public int YearMonth { get; set; }
/// <summary>
///         /// شماره قسط پرداختی
///</summary>
public int InstallmentNumber { get; set; }
/// <summary>
///         /// مبلغ یک قسط فاکتور
///</summary>
public decimal InstallmentAmount { get; set; }
/// <summary>
///         /// مبلغ عودت وجه جهت کسر از پرداختی فاکتور
///</summary>
public decimal? RefundAmount { get; set; }
/// <summary>
///         /// مبلغ پرداختی به فاکتور
///</summary>
public decimal? PaidAmount { get; set; }
/// <summary>
///         /// باقیمانده مبلغ پرداختی فاکتور
///</summary>
public decimal? RemainingPaymentToSupplier { get; set; }
/// <summary>
///         /// مجموع پرداختی از فاکتور
///</summary>
public decimal? TotalPaymentToSupplier { get; set; }
/// <summary>
///         /// وضعیت پرداخت قسط
///</summary>
public int? InstallmentPaymentStatusType { get; set; }
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
///         /// شناسه مرجع پرداخت
///</summary>
public string PaymentReferenceId { get; set; }
/// <summary>
///         /// زمان ثبت شناسه مرجع پرداخت
///</summary>
public string PaymentReferencePersianDateTime { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual FactorGroupItem FactorGroupItem { get; set; }
    public virtual CommonBaseData InstallmentPaymentStatusTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual PersianDateMonth YearMonthNavigation { get; set; }
    }
 }
