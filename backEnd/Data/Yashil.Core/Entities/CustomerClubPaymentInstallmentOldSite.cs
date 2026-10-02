using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class CustomerClubPaymentInstallmentOldSite  
    {
public int Id { get; set; }
public decimal? InstallmentAmount { get; set; }
public string MonthTitle { get; set; }
public int YearNumber { get; set; }
public string ReservationNumber { get; set; }
public int? PaidDateInt { get; set; }
public string CardNumber { get; set; }
public string CustomerClubCardCode { get; set; }
public int? InstallmentCount { get; set; }
public string ApplicationTitle { get; set; }
public string Description { get; set; }
public int YearMonth { get; set; }
public long? InstallmentNumber { get; set; }
public int Refounded { get; set; }
/// <summary>
///         /// گروه بندی مطالبات
///</summary>
public int? DemandGroupId { get; set; }
public byte[] CardNumberEncrypted { get; set; }

    public virtual ItemGroup DemandGroup { get; set; }
    }
 }
