using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class OldSiteRefundDetail  
    {
public int Id { get; set; }
/// <summary>
///         /// شماره گردش تراکنش -hekmat_payment_id
///</summary>
public int ReservationNumber { get; set; }
/// <summary>
///         /// شماره کارت
///</summary>
public string CardNumber { get; set; }
/// <summary>
///         /// مبلغ
///</summary>
public decimal Total { get; set; }
/// <summary>
///         /// تعداد اقساط
///</summary>
public int InstallmentCount { get; set; }
/// <summary>
///         /// تاریخ تایید تراکنش
///</summary>
public int PaidDateInt { get; set; }
/// <summary>
///         /// باشگاه مشتریان
///</summary>
public int CustomerClubId { get; set; }
/// <summary>
///         /// زمان ایجاد
///</summary>
public DateTime CreationDate { get; set; }
public int? RefundDetailGroupId { get; set; }
public string NationalCode { get; set; }
public string HekmatOwnerNationalCode { get; set; }
public string MobileNumber { get; set; }
public string ApplicationTitle { get; set; }
public int? RefundType { get; set; }
public int? PersonBankAccountId { get; set; }
public long? OldPaymentAmount { get; set; }
public long? NewPaymentAmount { get; set; }
public long? RefundAmount { get; set; }
public byte[] EncryptedHekmatOwnerNationalCode { get; set; }
public byte[] EncryptedNationalCode { get; set; }
public byte[] EncryptedMobileNumber { get; set; }

    public virtual ItemGroup RefundDetailGroup { get; set; }
    }
 }
