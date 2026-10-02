using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class NajaDemand  :IBaseEntity<int>
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// مبلغ هر قسط
///</summary>
public long? InstallmentAmount { get; set; }
/// <summary>
///         /// ماه ارسال شده به ناجا
///</summary>
public string Month { get; set; }
/// <summary>
///         /// سال ارسال شده به ناجا
///</summary>
public int? Year { get; set; }
/// <summary>
///         /// شناسه پرداخت
///</summary>
public int? PaymentId { get; set; }
/// <summary>
///         /// تاریخ تایید
///</summary>
public string ApproveDate { get; set; }
/// <summary>
///         /// شماره کارت حکمت
///</summary>
public string CardNumber { get; set; }
/// <summary>
///         /// کد ملی صاحب کارت حکمت
///</summary>
public string NationalCodeCardOwner { get; set; }
/// <summary>
///         /// تعداد اقساط
///</summary>
public int? InstallmentCount { get; set; }
/// <summary>
///         /// نام پرنامه
///</summary>
public string ApplicationTitle { get; set; }
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
