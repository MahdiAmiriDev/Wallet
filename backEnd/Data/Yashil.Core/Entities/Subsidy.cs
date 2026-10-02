using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Subsidy  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Subsidy()
{
    Order = new HashSet<Order>();
    OrderItem = new HashSet<OrderItem>();
 }

public int Id { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// سقف یارانه وزارت
///</summary>
public long VezaratCeiling { get; set; }
/// <summary>
///         /// سقف یارانه ساتا
///</summary>
public long SataCeiling { get; set; }
/// <summary>
///         /// سقف مبلغ یارانه ساتا به ازای هر شماره حکمت
///</summary>
public int SataCeilingPerHekmatCard { get; set; }
/// <summary>
///         /// سقف مبلغ یارانه وزارت به ازای هر شماره حکمت
///</summary>
public int VezaratCeilingPerHekmatCard { get; set; }
/// <summary>
///         /// سقف تعداد یارانه ساتا به ازای هر شماره حکمت
///</summary>
public int SataSubsidyMaxCountPerHekmatCard { get; set; }
/// <summary>
///         /// سقف تعداد یارانه وزارت به ازای هر شماره حکمت
///</summary>
public int VezaratSubsidyMaxCountPerHekmatCard { get; set; }
/// <summary>
///         /// درصد سود یارانه وزارت
///</summary>
public decimal VezaratProfitPercentage { get; set; }
/// <summary>
///         /// درصد سود یارانه ساتا
///</summary>
public decimal SataProfitPercentage { get; set; }
/// <summary>
///         /// سال
///</summary>
public int Year { get; set; }
/// <summary>
///         /// یارانه استفاده شده در وزارت
///</summary>
public int UsedInVezarat { get; set; }
/// <summary>
///         /// یارانه استفاده شده در ساتا
///</summary>
public int UsedInSata { get; set; }
/// <summary>
///         /// فعال
///</summary>
public bool IsActive { get; set; }
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
public bool IsVezaratActive { get; set; }
public bool IsSataActive { get; set; }
/// <summary>
///         /// شناسه گروه دسته بندی
///</summary>
public int CategoryGroupId { get; set; }
/// <summary>
///         /// شناسه گروه یارانه
///</summary>
public int SubsidyGroupId { get; set; }
/// <summary>
///         /// آیا ضرایب یارانه اعمال شود ؟
///</summary>
public bool ApplySubsidyFactor { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual CategoryGroup CategoryGroup { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual SubsidyGroup SubsidyGroup { get; set; }
public virtual ICollection<Order>
    Order { get; set; }
public virtual ICollection<OrderItem>
    OrderItem { get; set; }
    }
 }
