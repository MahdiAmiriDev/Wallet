using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class UserRole  :IBaseEntity<int>,IApplicationBasedEntity
    {
public int Id { get; set; }
/// <summary>
///         /// کاربر
///</summary>
public int UserId { get; set; }
/// <summary>
///         /// نقش
///</summary>
public int RoleId { get; set; }
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
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
/// <summary>
///         /// برنامه
///</summary>
public int ApplicationId { get; set; }
public int CreatorOrganizationId { get; set; }
public int? Code { get; set; }
/// <summary>
///         /// آیا نقش را دارد
///</summary>
public bool HasRole { get; set; }
/// <summary>
///         /// آیا میتواند این نقش را به دیگری تخصیص دهد
///</summary>
public bool CanAssign { get; set; }

    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Role Role { get; set; }
    public virtual User User { get; set; }
    }
 }
