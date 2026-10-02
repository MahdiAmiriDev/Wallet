using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class VirtualClassRoomConfig  :IBaseEntity<int>,IApplicationBasedEntity
    {
public VirtualClassRoomConfig()
{
    CompanyVirtualClassRoomConfig = new HashSet<CompanyVirtualClassRoomConfig>();
    OrganizationVirtualClassRoomConfig = new HashSet<OrganizationVirtualClassRoomConfig>();
    VirtualClass = new HashSet<VirtualClass>();
 }

public int Id { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// نام کاربری
///</summary>
public string UserName { get; set; }
/// <summary>
///         /// کلمه عبور
///</summary>
public string Password { get; set; }
/// <summary>
///         /// آدرس سرویس
///</summary>
public string ServerApiUrl { get; set; }
/// <summary>
///         /// رمز مشترک
///</summary>
public string SharedSecret { get; set; }
/// <summary>
///         /// نوع کلاس مجازی
///</summary>
public int VirtualClassRoomType { get; set; }
/// <summary>
///         /// ظرفیت
///</summary>
public int Capacity { get; set; }
/// <summary>
///         /// شناسه گروه
///</summary>
public int? GroupId { get; set; }
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
public bool? AllowLoginGuest { get; set; }
public bool? OperatorLoginFirst { get; set; }
public string DefaultSkyRoomPassword { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual CommonBaseData VirtualClassRoomTypeNavigation { get; set; }
public virtual ICollection<CompanyVirtualClassRoomConfig>
    CompanyVirtualClassRoomConfig { get; set; }
public virtual ICollection<OrganizationVirtualClassRoomConfig>
    OrganizationVirtualClassRoomConfig { get; set; }
public virtual ICollection<VirtualClass>
    VirtualClass { get; set; }
    }
 }
