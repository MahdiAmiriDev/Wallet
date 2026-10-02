using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class CustomerClubPaymentInstallment  :IBaseEntity<int>,IApplicationBasedEntity
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// تراکنش
///</summary>
public int PaymentId { get; set; }
/// <summary>
///         /// مبلغ قسط
///</summary>
public decimal InstallmentAmount { get; set; }
/// <summary>
///         /// تاریخ قسط
///</summary>
public int YearMonth { get; set; }
/// <summary>
///         /// شماره قسط
///</summary>
public int InstallmentNumber { get; set; }
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
///         /// گروه بندی مطالبات ناجا
///</summary>
public int? DemandGroupId { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual ItemGroup DemandGroup { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Payment Payment { get; set; }
    public virtual PersianDateMonth YearMonthNavigation { get; set; }
    }
 }
