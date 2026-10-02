using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class AdditionalUserProp  :IBaseEntity<int>,IApplicationBasedEntity
    {
public AdditionalUserProp()
{
    AdditionalUserPropCompanyGroup = new HashSet<AdditionalUserPropCompanyGroup>();
 }

public int Id { get; set; }
/// <summary>
///         /// کاربر
///</summary>
public int UserId { get; set; }
/// <summary>
///         /// مرکز
///</summary>
public int? CompanyId { get; set; }
/// <summary>
///         /// نمایندگی
///</summary>
public int? RepresentationId { get; set; }
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
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
/// <summary>
///         /// سازمان ایجاد کننده رکورد
///</summary>
public int CreatorOrganizationId { get; set; }
/// <summary>
///         /// باشگاه مشتریان
///</summary>
public int? CustomerClubId { get; set; }
/// <summary>
///         /// کد باشگاه مشتریان
///</summary>
public string CustomerClubCode { get; set; }
/// <summary>
///         /// دسته بندی نمایندگی
///</summary>
public int? RepresentationCategoryId { get; set; }
/// <summary>
///         /// ????? ?? ????? ??????? ???? ????? ???? ???? ??? ?
///</summary>
public bool ParticipatedInHumaxTest { get; set; }
public bool? PurchasedEmploymentExam { get; set; }
public bool IsResumeFillOutEnough { get; set; }
public string Resume { get; set; }
public string CardNumber { get; set; }
/// <summary>
///         /// گروه کاربری
///</summary>
public int? AdditionalUserPropGroupId { get; set; }
public int? MaximumUploadCapacity { get; set; }

    public virtual ItemGroup AdditionalUserPropGroup { get; set; }
    public virtual Application Application { get; set; }
    public virtual Company Company { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CustomerClub CustomerClub { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Representation Representation { get; set; }
    public virtual RepresentationCategory RepresentationCategory { get; set; }
    public virtual User User { get; set; }
public virtual ICollection<AdditionalUserPropCompanyGroup>
    AdditionalUserPropCompanyGroup { get; set; }
    }
 }
