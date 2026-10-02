using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class RptLmsVirtualClassDashboard  
    {
public int VirtualClassId { get; set; }
public string CompanyTitle { get; set; }
public string RepresentationTitle { get; set; }
public string ProductTitle { get; set; }
public string CategoryTitle { get; set; }
public string VirtualClassTitle { get; set; }
public DateTime CreationDate { get; set; }
public string VirtualClassStartTime { get; set; }
public string VirtualClassEndTime { get; set; }
public string VirtualClassOpenningDate { get; set; }
public string DayOfWeek { get; set; }
public int? DayNumber { get; set; }
public int? StudentCount { get; set; }
public int VirtualClassRollCall { get; set; }
public int YearNumber { get; set; }
public int YearMonth { get; set; }
public int MonthNumber { get; set; }
public int? RowNumber { get; set; }
    }
 }
