using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;
namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
	public class UploadedCertificateListViewModel:IBaseViewModel
	{
		public int ViewModelId
	    {
			get => Id;
	        set => Id = value;
	    }

        public int Id { get; set; }
	
		public string IssueDateInt { get; set; }
	
		public string Number { get; set; }
	
		public string CityTitle { get; set; }
	
		public string RepresentationTitle { get; set; }
	
		public string TeacherName { get; set; }

        public string CourseTitle { get; set; }
	
		public string CourseType { get; set; }
	
		public string CourseDuration { get; set; }
	
		public string SexTitle { get; set; }

        public string Score { get; set; }

        public string FieldOfStudy { get; set; }
	
		public string CoursePrice { get; set; }

        public string TeacherMobile { get; set; }
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

    }


	public class UploadedCertificateEditModel:IBaseViewModel
	{
		public int ViewModelId
		{
			get => Id;
		    set => Id = value;
		}
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int Id { get; set; }		
	
		[StringLength(200)]
		public string IssueDateInt { get; set; }		
	
		[StringLength(200)]
		public string Number { get; set; }		
	
		[StringLength(400)]
		public string CityTitle { get; set; }		
	
		[StringLength(400)]
		public string RepresentationTitle { get; set; }		
	
		[StringLength(200)]
		public string TeacherName { get; set; }		
	
		[StringLength(1000)]
		public string CourseCode { get; set; }		
	
		[StringLength(1000)]
		public string CourseTitle { get; set; }		
	
		[StringLength(200)]
		public string CourseType { get; set; }		
	
		[StringLength(200)]
		public string CourseDuration { get; set; }		
	
		[StringLength(200)]
		public string SexTitle { get; set; }		
	
		[StringLength(200)]
		public string PlaceOfIssue { get; set; }		
	
		[StringLength(200)]
		public string Score { get; set; }		
	
		[StringLength(600)]
		public string Email { get; set; }		
	
		[StringLength(200)]
		public string DateOfBirthInt { get; set; }		
	
		[StringLength(200)]
		public string LastEvidence { get; set; }		
	
		[StringLength(200)]
		public string FieldOfStudy { get; set; }		
	
		[StringLength(200)]
		public string CoursePrice { get; set; }		
	
		[StringLength(200)]
		public string TeacherMobile { get; set; }		
	
	}

	public class UploadedCertificateSimpleViewModel:IBaseViewModel
	{
		public int ViewModelId
		{
			get => Id;
		    set => Id = value;
		}
		
		public int Id { get; set; }
		
		
		public string Title { get; set; }
		
	}

    public class UploadedCertificateInfoViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// عنوان
        ///</summary>
        public string Title { get; set; }
        /// <summary>
        ///نام واقعی سند
        ///</summary>
        public string OriginalName { get; set; }

        /// <summary>
        ///  نام و نام خانوادگی کاربر ایجاد کننده
        ///</summary>
        public string CreateByFullName { get; set; }
        /// <summary>
        /// تاریخ درج
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}      

