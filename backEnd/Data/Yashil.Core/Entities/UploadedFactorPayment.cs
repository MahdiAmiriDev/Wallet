using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class UploadedFactorPayment  :IBaseEntity<int>
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
///         /// فایل آپلود شده
///</summary>
public int AppDocumentId { get; set; }
/// <summary>
///         /// کد فاکتور
///</summary>
public string FactorCode { get; set; }
/// <summary>
///         /// آیا کد فاکتور نامعتبر است
///</summary>
public bool FactorCodeIsInvalid { get; set; }
/// <summary>
///         /// مبلغ پرداخت
///</summary>
public string Price { get; set; }
/// <summary>
///         /// ایا مبلغ واریزی نامعتبر است
///</summary>
public bool PriceIsInvalid { get; set; }
/// <summary>
///         /// تاریخ پرداخت
///</summary>
public string PaymentDate { get; set; }
/// <summary>
///         /// آیا تاریخ پرداخت نامعتبر است
///</summary>
public bool PaymentDateIsInvalid { get; set; }
/// <summary>
///         /// تعداد اقساط
///</summary>
public string InstallmentCount { get; set; }
/// <summary>
///         /// آیا تعداد اقساط نامعتبر است
///</summary>
public bool InstallmentCountIsInvalid { get; set; }
/// <summary>
///         /// شماره حساب
///</summary>
public string BankAccountNumber { get; set; }
/// <summary>
///         /// آیا شماره حساب نامعتبر است
///</summary>
public bool BankAccountNumberIsInvalid { get; set; }
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

    public virtual AppDocument AppDocument { get; set; }
    }
 }
