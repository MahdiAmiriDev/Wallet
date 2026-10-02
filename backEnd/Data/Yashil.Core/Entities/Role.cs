using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Role  :IBaseEntity<int>
    {
public Role()
{
    ConversationRecipient = new HashSet<ConversationRecipient>();
    FactorVerificationConfigurationRole = new HashSet<FactorVerificationConfigurationRole>();
    ResourceAttributeRole = new HashSet<ResourceAttributeRole>();
    RoleDashboard = new HashSet<RoleDashboard>();
    RoleReport = new HashSet<RoleReport>();
    RoleResourceAction = new HashSet<RoleResourceAction>();
    UserRole = new HashSet<UserRole>();
 }

/// <summary>
///         /// کد
///</summary>
public int Id { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// فعال بودن
///</summary>
public bool IsActive { get; set; }
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
public int? ApplicationId { get; set; }
/// <summary>
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
public int? Code { get; set; }
/// <summary>
///         /// آیا این نقش می تواند پیام دریافت کند
///</summary>
public bool AllowReceiveMessage { get; set; }

    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<ConversationRecipient>
    ConversationRecipient { get; set; }
public virtual ICollection<FactorVerificationConfigurationRole>
    FactorVerificationConfigurationRole { get; set; }
public virtual ICollection<ResourceAttributeRole>
    ResourceAttributeRole { get; set; }
public virtual ICollection<RoleDashboard>
    RoleDashboard { get; set; }
public virtual ICollection<RoleReport>
    RoleReport { get; set; }
public virtual ICollection<RoleResourceAction>
    RoleResourceAction { get; set; }
public virtual ICollection<UserRole>
    UserRole { get; set; }
    }
 }
