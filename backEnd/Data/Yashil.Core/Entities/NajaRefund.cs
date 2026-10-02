using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class NajaRefund  :IBaseEntity<int>
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// شناسه پرداخت
///</summary>
public int? PaymentId { get; set; }
/// <summary>
///         /// کد ملی داوطلب
///</summary>
public string NationalCode { get; set; }
/// <summary>
///         /// تلفن
///</summary>
public string Tell { get; set; }
/// <summary>
///         /// نام و نام خانوادگی
///</summary>
public string FullName { get; set; }
/// <summary>
///         /// کد ملی صاحب کارت حکمت
///</summary>
public string NationalCodeCardOwner { get; set; }
/// <summary>
///         /// شماره کارت حکمت
///</summary>
public string CardNumber { get; set; }
/// <summary>
///         /// قیمت
///</summary>
public long? Price { get; set; }
/// <summary>
///         /// تاریخ تایید
///</summary>
public string ApproveDate { get; set; }
/// <summary>
///         /// قیمت نهایی
///</summary>
public long? FinalPrice { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// مبلغ هر قسط
///</summary>
public long? InstallmentAmount { get; set; }
/// <summary>
///         /// تعداد اقساط واریز شده به انستیتو
///</summary>
public int? NumberInstallmentDeposited { get; set; }
/// <summary>
///         /// مبلغ واریز شده به انستیتو
///</summary>
public long? AmountDeposited { get; set; }
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
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }

    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    }
 }
