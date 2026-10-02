using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class RoleResourceAction  :IBaseEntity<int>,IApplicationBasedEntity
    {
/// <summary>
///         /// کد
///</summary>
public int Id { get; set; }
/// <summary>
///         /// منبع عملیات
///</summary>
public int ResourceActionId { get; set; }
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
public int ApplicationId { get; set; }
public int CreatorOrganizationId { get; set; }
public int? Code { get; set; }

    public virtual ResourceAppAction ResourceAction { get; set; }
    public virtual Role Role { get; set; }
    }
 }
