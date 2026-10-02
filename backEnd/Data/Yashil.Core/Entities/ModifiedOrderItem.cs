using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ModifiedOrderItem  :IBaseEntity<int>,IApplicationBasedEntity
    {
public ModifiedOrderItem()
{
    RefundDetail = new HashSet<RefundDetail>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// آیتم سفارش قدیمی
///</summary>
public int OldOrderItemId { get; set; }
/// <summary>
///         /// نوع اصلاح سفارش تغییر یا لغو
///</summary>
public int? ModifiedOrderItemType { get; set; }
/// <summary>
///         /// وضعیت اصلاح سفارش
///</summary>
public int ModifiedOrderItemStatusType { get; set; }
/// <summary>
///         /// انجام دهنده کار-کارشناس crm
///</summary>
public int? Assignee { get; set; }
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
///         /// آیتم سفارش جدی
///</summary>
public int? NewOrderItemId { get; set; }
public string RejectRreason { get; set; }
public string ExpertDescription { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User AssigneeNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CommonBaseData ModifiedOrderItemStatusTypeNavigation { get; set; }
    public virtual CommonBaseData ModifiedOrderItemTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual OrderItem NewOrderItem { get; set; }
    public virtual OrderItem OldOrderItem { get; set; }
public virtual ICollection<RefundDetail>
    RefundDetail { get; set; }
    }
 }
