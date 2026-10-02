using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class NewsStore  :IBaseEntity<int>,IApplicationBasedEntity
    {
public NewsStore()
{
    MainNews = new HashSet<MainNews>();
    NewsKeyword = new HashSet<NewsKeyword>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// سرویس خبری
///</summary>
public int ServiceId { get; set; }
/// <summary>
///         /// نوع خبر
///</summary>
public int NewsType { get; set; }
/// <summary>
///         /// زیرتیتر
///</summary>
public string Deck { get; set; }
/// <summary>
///         /// روتیتر
///</summary>
public string Kicker { get; set; }
/// <summary>
///         /// تیتر
///</summary>
public string Title { get; set; }
/// <summary>
///         /// خلاصه تیتر(سوتیتر)
///</summary>
public string SubTitle { get; set; }
/// <summary>
///         /// لید
///</summary>
public string Led { get; set; }
/// <summary>
///         /// متن خبر
///</summary>
public string Body { get; set; }
/// <summary>
///         /// زمان انتشار خبر
///</summary>
public int? EffluenceTime { get; set; }
/// <summary>
///         /// تاریخ انتشار خبر
///</summary>
public int? EffluenceDateInt { get; set; }
/// <summary>
///         /// تعداد بازدید از این خبر
///</summary>
public int? ViewCount { get; set; }
/// <summary>
///         /// تاریخ انقضای خبر
///</summary>
public int? ExpireDateInt { get; set; }
/// <summary>
///         /// زمان انقضای خبر
///</summary>
public int? ExpireTime { get; set; }
/// <summary>
///         /// زبان خبر
///</summary>
public int Language { get; set; }
/// <summary>
///         /// اخبار سایر سایت ها
///</summary>
public bool IsOtherSiteNews { get; set; }
/// <summary>
///         /// کد هش مربوط به مسیر صفحه در یک سایت دیگر
///</summary>
public string NewsWebSiteCode { get; set; }
/// <summary>
///         /// تاریخ انقضای خبر به میلادی
///</summary>
public DateTime? ExpireDate { get; set; }
/// <summary>
///         /// تاریخ انتشار خبر به میلادی
///</summary>
public DateTime? EffluenceDate { get; set; }
/// <summary>
///         /// تایید شده
///</summary>
public bool Confirmed { get; set; }
/// <summary>
///         /// کلمات کلیدی
///</summary>
public string KeywordStr { get; set; }
public long? OtherSiteNewsId { get; set; }
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
///         /// شناسه مرکز
///</summary>
public int? CompanyId { get; set; }
public string Link { get; set; }
public bool? IsPinned { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual Company Company { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CommonBaseData LanguageNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual CommonBaseData NewsTypeNavigation { get; set; }
    public virtual Service Service { get; set; }
public virtual ICollection<MainNews>
    MainNews { get; set; }
public virtual ICollection<NewsKeyword>
    NewsKeyword { get; set; }
    }
 }
