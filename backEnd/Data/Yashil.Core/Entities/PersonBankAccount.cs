using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class PersonBankAccount  :IBaseEntity<int>,IApplicationBasedEntity
    {
public PersonBankAccount()
{
    Factor = new HashSet<Factor>();
    RefundDetail = new HashSet<RefundDetail>();
    SaleConfiguration = new HashSet<SaleConfiguration>();
 }

public int Id { get; set; }
/// <summary>
///         /// شخص
///</summary>
public int PersonId { get; set; }
/// <summary>
///         /// بانک
///</summary>
public int BankType { get; set; }
/// <summary>
///         /// شماره کارت
///</summary>
public string CardNumber { get; set; }
/// <summary>
///         /// شماره حساب
///</summary>
public string AccountNumber { get; set; }
/// <summary>
///         /// شماره شبا
///</summary>
public string ShebaNumber { get; set; }
/// <summary>
///         /// نام شعبه
///</summary>
public string BranchName { get; set; }
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
public int? AccessLevelId { get; set; }
/// <summary>
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
/// <summary>
///         /// سازمان ایجاد کننده رکورد
///</summary>
public int CreatorOrganizationId { get; set; }
public int? DepositStatus { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual CommonBaseData BankTypeNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CommonBaseData DepositStatusNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Person Person { get; set; }
public virtual ICollection<Factor>
    Factor { get; set; }
public virtual ICollection<RefundDetail>
    RefundDetail { get; set; }
public virtual ICollection<SaleConfiguration>
    SaleConfiguration { get; set; }
    }
 }
