using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class MonthNumber  
    {
public MonthNumber()
{
    Order = new HashSet<Order>();
    PersianDateMonth = new HashSet<PersianDateMonth>();
 }

public int Id { get; set; }
public string Title { get; set; }

public virtual ICollection<Order>
    Order { get; set; }
public virtual ICollection<PersianDateMonth>
    PersianDateMonth { get; set; }
    }
 }
