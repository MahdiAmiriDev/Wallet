using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class TaxCategoryBk14030627  :IBaseEntity<int>,IApplicationBasedEntity
    {
public int Id { get; set; }
public string Code { get; set; }
public string Title { get; set; }
public decimal Percentage { get; set; }
public string Description { get; set; }
public bool AllowDelete { get; set; }
public int CreateBy { get; set; }
public int? ModifyBy { get; set; }
public DateTime CreationDate { get; set; }
public DateTime? ModificationDate { get; set; }
public int ApplicationId { get; set; }
public int AccessLevelId { get; set; }
public bool Deleted { get; set; }
public int CreatorOrganizationId { get; set; }
public string TitleForConvert { get; set; }
public string UnitOfMeasurement { get; set; }
public bool IsIncludeInsurance { get; set; }
    }
 }
