using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class AdditionalPaymentView  
    {
public int OrderId { get; set; }
public string Title { get; set; }
public long? PaymentTotal { get; set; }
public long? OrderTotal { get; set; }
public string NationalCode { get; set; }
public string RecipientMobileNumber { get; set; }
public string UserName { get; set; }
public string ApplicationTitle { get; set; }
public int CreationDateInt { get; set; }
    }
 }
