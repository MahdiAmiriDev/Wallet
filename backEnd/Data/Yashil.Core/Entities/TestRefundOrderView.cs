using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class TestRefundOrderView  
    {
public int OrderId { get; set; }
public decimal? OrderTotal { get; set; }
public decimal? PaymentTotal { get; set; }
public int? PaymentCount { get; set; }
    }
 }
