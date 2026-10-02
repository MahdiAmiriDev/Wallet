using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class AppEntityAttributeMapping  :IBaseEntity<int>,IApplicationBasedEntity
    {
public AppEntityAttributeMapping()
{
    AppEntityAttributeValue = new HashSet<AppEntityAttributeValue>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// موجودیت
///</summary>
public int AppEntityId { get; set; }
/// <summary>
///         /// ویژگی
///</summary>
public int AppEntityAttributeId { get; set; }
/// <summary>
///         /// اجباری بودن
///</summary>
public bool IsRequired { get; set; }
/// <summary>
///         /// نوع کنترل
///</summary>
public int AttributeControlType { get; set; }
/// <summary>
///         /// ترتیب نمایش
///</summary>
public int? DisplayOrder { get; set; }
/// <summary>
///         /// حداقل مقدار
///</summary>
public int? ValidationMinLength { get; set; }
/// <summary>
///         /// حداکثر مقدار
///</summary>
public int? ValidationMaxLength { get; set; }
/// <summary>
///         /// مقدار پیش فرض
///</summary>
public string DefaultValue { get; set; }
/// <summary>
///         /// مقادیر قابل قبول
///</summary>
public string AllowedValues { get; set; }
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

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual AppEntity AppEntity { get; set; }
    public virtual AppEntityAttribute AppEntityAttribute { get; set; }
    public virtual Application Application { get; set; }
    public virtual CommonBaseData AttributeControlTypeNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<AppEntityAttributeValue>
    AppEntityAttributeValue { get; set; }
    }
 }
