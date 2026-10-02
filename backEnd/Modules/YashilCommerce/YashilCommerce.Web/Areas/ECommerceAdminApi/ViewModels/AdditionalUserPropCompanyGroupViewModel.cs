using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;
namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
	public class AdditionalUserPropCompanyGroupListViewModel:IBaseViewModel
	{
		public int ViewModelId
	    {
			get => Id;
	        set => Id = value;
	    }
		public int? ParentId { get; set; }
	
		public int Id { get; set; }
	
		public string AccessLevelTitle { get; set; }
	
		public string AdditionalUserPropTitle { get; set; }
	
		public string CompanyGroupTitle { get; set; }
	
	}


	public class AdditionalUserPropCompanyGroupEditModel:IBaseViewModel
	{
		public int ViewModelId
		{
			get => Id;
		    set => Id = value;
		}
	
		public int Id { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int AccessLevelId { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int AdditionalUserPropId { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int CompanyGroupId { get; set; }		
	
	}

	public class AdditionalUserPropCompanyGroupSimpleViewModel:IBaseViewModel
	{
		public int ViewModelId
		{
			get => Id;
		    set => Id = value;
		}
		
		public int Id { get; set; }
		
		
		public string Title { get; set; }
		
	}
}      
