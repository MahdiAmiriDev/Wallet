using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class WrongOrderItemSubsidiesView  
    {
public string NationalCode { get; set; }
public string CardNumber { get; set; }
public decimal PriceIncDiscount { get; set; }
public decimal? CalcSubsidy { get; set; }
public decimal? SubsidyPrice { get; set; }
public decimal? CalcSubsidyCeilingPerHekmatCard { get; set; }
public int? SubsidyCeilingPerHekmatCard { get; set; }
public decimal? CitySubsidyFactor { get; set; }
    }
 }
