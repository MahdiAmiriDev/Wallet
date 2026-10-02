using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class UploadedFactorInstallmentPayment  :IBaseEntity<int>
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
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
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
/// <summary>
///         /// موسسه
///</summary>
public string CompanyOrRepresentationTitle { get; set; }
/// <summary>
///         /// شماره فاکتور
///</summary>
public string FactorCode { get; set; }
/// <summary>
///         /// تعداد اقساط پرداختی
///</summary>
public int NumberOfInstallmentsPaid { get; set; }
/// <summary>
///         /// قیمت کل
///</summary>
public decimal SumAmountPaymentToSupplier { get; set; }
/// <summary>
///         /// مبلغ هر قسط
///</summary>
public decimal InstallmentAmount { get; set; }
/// <summary>
///         /// مبلغ برگشتی
///</summary>
public decimal RefundAmount { get; set; }
/// <summary>
///         /// مبلغ پرداخت شده
///</summary>
public decimal PaidAmount { get; set; }
/// <summary>
///         /// مانده پرداختی به تامین‌کننده
///</summary>
public decimal RemainingPaymentToSupplier { get; set; }
/// <summary>
///         /// کل پرداختی به تامین‌کننده
///</summary>
public decimal TotalPaymentToSupplier { get; set; }
/// <summary>
///         /// شماره قسط جاری
///</summary>
public int InstallmentNumber { get; set; }
/// <summary>
///         /// تعداد اقساط باقی‌مانده
///</summary>
public int NumberOfInstallmentsRemaining { get; set; }
/// <summary>
///         /// شماره حساب بانکی
///</summary>
public string AccountNumber { get; set; }
/// <summary>
///         /// شماره شبا
///</summary>
public string ShebaNumber { get; set; }
/// <summary>
///         /// صاحب حساب بانکی
///</summary>
public string BankAccountOwner { get; set; }
/// <summary>
///         /// نام بانک
///</summary>
public string BankName { get; set; }
/// <summary>
///         /// شناسه سند بارگذاری شده.
///</summary>
public int AppDocumentId { get; set; }
/// <summary>
///         /// سال و ماه مربوط به اقساط (فرمت YYYYMM).
///</summary>
public int YearMonth { get; set; }
/// <summary>
///         /// شناسه رکورد پرداخت اقساط فاکتور.
///</summary>
public int FactorPaymentInstallmentId { get; set; }
/// <summary>
///         /// آیا کدآموزشگاه نامعتبر است.
///</summary>
public bool? CodeIsInvalid { get; set; }
/// <summary>
///         /// آیا شماره فاکتور نامعتبر است.
///</summary>
public bool? FactorCodeIsInvalid { get; set; }
/// <summary>
///         /// آیا مجموع مبلغ پرداختی به تأمین‌کننده نامعتبر است.
///</summary>
public bool? SumAmountPaymentToSupplierIsInvalid { get; set; }
/// <summary>
///         /// آیا مبلغ هر قسط نامعتبر است.
///</summary>
public bool? InstallmentAmountIsInvalid { get; set; }
/// <summary>
///         /// آیا شماره قسط نامعتبر است.
///</summary>
public bool? InstallmentNumberIsInvalid { get; set; }
/// <summary>
///         /// آیا مانده قسط نامعتبر است.
///</summary>
public bool? NumberOfInstallmentsRemainingIsInvalid { get; set; }
/// <summary>
///         /// آیا شماره حساب نامعتبر است.
///</summary>
public bool? AccountNumberIsInvalid { get; set; }
/// <summary>
///         /// آیا شماره شبا نامعتبر است.
///</summary>
public bool? ShebaNumberIsInvalid { get; set; }
/// <summary>
///         /// آیا نام صاحب حساب نامعتبر است.
///</summary>
public bool? BankAccountOwnerIsInvalid { get; set; }
/// <summary>
///         /// آیا شناسه پرداخت اقساط فاکتور نامعتبر است.
///</summary>
public bool? FactorPaymentInstallmentIdIsInvalid { get; set; }
public string ErrorMessage { get; set; }
/// <summary>
///         /// انجام شد
///</summary>
public bool? IsDone { get; set; }
/// <summary>
///         /// مبلغ پرداختی نامعتبر است
///</summary>
public bool? PaidAmountIsInvalid { get; set; }
/// <summary>
///         /// مبلغ بازگشتی نامعتبر است
///</summary>
public bool? RefundAmountIsInvalid { get; set; }

    public virtual AppDocument AppDocument { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    }
 }
