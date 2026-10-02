using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class DailyClassStatistics  
    {
public string OpenningDate { get; set; }
public int? ClassCount { get; set; }
public int? ClassStudentCount { get; set; }
public int? RoolCallCount { get; set; }
    }
 }
