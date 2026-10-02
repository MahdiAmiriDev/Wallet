using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Loan  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Loan()
{
    LoanGuarantor = new HashSet<LoanGuarantor>();
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
///         /// شخص
///</summary>
public int PersonId { get; set; }
/// <summary>
///         /// بانک
///</summary>
public int BankType { get; set; }
/// <summary>
///         /// نام شعبه
///</summary>
public string BranchName { get; set; }
/// <summary>
///         /// مبلغ درخواست وام
///</summary>
public long? RequestedAmount { get; set; }
/// <summary>
///         /// مبلغ تایید شده وام
///</summary>
public long ApprovedAmount { get; set; }
/// <summary>
///         /// وضعیت وام
///</summary>
public int LoanStatus { get; set; }
/// <summary>
///         /// تاریخ درخواست وام
///</summary>
public int RequestDate { get; set; }
/// <summary>
///         /// تاریخ تصویب وام
///</summary>
public int AllocationDate { get; set; }
/// <summary>
///         /// علت رد درخواست
///</summary>
public string RejectingReason { get; set; }
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
    public virtual CommonBaseData BankTypeNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CommonBaseData LoanStatusNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Person Person { get; set; }
public virtual ICollection<LoanGuarantor>
    LoanGuarantor { get; set; }
    }
 }
