using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class CreditOrCashPayment  :IBaseEntity<int>,IApplicationBasedEntity
    {
public CreditOrCashPayment()
{
    Installment = new HashSet<Installment>();
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
///         /// اطلاعات کارت
///</summary>
public int CardId { get; set; }
/// <summary>
///         /// پذیرنده
///</summary>
public int MerchantId { get; set; }
/// <summary>
///         /// شماره سفارش
///</summary>
public string OrderId { get; set; }
/// <summary>
///         /// تاریخ خرید
///</summary>
public DateTime BuyDate { get; set; }
/// <summary>
///         /// مبلغ کل
///</summary>
public int TotalAmount { get; set; }
/// <summary>
///         /// تعداد اقساط
///</summary>
public int? InstallmentNumber { get; set; }
/// <summary>
///         /// وضعیت
///</summary>
public int Status { get; set; }
/// <summary>
///         /// خرید اعتباری یا ریالی
///</summary>
public bool CreditOrCash { get; set; }
/// <summary>
///         /// تاریخ بازگشت مبلغ
///</summary>
public DateTime? ReturnedDate { get; set; }
/// <summary>
///         /// مبلغ بازگشت داده شده
///</summary>
public int? ReturnedAmount { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual CardInfo Card { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual MerchantInfo Merchant { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual CommonBaseData StatusNavigation { get; set; }
public virtual ICollection<Installment>
    Installment { get; set; }
    }
 }
