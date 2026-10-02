using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class CardInfo  :IBaseEntity<int>,IApplicationBasedEntity
    {
public CardInfo()
{
    CardAmountHistory = new HashSet<CardAmountHistory>();
    CreditOrCashPayment = new HashSet<CreditOrCashPayment>();
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
///         /// شماره کارت
///</summary>
public string CardNumber { get; set; }
/// <summary>
///         /// cvv2
///</summary>
public string Cvv2 { get; set; }
/// <summary>
///         /// رمز اول
///</summary>
public string FirstPassword { get; set; }
/// <summary>
///         /// رمز دوم
///</summary>
public string SecondPassword { get; set; }
/// <summary>
///         /// تاریخ انقضا
///</summary>
public int? ExpireDate { get; set; }
/// <summary>
///         /// مبلغ اعتبار
///</summary>
public int CreditAmount { get; set; }
/// <summary>
///         /// مبلغ بن ریالی
///</summary>
public int CashAmount { get; set; }
/// <summary>
///         /// نوع کارت
///</summary>
public int CardType { get; set; }
/// <summary>
///         /// دارنده
///</summary>
public int PersonId { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual CommonBaseData CardTypeNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Person Person { get; set; }
public virtual ICollection<CardAmountHistory>
    CardAmountHistory { get; set; }
public virtual ICollection<CreditOrCashPayment>
    CreditOrCashPayment { get; set; }
    }
 }
