using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class PersianDateMonth  :IBaseEntity<int>
    {
public PersianDateMonth()
{
    CustomerClubPaymentInstallment = new HashSet<CustomerClubPaymentInstallment>();
    FactorGroupItem = new HashSet<FactorGroupItem>();
    FactorPaymentInstallment = new HashSet<FactorPaymentInstallment>();
    Payment = new HashSet<Payment>();
 }

public int YearMonth { get; set; }
public int YearNumber { get; set; }
public int MonthNumber { get; set; }
public int? RowNumber { get; set; }
public string StrYearMonth { get; set; }
public int Id { get; set; }
public int CreateBy { get; set; }
public int? ModifyBy { get; set; }
public DateTime CreationDate { get; set; }
public DateTime? ModificationDate { get; set; }
public bool Deleted { get; set; }

    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual MonthNumber MonthNumberNavigation { get; set; }
public virtual ICollection<CustomerClubPaymentInstallment>
    CustomerClubPaymentInstallment { get; set; }
public virtual ICollection<FactorGroupItem>
    FactorGroupItem { get; set; }
public virtual ICollection<FactorPaymentInstallment>
    FactorPaymentInstallment { get; set; }
public virtual ICollection<Payment>
    Payment { get; set; }
    }
 }
