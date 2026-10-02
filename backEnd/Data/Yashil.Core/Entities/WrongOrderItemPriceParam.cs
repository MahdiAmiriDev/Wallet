using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class WrongOrderItemPriceParam  
    {
public int CreationDateInt { get; set; }
public int OrderId { get; set; }
public int OrderItemId { get; set; }
public string OrderNumber { get; set; }
public string NationalCode { get; set; }
public string SubsideType { get; set; }
public decimal? ProductPrice { get; set; }
public decimal? WageAmountExcTax { get; set; }
public decimal? K { get; set; }
public decimal? Subsidy { get; set; }
public decimal? TaxPercentage { get; set; }
public long? TaxPrice { get; set; }
public long? CalcTaxPrice { get; set; }
public long? DiffTaxPrice { get; set; }
public long? Total { get; set; }
public long? CalcTotal { get; set; }
    }
 }
