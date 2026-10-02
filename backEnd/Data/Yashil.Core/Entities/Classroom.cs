using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ClassRoom  :IBaseEntity<int>,IApplicationBasedEntity
    {
public ClassRoom()
{
    ClassRoomReservation = new HashSet<ClassRoomReservation>();
    ClassSession = new HashSet<ClassSession>();
    InverseParent = new HashSet<ClassRoom>();
    ProductSalesPlanning = new HashSet<ProductSalesPlanning>();
 }

public int Id { get; set; }
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
/// <summary>
///         /// ظرفیت کلاس
///</summary>
public int Capacity { get; set; }
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
public int? Floor { get; set; }
public int? ComputerCount { get; set; }
public bool HasProjector { get; set; }
public bool HasRemoteController { get; set; }
public bool HasSmartBoard { get; set; }
public bool HasWhiteBoard { get; set; }
public int ClassRoomCategoryId { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual ClassRoomCategory ClassRoomCategory { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual ClassRoom Parent { get; set; }
public virtual ICollection<ClassRoomReservation>
    ClassRoomReservation { get; set; }
public virtual ICollection<ClassSession>
    ClassSession { get; set; }
public virtual ICollection<ClassRoom>
    InverseParent { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanning { get; set; }
    }
 }
