using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class HekmatPaymentGatewayTransactionBulkRequestDetail  :IBaseEntity<int>
    {
public HekmatPaymentGatewayTransactionBulkRequestDetail()
{
    InverseParent = new HashSet<HekmatPaymentGatewayTransactionBulkRequestDetail>();
 }

public int Id { get; set; }
/// <summary>
///         /// شناسه سند
///</summary>
public int AppDocumentId { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// شناسه دزخواست انبوه تراکنش های حکمت
///</summary>
public int HekmatPaymentGatewayTransactionBulkRequestId { get; set; }
/// <summary>
///         /// شماره کارت حکمت
///</summary>
public string CardNumber { get; set; }
/// <summary>
///         /// نامعتبر بودن شماره کارت حکمت
///</summary>
public bool? CardNumberIsInValid { get; set; }
/// <summary>
///         /// کد ملی صاحب کارت حکمت
///</summary>
public string NationalCode { get; set; }
/// <summary>
///         /// نامعتبر بودن کد ملی صاحب کارت حکمت
///</summary>
public bool? NationalCodeIsInValid { get; set; }
/// <summary>
///         /// مبلغ تراکنش
///</summary>
public string Price { get; set; }
/// <summary>
///         /// نامعتبر بودن مبلغ تراکنش
///</summary>
public bool? PriceIsInValid { get; set; }
/// <summary>
///         /// تعداد اقساط
///</summary>
public string InstallmentCount { get; set; }
/// <summary>
///         /// نامعتبر بودن تعداد اقساط
///</summary>
public bool? InstallmentCountIsInValid { get; set; }
/// <summary>
///         /// پیام اعتبار سنجی
///</summary>
public string ValidateMessage { get; set; }
public int? StatusCode { get; set; }
public int? ErrorCode { get; set; }
public string HekmatTransactionResultMessage { get; set; }
public string CodePath { get; set; }
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
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
/// <summary>
///         /// تاریخ تایید
///</summary>
public int? PaidDateInt { get; set; }

    public virtual AppDocument AppDocument { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual HekmatPaymentGatewayTransactionBulkRequest HekmatPaymentGatewayTransactionBulkRequest { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual HekmatPaymentGatewayTransactionBulkRequestDetail Parent { get; set; }
public virtual ICollection<HekmatPaymentGatewayTransactionBulkRequestDetail>
    InverseParent { get; set; }
    }
 }
