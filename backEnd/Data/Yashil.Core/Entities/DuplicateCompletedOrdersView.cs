using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class DuplicateCompletedOrdersView  
    {
public long? RowNumber { get; set; }
public int Id { get; set; }
public string ProductTitle { get; set; }
public string OrderTitle { get; set; }
public string OrderStatusType { get; set; }
public string ApplicationTitle { get; set; }
public string SubsidyStatusType { get; set; }
public decimal? VezaratSubsidyPrice { get; set; }
public decimal? SataSubsidePrice { get; set; }
public string NationalCode { get; set; }
public string RecipientMobileNumber { get; set; }
public int OrderCreationDate { get; set; }
public int? PaidDateUtcInt { get; set; }
public string UserName { get; set; }
public string CardNumber { get; set; }
    }
 }
