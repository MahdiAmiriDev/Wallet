using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class UserResumeView  
    {
public long? Id { get; set; }
public int UserId { get; set; }
public string NationalId { get; set; }
public string GuardianNationalId { get; set; }
public string Mobile { get; set; }
public string GuardianMobile { get; set; }
public string Email { get; set; }
public string BirthYear { get; set; }
public int? Age { get; set; }
public string Major0 { get; set; }
public string Institution0 { get; set; }
public string InstitutionType { get; set; }
public string Average0 { get; set; }
public string EducationStartYear0 { get; set; }
public string GraduationYear0 { get; set; }
public string Province { get; set; }
public string City { get; set; }
public string MarriageStatus { get; set; }
public string ChildrenCount { get; set; }
public string Gender { get; set; }
public string MilitaryService { get; set; }
public string Grade0 { get; set; }
public string Languages { get; set; }
public string LanguagesMastery { get; set; }
public string PhysicalStatus { get; set; }
public string JobPreferencesProvince { get; set; }
public string JobPreferencesCity { get; set; }
public string JobField { get; set; }
public string Contracts { get; set; }
public string SeniorityLevels { get; set; }
public string CertificatesTitle { get; set; }
public string CertificatesLevel { get; set; }
public string CorporationTitle { get; set; }
public string JobTitle { get; set; }
public string CardNumber { get; set; }
public int? ResumeModificationDate { get; set; }
public int? CreationDate { get; set; }
    }
 }
