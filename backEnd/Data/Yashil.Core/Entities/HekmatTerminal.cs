using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class HekmatTerminal  :IBaseEntity<int>,IApplicationBasedEntity
    {
public HekmatTerminal()
{
    HekmatPaymentGatewayTransaction = new HashSet<HekmatPaymentGatewayTransaction>();
    YashilHekmatTerminal = new HashSet<YashilHekmatTerminal>();
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
///         /// شماره ترمینال پردازش تراکنش های حکمت مربوط به ای پی آی حکمت
        /// 
///</summary>
public string TerminalNumber { get; set; }
/// <summary>
///         /// فعال
///</summary>
public bool? IsActive { get; set; }
public int? MaxInstallmentCount { get; set; }
/// <summary>
///         /// آیا شماره ترمینال آموزشی می باشد ؟
///</summary>
public bool IsLearn { get; set; }
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

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransaction>
    HekmatPaymentGatewayTransaction { get; set; }
public virtual ICollection<YashilHekmatTerminal>
    YashilHekmatTerminal { get; set; }
    }
 }
