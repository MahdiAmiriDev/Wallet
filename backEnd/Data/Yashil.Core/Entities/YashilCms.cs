using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class YashilCms  :IBaseEntity<int>,IApplicationBasedEntity
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// تیتر
///</summary>
public string Title { get; set; }
/// <summary>
///         /// خلاصه تیتر(سوتیتر)
///</summary>
public string SubTitle { get; set; }
/// <summary>
///         /// موقعیت در صفحه
///</summary>
public int PagePositionType { get; set; }
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
public string Url { get; set; }
/// <summary>
///         /// ترتیب نمایش
///</summary>
public int DisplayOrder { get; set; }
public bool? CroppableImage { get; set; }
public int? MaxHeight { get; set; }
public int? MaxWidth { get; set; }
public int? MobileMaxHeight { get; set; }
public int? MobileMaxWidth { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual CommonBaseData PagePositionTypeNavigation { get; set; }
    }
 }
