using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;
namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
	public class ProductGroupListViewModel:IBaseViewModel
	{
		public int ViewModelId
	    {
			get => Id;
	        set => Id = value;
	    }
		public int? ParentId { get; set; }
	
		public int Id { get; set; }
	
		public string ParentTitle { get; set; }
	
		public string Code { get; set; }
	
		public string Title { get; set; }
	
		public string CodePath { get; set; }
	
		public string AccessLevelTitle { get; set; }
	
		public string CategoryGroupTitle { get; set; }
		/// <summary>
		/// شامل بیمه می شود
		/// </summary>
        public bool IsIncludeInsurance { get; set; }
		/// <summary>
		/// کد اصلی
		/// </summary>
        public string MainCode { get; set; }

        /// <summary>
        /// آیا شامل مالیت می شود؟
        /// </summary>
        public bool IsIncludeTax { get; set; }

    }


	public class ProductGroupEditModel:IBaseViewModel
	{
		public int ViewModelId
		{
			get => Id;
		    set => Id = value;
		}
	
		public int Id { get; set; }		
	
		public int? ParentId { get; set; }		
	
		[StringLength(300)]
		public string Code { get; set; }		
	
		[StringLength(600)]
		[Required]
		
		public string Title { get; set; }		
	
		[StringLength(800)]
		public string CodePath { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int AccessLevelId { get; set; }		
	
		public int? CategoryGroupId { get; set; }

        public bool? IsIncludeInsurance { get; set; }
        public string MainCode { get; set; }

		/// <summary>
		/// آیا شامل مالیت می شود؟
		/// </summary>
        public bool IsIncludeTax { get; set; }

    }

	public class ProductGroupSimpleViewModel:IBaseViewModel
	{
		public int ViewModelId
		{
			get => Id;
		    set => Id = value;
		}
		
		public int Id { get; set; }
        
		public string Title { get; set; }

        public string Code { get; set; }

        public bool IsIncludeTax { get; set; }
		
		
	}
}      
