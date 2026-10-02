using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Conversation  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Conversation()
{
    ConversationRecipient = new HashSet<ConversationRecipient>();
    ConversationSeen = new HashSet<ConversationSeen>();
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
///         /// وضعیت پیام
///</summary>
public int? ConversationStatusType { get; set; }
/// <summary>
///         /// نوع پیام
///</summary>
public int? ConversationType { get; set; }
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
///         /// تاریخ ارسال پیام
///</summary>
public DateTime? SendDate { get; set; }
/// <summary>
///         /// ارسال کننده پیام
///</summary>
public int? Sender { get; set; }
public int ConversationCategoryType { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual CommonBaseData ConversationStatusTypeNavigation { get; set; }
    public virtual CommonBaseData ConversationTypeNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual User SenderNavigation { get; set; }
public virtual ICollection<ConversationRecipient>
    ConversationRecipient { get; set; }
public virtual ICollection<ConversationSeen>
    ConversationSeen { get; set; }
    }
 }
