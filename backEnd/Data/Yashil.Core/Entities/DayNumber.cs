using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class DayNumber  
    {
public DayNumber()
{
    Order = new HashSet<Order>();
 }

public int Id { get; set; }

public virtual ICollection<Order>
    Order { get; set; }
    }
 }
