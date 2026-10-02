using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ProductSalesPlanningStudent  :IBaseEntity<int>,IApplicationBasedEntity
    {
public ProductSalesPlanningStudent()
{
    MediaComment = new HashSet<MediaComment>();
    ProductSalesPlanningConversation = new HashSet<ProductSalesPlanningConversation>();
    ProductSalesPlanningReview = new HashSet<ProductSalesPlanningReview>();
    RollCall = new HashSet<RollCall>();
    UserExam = new HashSet<UserExam>();
    UserHomeWork = new HashSet<UserHomeWork>();
    VirtualClassStudent = new HashSet<VirtualClassStudent>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// دوره
///</summary>
public int ProductSalesPlanningId { get; set; }
/// <summary>
///         /// معلم یا دانشجو
///</summary>
public int StudentId { get; set; }
/// <summary>
///         /// نمره
///</summary>
public double? Score { get; set; }
/// <summary>
///         /// وضعیت دانشجو
///</summary>
public int? StudentStatusType { get; set; }
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
    public virtual User Student { get; set; }
    public virtual CommonBaseData StudentStatusTypeNavigation { get; set; }
public virtual ICollection<MediaComment>
    MediaComment { get; set; }
public virtual ICollection<ProductSalesPlanningConversation>
    ProductSalesPlanningConversation { get; set; }
public virtual ICollection<ProductSalesPlanningReview>
    ProductSalesPlanningReview { get; set; }
public virtual ICollection<RollCall>
    RollCall { get; set; }
public virtual ICollection<UserExam>
    UserExam { get; set; }
public virtual ICollection<UserHomeWork>
    UserHomeWork { get; set; }
public virtual ICollection<VirtualClassStudent>
    VirtualClassStudent { get; set; }
    }
 }
