using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Contract  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Contract()
{
    ContractPaymentMethod = new HashSet<ContractPaymentMethod>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// مفاد قرارداد
///</summary>
public string Title { get; set; }
/// <summary>
///         /// تنظیم فروش
///</summary>
public int SaleConfigurationId { get; set; }
/// <summary>
///         /// شماره قرارداد
///</summary>
public string ContractNumber { get; set; }
/// <summary>
///         /// تاریخ تنظیم
///</summary>
public int ArrangementDate { get; set; }
/// <summary>
///         /// نوع قرارداد
///</summary>
public int ContractType { get; set; }
/// <summary>
///         /// وضعیت قرارداد
///</summary>
public int ContractStatus { get; set; }
/// <summary>
///         /// تایید شده
///</summary>
public bool Confirmed { get; set; }
/// <summary>
///         /// برنامه عملکرد مالی
///</summary>
public int? TurnoverExpect { get; set; }
/// <summary>
///         /// شماره مجوز
///</summary>
public string LicenseNumber { get; set; }
/// <summary>
///         /// کد اقتصادی
///</summary>
public string EconomicCode { get; set; }
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
public int? TbaoId { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual CommonBaseData ContractStatusNavigation { get; set; }
    public virtual CommonBaseData ContractTypeNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual SaleConfiguration SaleConfiguration { get; set; }
public virtual ICollection<ContractPaymentMethod>
    ContractPaymentMethod { get; set; }
    }
 }
