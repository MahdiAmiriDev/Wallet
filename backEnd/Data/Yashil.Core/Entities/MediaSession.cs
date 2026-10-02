using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class MediaSession  :IBaseEntity<int>,IApplicationBasedEntity
    {
public MediaSession()
{
    MediaComment = new HashSet<MediaComment>();
    MediaKeyMoment = new HashSet<MediaKeyMoment>();
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
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// زمان
///</summary>
public int? Duration { get; set; }
/// <summary>
///         /// آیا عمومی است؟
///</summary>
public bool IsPublic { get; set; }
/// <summary>
///         /// انتشاریافته؟
///</summary>
public bool Publish { get; set; }
/// <summary>
///         /// رسانه
///</summary>
public int MediaCategoryId { get; set; }
/// <summary>
///         /// تعداد نمایش
///</summary>
public int ViewCont { get; set; }
/// <summary>
///         /// تعداد دانلود
///</summary>
public int DownloadCount { get; set; }
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
///         /// نوع محتوا ( ویدئو یا اسکورم دو دیگر محتوا)
///</summary>
public int MediaSessionContentType { get; set; }
/// <summary>
///         /// Indicates whether viewing the content is mandatory (1 = Yes, 0 = No)
///</summary>
public bool? IsRequired { get; set; }
public int? CommentRestrictionCount { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual MediaCategory MediaCategory { get; set; }
    public virtual CommonBaseData MediaSessionContentTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<MediaComment>
    MediaComment { get; set; }
public virtual ICollection<MediaKeyMoment>
    MediaKeyMoment { get; set; }
    }
 }
