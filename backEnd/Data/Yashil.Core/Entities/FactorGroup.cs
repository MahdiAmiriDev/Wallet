using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class FactorGroup  :IBaseEntity<int>,IApplicationBasedEntity
    {
public FactorGroup()
{
    Factor = new HashSet<Factor>();
    FactorGroupItem = new HashSet<FactorGroupItem>();
    FactorItem = new HashSet<FactorItem>();
    InverseParent = new HashSet<FactorGroup>();
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
///         /// گروه پدر
///</summary>
public int? ParentId { get; set; }
/// <summary>
///         /// وضعیت گروه
///</summary>
public int FactorGroupStatus { get; set; }
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
public DateTime? ConfirmDate { get; set; }
public int? FactorGroupConfirmDateInt { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CommonBaseData FactorGroupStatusNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual FactorGroup Parent { get; set; }
public virtual ICollection<Factor>
    Factor { get; set; }
public virtual ICollection<FactorGroupItem>
    FactorGroupItem { get; set; }
public virtual ICollection<FactorItem>
    FactorItem { get; set; }
public virtual ICollection<FactorGroup>
    InverseParent { get; set; }
    }
 }
