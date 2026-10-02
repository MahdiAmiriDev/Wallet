using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class HekmatPaymentGatewayModifiedTransaction  :IBaseEntity<int>,IApplicationBasedEntity
    {
public int Id { get; set; }
/// <summary>
///         /// شناسه تراکنش های درگاه پرداخت حکمت
///</summary>
public int HekmatPaymentGatewayTransactionId { get; set; }
/// <summary>
///         /// قیمت قبل از اصلاح
///</summary>
public long OldPrice { get; set; }
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
    public virtual HekmatPaymentGatewayTransaction HekmatPaymentGatewayTransaction { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    }
 }
