using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
	public class ProductTagsListViewModel:IBaseViewModel
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
	
		public string ProductTitle { get; set; }
	
		public string TagTypeTitle { get; set; }
	
		public string CodePath { get; set; }
	
		public string AccessLevelTitle { get; set; }
	
	}


	public class ProductTagsEditModel:IBaseViewModel
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
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int ProductId { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int TagType { get; set; }		
	
		[StringLength(800)]
		public string CodePath { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int AccessLevelId { get; set; }

		/// <summary>
		/// توضیحات در قسمت 
		/// detail
		/// تگ های محصولات
		/// </summary>
        public string Description { get; set; }
    }

	public class ProductTagsSimpleViewModel:IBaseViewModel
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
