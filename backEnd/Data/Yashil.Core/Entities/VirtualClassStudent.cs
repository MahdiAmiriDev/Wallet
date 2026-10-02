using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class VirtualClassStudent  :IBaseEntity<int>,IApplicationBasedEntity
    {
public VirtualClassStudent()
{
    InverseParent = new HashSet<VirtualClassStudent>();
 }

public int Id { get; set; }
/// <summary>
///         /// شناسه دانشجو
///</summary>
public int? ProductSalesPlanningStudentId { get; set; }
/// <summary>
///         /// شناسه کلاس مجازی
///</summary>
public int? VirtualClassId { get; set; }
/// <summary>
///         /// شناسه کلاس مجازی
///</summary>
public string VirtualClassUrl { get; set; }
public int? ParentId { get; set; }
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
public string CodePath { get; set; }
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
public bool IsMessageSent { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual VirtualClassStudent Parent { get; set; }
    public virtual ProductSalesPlanningStudent ProductSalesPlanningStudent { get; set; }
    public virtual VirtualClass VirtualClass { get; set; }
public virtual ICollection<VirtualClassStudent>
    InverseParent { get; set; }
    }
 }
