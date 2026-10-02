using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class WrongOrderItemPayments  
    {
public int OrderId { get; set; }
public string NationalCode { get; set; }
public int OrderStatusType { get; set; }
public decimal? SumPrice { get; set; }
public decimal Total { get; set; }
public int ApplicationId { get; set; }
public int? OrderItemId { get; set; }
    }
 }
