using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class YearNumber  
    {
public YearNumber()
{
    Order = new HashSet<Order>();
 }

public int Id { get; set; }
public int? Title { get; set; }

public virtual ICollection<Order>
    Order { get; set; }
    }
 }
