using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Message  :IBaseEntity<int>,IApplicationBasedEntity
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// شماره موبایل
///</summary>
public string SendTo { get; set; }
/// <summary>
///         /// متن
///</summary>
public string Title { get; set; }
/// <summary>
///         /// مقدار خاص
///</summary>
public string Value { get; set; }
/// <summary>
///         /// نوع پیام
///</summary>
public int? MessageType { get; set; }
/// <summary>
///         /// زمان انقضاء
///</summary>
public DateTime? ExpireAt { get; set; }
/// <summary>
///         /// تعداد دفعات وارد کردن مقدار توسط کاربر
///</summary>
public int RetryCount { get; set; }
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
///         /// منقضی شدن
///</summary>
public bool Expired { get; set; }

    public virtual CommonBaseData MessageTypeNavigation { get; set; }
    }
 }
