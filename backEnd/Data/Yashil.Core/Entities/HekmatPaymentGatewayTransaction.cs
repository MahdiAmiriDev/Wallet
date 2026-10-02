using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class HekmatPaymentGatewayTransaction  :IBaseEntity<int>,IApplicationBasedEntity
    {
public HekmatPaymentGatewayTransaction()
{
    HekmatPaymentGatewayModifiedTransaction = new HashSet<HekmatPaymentGatewayModifiedTransaction>();
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
public long Price { get; set; }
/// <summary>
///         /// قیمت
///</summary>
public long? BasePrice { get; set; }
/// <summary>
///         /// قیمت
///</summary>
public long? PriceToPay { get; set; }
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
///         /// کد خطا
///</summary>
public int ErrorCode { get; set; }
/// <summary>
///         /// شماره کارت حکمت
///</summary>
public string HekmatCardNumber { get; set; }
/// <summary>
///         /// کد ملی
///</summary>
public string NationalCode { get; set; }
/// <summary>
///         /// شماره همراه
///</summary>
public string Mobile { get; set; }
/// <summary>
///         /// شماره تلفن
///</summary>
public string Tell { get; set; }
/// <summary>
///         /// تعداد اقساط
///</summary>
public int InstallmentCount { get; set; }
/// <summary>
///         /// توکن حکمت
///</summary>
public string HekmatToken { get; set; }
/// <summary>
///         /// ترمینال حکمت
///</summary>
public int HekmatTerminalId { get; set; }
/// <summary>
///         /// نتیجه تراکنش حکمت
///</summary>
public string HekmatTransactionResultMessage { get; set; }
/// <summary>
///         /// فاکتور شده؟
///</summary>
public bool HasBeenInvoiced { get; set; }
/// <summary>
///         /// تاریخ تایید تراکنش
///</summary>
public DateTime? ApproveDate { get; set; }
/// <summary>
///         /// کد قدیمی درگاه پرداخت
///</summary>
public int? OldHekmatId { get; set; }
/// <summary>
///         /// کد رهگیری درگاه یاشیل
///</summary>
public int HekmatReservationNumber { get; set; }
/// <summary>
///         /// شناسه فاکتور
///</summary>
public int? FactorId { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
public int? CreationDateInt { get; set; }
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
public int? ApproveDateInt { get; set; }
public byte[] EncryptedNationalCode { get; set; }
public byte[] EncryptedMobile { get; set; }
public byte[] EncryptedTell { get; set; }
public byte[] HashedNationalCode { get; set; }
public byte[] HashedMobile { get; set; }
public byte[] EncryptedCallBackUrl { get; set; }
public string YearMonth { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual HekmatTerminal HekmatTerminal { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual YashilHekmatTerminal YashilHekmatTerminal { get; set; }
public virtual ICollection<HekmatPaymentGatewayModifiedTransaction>
    HekmatPaymentGatewayModifiedTransaction { get; set; }
    }
 }
