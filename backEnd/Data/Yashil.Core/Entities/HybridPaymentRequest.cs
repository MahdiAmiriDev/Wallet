using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class HybridPaymentRequest  :IBaseEntity<int>,IApplicationBasedEntity
    {
public HybridPaymentRequest()
{
    HybridPayment = new HashSet<HybridPayment>();
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
///         /// قیمت
///</summary>
public long BasePrice { get; set; }
/// <summary>
///         /// شناسه پذیرنده
///</summary>
public int MerchantId { get; set; }
/// <summary>
///         /// شناسه ترمینال یاشیل
///</summary>
public int YashilHekmatTerminalId { get; set; }
/// <summary>
///         /// آدرس بازگشت
///</summary>
public string CallBackUrl { get; set; }
/// <summary>
///         /// داده های تکمیلی
///</summary>
public string AdditionalDate { get; set; }
/// <summary>
///         /// کد رهگیری مشتری به ازای هر پرداخت
///</summary>
public string LocalReservationNumber { get; set; }
/// <summary>
///         /// وضعیت
///</summary>
public int StatusCode { get; set; }
/// <summary>
///         /// شماره کارت حکمت
///</summary>
public string HekmatCardNumber { get; set; }
/// <summary>
///         /// تعداد اقساط
///</summary>
public int? InstallmentCount { get; set; }
/// <summary>
///         /// کارمزد تامین کننده
///</summary>
public decimal SupplierWage { get; set; }
/// <summary>
///         /// کارمزد انستیتو
///</summary>
public decimal InstituteWage { get; set; }
/// <summary>
///         /// کارمزد انستیتو
///</summary>
public decimal CashInstituteWage { get; set; }
/// <summary>
///         /// کارمزد تامین کننده
///</summary>
public decimal CashSupplierWage { get; set; }
/// <summary>
///         /// درصد مالیات
///</summary>
public int Tax { get; set; }
/// <summary>
///         /// درصد پرداخت نقدی
///</summary>
public int? CashPaymentPercentage { get; set; }
/// <summary>
///         /// وضعیت درخواست پرداخت ترکیبی
///</summary>
public int HybridPaymentRequestStatusType { get; set; }
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
public int? ShippingAmount { get; set; }
public bool? UseFormula { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CommonBaseData HybridPaymentRequestStatusTypeNavigation { get; set; }
    public virtual MerchantInfo Merchant { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual YashilHekmatTerminal YashilHekmatTerminal { get; set; }
public virtual ICollection<HybridPayment>
    HybridPayment { get; set; }
    }
 }
