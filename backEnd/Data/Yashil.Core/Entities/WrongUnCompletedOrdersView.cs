using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class WrongUnCompletedOrdersView  
    {
public string OrderStatusType { get; set; }
public int CreationDateInt { get; set; }
public int OrderId { get; set; }
public decimal? OrderItemTotal { get; set; }
public decimal? PaymentTotal { get; set; }
public string ApplicationTitle { get; set; }
    }
 }
