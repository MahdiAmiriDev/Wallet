using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class HekmatTransaction  :IBaseEntity<int>
    {
public int Id { get; set; }
/// <summary>
///         /// شناسه فایل بارگذاری شده
///</summary>
public int AppDocumentId { get; set; }
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
///         /// کد رهگیری پذیرنده-ردیف گردش ترمینال-hekmatId-کد سمت حکمت
///</summary>
public string InstallmentTrackingCode { get; set; }
/// <summary>
///         /// شماره کارت - CardHashPan
///</summary>
public string CardNumber { get; set; }
/// <summary>
///         /// کد ملی
///</summary>
public string NationalCode { get; set; }
/// <summary>
///         /// تعداد اقساط
///</summary>
public int InstallmentCount { get; set; }
/// <summary>
///         /// مبلغ تراکنش
///</summary>
public long Price { get; set; }
/// <summary>
///         /// وضعیت تراکنش
///</summary>
public int Status { get; set; }
/// <summary>
///         /// کد خطا
///</summary>
public int ErrorCode { get; set; }
/// <summary>
///         /// توکن حکمت - از سمت حکمت ارسال می شود
///</summary>
public string HekmatToken { get; set; }
/// <summary>
///         /// نتیجه تراکنش حکمت
///</summary>
public string TransactionResult { get; set; }
/// <summary>
///         /// تاریخ انجام تراکنش سمت حکمت
///</summary>
public DateTime TransactionDate { get; set; }
/// <summary>
///         /// آیا عملیات به روز رسانی تراکنش های حکمت کامل انجام شده است
///</summary>
public bool Done { get; set; }
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
public byte[] EncryptedNationalCode { get; set; }

    public virtual AppDocument AppDocument { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    }
 }
