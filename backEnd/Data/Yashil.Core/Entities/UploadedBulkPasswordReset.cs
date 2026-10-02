using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class UploadedBulkPasswordReset  :IBaseEntity<int>
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// نام کاربری که قرار است کلمه عبور آن بازنشانی شود
///</summary>
public string UserName { get; set; }
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
public string Message { get; set; }
public bool? IsInvalidUserName { get; set; }
public int? AppDocumentId { get; set; }
public bool Deleted { get; set; }
public byte[] Password { get; set; }
public byte[] PasswordSalt { get; set; }

    public virtual AppDocument AppDocument { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    }
 }
