using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class NajaPayment  
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// نام پرنامه
///</summary>
public string ApplicationTitle { get; set; }
/// <summary>
///         /// شناسه پرداخت
///</summary>
public int? PaymnetId { get; set; }
/// <summary>
///         /// نام محصول (دوره)
///</summary>
public string ProductTitle { get; set; }
/// <summary>
///         /// نام استان
///</summary>
public string ProvinceTitle { get; set; }
/// <summary>
///         /// نام شهر
///</summary>
public string CityTitle { get; set; }
/// <summary>
///         /// کد ملی صاحب کارت حکمت
///</summary>
public string NationalCodeCardOwner { get; set; }
/// <summary>
///         /// شماره کارت حکمت
///</summary>
public string CardNumber { get; set; }
/// <summary>
///         /// مبلغ
///</summary>
public long? Price { get; set; }
/// <summary>
///         /// تعداد اقساط
///</summary>
public int? InstallmentCount { get; set; }
/// <summary>
///         /// کد ملی داوطلب
///</summary>
public string NationalCode { get; set; }
/// <summary>
///         /// تاریخ تایید
///</summary>
public string ApproveDate { get; set; }
/// <summary>
///         /// ماه کسر قسط
///</summary>
public string YearMonth { get; set; }
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

    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    }
 }
