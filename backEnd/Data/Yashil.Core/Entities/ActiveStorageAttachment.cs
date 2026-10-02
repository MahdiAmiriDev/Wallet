using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ActiveStorageAttachment  :IBaseEntity<int>,IApplicationBasedEntity
    {
/// <summary>
///         /// شناسه پیوست
///</summary>
public int Id { get; set; }
/// <summary>
///         /// نام پیوست
///</summary>
public string Name { get; set; }
/// <summary>
///         /// فرمت ضبط
///</summary>
public string RecordType { get; set; }
/// <summary>
///         /// شناسه ضبط
///</summary>
public int RecordId { get; set; }
/// <summary>
///         /// شناسه وبینار
///</summary>
public int BlobId { get; set; }
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
public int CreatorOrganizationId { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    }
 }
