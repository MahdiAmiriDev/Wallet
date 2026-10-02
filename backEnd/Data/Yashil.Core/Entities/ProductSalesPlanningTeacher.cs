using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ProductSalesPlanningTeacher  :IBaseEntity<int>,IApplicationBasedEntity
    {
public ProductSalesPlanningTeacher()
{
    ProductSalesPlanningConversation = new HashSet<ProductSalesPlanningConversation>();
    RollCall = new HashSet<RollCall>();
    VirtualClassTeacher = new HashSet<VirtualClassTeacher>();
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
///         /// نوع معلم
///</summary>
public int? TeacherStatusType { get; set; }
/// <summary>
///         /// آیدی کاربر معلم
///</summary>
public int? TeacherUserId { get; set; }
/// <summary>
///         /// آیدی شخص معلم
///</summary>
public int TeacherPersonId { get; set; }
/// <summary>
///         /// برنامه ریزی دوره
///</summary>
public int? ProductSalesPlanningId { get; set; }
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
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual ProductSalesPlanning ProductSalesPlanning { get; set; }
    public virtual Person TeacherPerson { get; set; }
    public virtual CommonBaseData TeacherStatusTypeNavigation { get; set; }
    public virtual User TeacherUser { get; set; }
public virtual ICollection<ProductSalesPlanningConversation>
    ProductSalesPlanningConversation { get; set; }
public virtual ICollection<RollCall>
    RollCall { get; set; }
public virtual ICollection<VirtualClassTeacher>
    VirtualClassTeacher { get; set; }
    }
 }
