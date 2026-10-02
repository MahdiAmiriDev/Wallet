using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class MerchantInfo  :IBaseEntity<int>,IApplicationBasedEntity
    {
public MerchantInfo()
{
    CreditOrCashPayment = new HashSet<CreditOrCashPayment>();
    HybridPaymentRequest = new HashSet<HybridPaymentRequest>();
    MerchantTerminalInfo = new HashSet<MerchantTerminalInfo>();
    YashilHekmatTerminal = new HashSet<YashilHekmatTerminal>();
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
///         /// کاربر پذیرنده
///</summary>
public int UserId { get; set; }
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
///         /// درگاه حکمت
///</summary>
public bool ForHekmat { get; set; }
/// <summary>
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
/// <summary>
///         /// سازمان ایجاد کننده رکورد
///</summary>
public int CreatorOrganizationId { get; set; }
/// <summary>
///         /// نام پذیرنده
///</summary>
public string MerchantName { get; set; }
/// <summary>
///         /// شهر
///</summary>
public int? CityId { get; set; }
/// <summary>
///         /// شماره تلفن ثابت
///</summary>
public string PhoneNumber { get; set; }
/// <summary>
///         /// شماره تلفن همراه
///</summary>
public string MobileNumber { get; set; }
/// <summary>
///         /// کد پستی
///</summary>
public string PostalCode { get; set; }
/// <summary>
///         /// آدرس
///</summary>
public string Address { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual City City { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual User User { get; set; }
public virtual ICollection<CreditOrCashPayment>
    CreditOrCashPayment { get; set; }
public virtual ICollection<HybridPaymentRequest>
    HybridPaymentRequest { get; set; }
public virtual ICollection<MerchantTerminalInfo>
    MerchantTerminalInfo { get; set; }
public virtual ICollection<YashilHekmatTerminal>
    YashilHekmatTerminal { get; set; }
    }
 }
