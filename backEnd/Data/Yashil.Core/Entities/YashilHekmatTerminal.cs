using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class YashilHekmatTerminal  :IBaseEntity<int>,IApplicationBasedEntity
    {
public YashilHekmatTerminal()
{
    HekmatPaymentGatewayTransaction = new HashSet<HekmatPaymentGatewayTransaction>();
    HekmatPaymentGatewayTransactionBulkRequestCategory = new HashSet<HekmatPaymentGatewayTransactionBulkRequestCategory>();
    HybridPaymentRequest = new HashSet<HybridPaymentRequest>();
    PaymentTransactionItemRefund = new HashSet<PaymentTransactionItemRefund>();
    RefundPaymentTransactionItemRequest = new HashSet<RefundPaymentTransactionItemRequest>();
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
///         /// فعال
///</summary>
public bool IsActive { get; set; }
/// <summary>
///         /// سقف تعداد اقساط
///</summary>
public int MaxInstallmentCount { get; set; }
/// <summary>
///         /// حداقل مبلغ ارسالی به ریال
///</summary>
public long MinAmount { get; set; }
/// <summary>
///         /// سقف مبلغ ارسالی به ریال
///</summary>
public long MaxAmount { get; set; }
/// <summary>
///         /// ترمینال حکمت
///</summary>
public int HekmatTerminalId { get; set; }
/// <summary>
///         /// شناسه پذیرنده
///</summary>
public int MerchantId { get; set; }
/// <summary>
///         /// آدرس سایت
///</summary>
public string Url { get; set; }
/// <summary>
///         /// کلید عمومی
///</summary>
public string PublicKey { get; set; }
/// <summary>
///         /// آی پی
///</summary>
public string IpAddress { get; set; }
/// <summary>
///         /// پورت
///</summary>
public int? Port { get; set; }
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
public decimal SupplierWage { get; set; }
public decimal InstituteWage { get; set; }
public decimal CashSupplierWage { get; set; }
public decimal CashInstituteWage { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual HekmatTerminal HekmatTerminal { get; set; }
    public virtual MerchantInfo Merchant { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransaction>
    HekmatPaymentGatewayTransaction { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransactionBulkRequestCategory>
    HekmatPaymentGatewayTransactionBulkRequestCategory { get; set; }
public virtual ICollection<HybridPaymentRequest>
    HybridPaymentRequest { get; set; }
public virtual ICollection<PaymentTransactionItemRefund>
    PaymentTransactionItemRefund { get; set; }
public virtual ICollection<RefundPaymentTransactionItemRequest>
    RefundPaymentTransactionItemRequest { get; set; }
    }
 }
