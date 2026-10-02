using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class UploadedCertificate  :IBaseEntity<int>
    {
public int Id { get; set; }
public string IssueDateInt { get; set; }
public string Number { get; set; }
public string CityTitle { get; set; }
public string RepresentationTitle { get; set; }
public string TeacherName { get; set; }
public string CourseCode { get; set; }
public string CourseTitle { get; set; }
public string CourseType { get; set; }
public string CourseDuration { get; set; }
public string SexTitle { get; set; }
public string Name { get; set; }
public string LastName { get; set; }
public string FatherName { get; set; }
public string LatinName { get; set; }
public string LatinLastName { get; set; }
public string NationalCode { get; set; }
public string IdentityNumber { get; set; }
public string PlaceOfIssue { get; set; }
public string Score { get; set; }
public string MobileNumber { get; set; }
public string Tell { get; set; }
public string Email { get; set; }
public string DateOfBirthInt { get; set; }
public string LastEvidence { get; set; }
public string FieldOfStudy { get; set; }
public string CoursePrice { get; set; }
public string TeacherMobile { get; set; }
public int CreateBy { get; set; }
public int? ModifyBy { get; set; }
public DateTime CreationDate { get; set; }
public DateTime? ModificationDate { get; set; }
public bool Deleted { get; set; }
public bool? NationalCodeIsInvalid { get; set; }
public bool? CourseTitleIsInvalid { get; set; }
public bool? CourseDurationIsInvalid { get; set; }
public bool? CourseTypeIsInvalid { get; set; }
public bool? ScoreIsInvalid { get; set; }
public string ErrorMessage { get; set; }
public bool? NameIsInvalid { get; set; }
public bool? LastNameIsInvalid { get; set; }
public bool? IssueDateIntIsInvalid { get; set; }
public bool? NumberIsInvalid { get; set; }
public int? AppDocumentId { get; set; }

    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    }
 }
