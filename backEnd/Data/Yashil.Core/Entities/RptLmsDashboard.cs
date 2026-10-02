using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class RptLmsDashboard  
    {
public string CompanyTitle { get; set; }
public string ProductTitle { get; set; }
public int ProductSalesPlanningId { get; set; }
public int? ProductSalesPlanningStartDate { get; set; }
public int? ProductSalesPlanningEndDate { get; set; }
public string ExamCategoryTitle { get; set; }
public string ExamTitle { get; set; }
public int Id { get; set; }
public int? ExamUsers { get; set; }
public int? AcceptedExam { get; set; }
public int? NotAcceptedExam { get; set; }
public int? NotParticipate { get; set; }
    }
 }
