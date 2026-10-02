using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class LocaleStringResource  :IBaseEntity<int>
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// شناسه زبان
///</summary>
public int LanguageId { get; set; }
/// <summary>
///         /// نام ماژول
///</summary>
public string ModuleName { get; set; }
/// <summary>
///         /// موجودیت
///</summary>
public int? EntityId { get; set; }
/// <summary>
///         /// نام منبع
///</summary>
public string Name { get; set; }
/// <summary>
///         /// مقدار
///</summary>
public string Value { get; set; }
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
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual AppEntity Entity { get; set; }
    public virtual Language Language { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    }
 }
