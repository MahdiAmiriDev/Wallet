using System.Collections.Generic;
using JetBrains.Annotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class CertificateListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string IssueDateInt { get; set; }

        [StringValidation] public string Number { get; set; }

        [StringValidation] public string CityTitle { get; set; }

        [StringValidation] public string RepresentationTitle { get; set; }

        [StringValidation] public string TeacherName { get; set; }

        [StringValidation] public string CourseCode { get; set; }

        [StringValidation] public string CourseTitle { get; set; }

        [StringValidation] public string CourseType { get; set; }

        [StringValidation] public string CourseDuration { get; set; }

        [StringValidation] public string SexTitle { get; set; }

        [StringValidation] public string Name { get; set; }

        [StringValidation] public string LastName { get; set; }

        [StringValidation] public string FatherName { get; set; }

        [StringValidation] public string LatinName { get; set; }

        [StringValidation] public string LatinLastName { get; set; }

        [StringValidation] public string NationalCode { get; set; }

        [StringValidation] public string IdentityNumber { get; set; }

        [StringValidation] public string PlaceOfIssue { get; set; }

        [StringValidation] public string Score { get; set; }

        [StringValidation] public string MobileNumber { get; set; }

        [StringValidation] public string Tell { get; set; }

        [StringValidation] public string Email { get; set; }

        [StringValidation] public string DateOfBirthInt { get; set; }

        [StringValidation] public string LastEvidence { get; set; }

        [StringValidation] public string FieldOfStudy { get; set; }

        [StringValidation] public string CoursePrice { get; set; }

        [StringValidation] public string TeacherMobile { get; set; }

    }

    public class CertificateEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringValidation] public string? IssueDateInt { get; set; }
        [StringValidation] public string? Number { get; set; }
        [StringValidation] public string? CityTitle { get; set; }
        [StringValidation] public string? RepresentationTitle { get; set; }
        [StringValidation] public string? TeacherName { get; set; }
        [StringValidation] public string? CourseCode { get; set; }
        [StringValidation] public string? CourseTitle { get; set; }
        [StringValidation] public string? CourseType { get; set; }
        [StringValidation] public string? CourseDuration { get; set; }
        [StringValidation] public string? SexTitle { get; set; }
        [StringValidation] public string? Name { get; set; }
        [StringValidation] public string? LastName { get; set; }
        [StringValidation] public string? FatherName { get; set; }
        [StringValidation] public string? LatinName { get; set; }
        [StringValidation] public string? LatinLastName { get; set; }
        [StringValidation] public string? NationalCode { get; set; }
        [StringValidation] public string? IdentityNumber { get; set; }
        [StringValidation] public string? PlaceOfIssue { get; set; }
        [StringValidation] public string Score { get; set; }
        [StringValidation] public string? MobileNumber { get; set; }
        [StringValidation] public string? Tell { get; set; }
        [StringValidation] public string? Email { get; set; }
        [StringValidation] public string? DateOfBirthInt { get; set; }
        [StringValidation] public string? LastEvidence { get; set; }
        [StringValidation] public string? FieldOfStudy { get; set; }
        [StringValidation] public string? CoursePrice { get; set; }
        [StringValidation] public string? TeacherMobile { get; set; }
    }

    public class CertificateSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
    }

    public class CertificateStatisticViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// تاریخ صدور
        /// </summary>
        [StringValidation] public string IssueDateInt { get; set; }
        /// <summary>
        /// شماره گواهینامه
        /// </summary>
        [StringValidation] public string Number { get; set; }
        /// <summary>
        /// نام دوره
        /// </summary>
        [StringValidation] public string CourseTitle { get; set; }
        /// <summary>
        /// نام  و نام خانوادگی
        /// </summary>
        [StringValidation] public string FullName { get; set; }

        /// <summary>
        /// کد ملی
        /// </summary>
        [StringValidation] public string NationalCode { get; set; }
        /// <summary>
        /// مدت دوره
        /// </summary>
        [StringValidation] public string CourseDuration { get; set; }
        /// <summary>
        /// نمره
        /// </summary>
        [StringValidation] public string Score { get; set; }
        /// <summary>
        /// نوع دوره
        /// </summary>
        [StringValidation] public string CourseType { get; set; }




    }

    public class CertificateCustomViewModel
    {
        /// <summary>
        /// لیست اطلاعات مربوط به گواهینامه 
        /// </summary>
        public List<CertificateStatisticViewModel> CertificatesStatistic { get; set; }
        /// <summary>
        ///  دریافت اطلاعات موفقیت آمیز بود یا خیر
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// پیام
        /// </summary>
        [StringValidation] public string Message { get; set; }

    }

    public class CertificateSearchParam
    {
        [StringValidation] public string CertificateNumber { get; set; }
        [StringValidation] public string NationalCode { get; set; }
    }
}