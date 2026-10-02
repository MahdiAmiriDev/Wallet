using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class MaharatiProductsView  
    {
public string Title { get; set; }
public decimal? Price { get; set; }
public decimal? DiscountPrice { get; set; }
public decimal SaleConfigurationDiscount { get; set; }
public decimal? ProductSalesPlanningDiscount { get; set; }
    }
 }
