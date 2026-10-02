using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Issue  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Issue()
{
    IssueComment = new HashSet<IssueComment>();
    IssueLogWork = new HashSet<IssueLogWork>();
    IssueParticipant = new HashSet<IssueParticipant>();
    IssueTypeItem = new HashSet<IssueTypeItem>();
 }

public int Id { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// آیتم سفارش
///</summary>
public int? OrderItemId { get; set; }
/// <summary>
///         /// (کاربر عضو)مطرح کننده مشکل
///</summary>
public int? IssueOwner { get; set; }
/// <summary>
///         /// کاربر غیر عضو
///</summary>
public string NonRegisteredUser { get; set; }
/// <summary>
///         /// نوع مسئله(پدر)
///</summary>
public int IssueType { get; set; }
/// <summary>
///         /// شرح مسئله
///</summary>
public string Description { get; set; }
/// <summary>
///         /// راه حل
///</summary>
public string Solution { get; set; }
/// <summary>
///         /// نتیجه مسئله
///</summary>
public int? ResolutionType { get; set; }
/// <summary>
///         /// اولویت
///</summary>
public int PriorityType { get; set; }
/// <summary>
///         /// تخمین زدن
///</summary>
public int? Estimate { get; set; }
/// <summary>
///         /// تاریخ سررسید
///</summary>
public int? DueDate { get; set; }
/// <summary>
///         /// تاریخ حل مسئله
///</summary>
public int? ResolvedDate { get; set; }
/// <summary>
///         /// وضعیت مسئله
///</summary>
public int? IssueStateType { get; set; }
/// <summary>
///         /// واگذارنده
///</summary>
public int Assignee { get; set; }
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
///         /// سفارش
///</summary>
public int? OrderId { get; set; }
/// <summary>
///         /// پرداخت
///</summary>
public int? PaymentId { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User AssigneeNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User IssueOwnerNavigation { get; set; }
    public virtual CommonBaseData IssueStateTypeNavigation { get; set; }
    public virtual CommonBaseData IssueTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Order Order { get; set; }
    public virtual OrderItem OrderItem { get; set; }
    public virtual Payment Payment { get; set; }
    public virtual CommonBaseData PriorityTypeNavigation { get; set; }
    public virtual CommonBaseData ResolutionTypeNavigation { get; set; }
public virtual ICollection<IssueComment>
    IssueComment { get; set; }
public virtual ICollection<IssueLogWork>
    IssueLogWork { get; set; }
public virtual ICollection<IssueParticipant>
    IssueParticipant { get; set; }
public virtual ICollection<IssueTypeItem>
    IssueTypeItem { get; set; }
    }
 }
