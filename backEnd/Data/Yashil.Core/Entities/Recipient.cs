using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Recipient  :IBaseEntity<int>,IApplicationBasedEntity
    {
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
///         /// آدزس
///</summary>
public int? AddressId { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// کد ملی
///</summary>
public string NationalCode { get; set; }
/// <summary>
///         /// شماره کارت
///</summary>
public string CardNumber { get; set; }
/// <summary>
///         /// نوع عضویت
///</summary>
public int MembershipType { get; set; }
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
/// <summary>
///         /// کد ملی برای بررسی یارانه ساتا -در صورتی که این کد مساوی مقدار ستون کد ملی باشد، یعنی وضعیت تایید یارانه درست است و در غیر این صورت باید از اول استعلام گرفته شود
///</summary>
public string NationalCodeForSubsidyApproval { get; set; }
/// <summary>
///         /// وضعیت تایید یارانه
///</summary>
public int? SubsidyStatusType { get; set; }
/// <summary>
///         /// نوع یارانه
///</summary>
public int? SubsidyType { get; set; }
/// <summary>
///         /// شماره موبایل
///</summary>
public string MobileNumber { get; set; }
public string Name { get; set; }
public string LastName { get; set; }
public int? BirthDate { get; set; }
public int? GenderType { get; set; }
public int? RelationshipWithBuyerType { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Address Address { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CommonBaseData GenderTypeNavigation { get; set; }
    public virtual CommonBaseData MembershipTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual CommonBaseData RelationshipWithBuyerTypeNavigation { get; set; }
    public virtual CommonBaseData SubsidyStatusTypeNavigation { get; set; }
    public virtual CommonBaseData SubsidyTypeNavigation { get; set; }
    }
 }
