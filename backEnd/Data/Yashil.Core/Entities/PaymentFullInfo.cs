using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class PaymentFullInfo  
    {
public int PaymentId { get; set; }
public int OrderId { get; set; }
public int? PaymentMethodType { get; set; }
public string PaymentMethodTitle { get; set; }
public decimal PaymentTotal { get; set; }
public long? PaymentTotalInt { get; set; }
public decimal? NewPaymentAmount { get; set; }
public string RefundDetailGroupState { get; set; }
public int? PaidDateInt { get; set; }
public string InstallmentTrackingCode { get; set; }
public string CustomerRefNum { get; set; }
public int? InstallmentCount { get; set; }
public int PaymentStatusType { get; set; }
public int ApplicationId { get; set; }
    }
 }
