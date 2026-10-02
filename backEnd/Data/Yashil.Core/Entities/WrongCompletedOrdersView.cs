using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class WrongCompletedOrdersView  
    {
public string OrderStatusType { get; set; }
public int CreationDateInt { get; set; }
public int OrderId { get; set; }
public decimal? OrderItemTotal { get; set; }
public decimal? SuccessFullPaymentTotal { get; set; }
public string ApplicationTitle { get; set; }
public string Detail { get; set; }
    }
 }
