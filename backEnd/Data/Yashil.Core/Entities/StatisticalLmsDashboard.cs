using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class StatisticalLmsDashboard  
    {
public int Id { get; set; }
public int? CompanyCount { get; set; }
public int? RepresentationCount { get; set; }
public int? CategoryCount { get; set; }
public int? UserCount { get; set; }
public int? StudentCount { get; set; }
public int? TeacherCount { get; set; }
public int? PersonHour { get; set; }
public int? ProductCount { get; set; }
public int? ProductSalesPlanningCount { get; set; }
public int? VirtualClassCount { get; set; }
    }
 }
