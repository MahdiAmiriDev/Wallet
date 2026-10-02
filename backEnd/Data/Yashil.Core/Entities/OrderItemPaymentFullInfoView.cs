using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class OrderItemPaymentFullInfoView  
    {
public int PaymentId { get; set; }
public string NationalCode { get; set; }
public string OrderStatusTitle { get; set; }
public int OrderId { get; set; }
public int OrderItemId { get; set; }
public bool OrderItemDeleted { get; set; }
public decimal? OrderItemTotal { get; set; }
public int? PaymentMethodType { get; set; }
public string PaymentMethodTitle { get; set; }
public decimal? PaymentTotal { get; set; }
public long? PaymentTotalInt { get; set; }
public decimal? NewPaymentAmount { get; set; }
public int? PaidDateInt { get; set; }
public string InstallmentTrackingCode { get; set; }
public string CardNumber { get; set; }
public long? CardNumberInt { get; set; }
public string CustomerRefNum { get; set; }
public int? InstallmentCount { get; set; }
public int PaymentStatusType { get; set; }
    }
 }
