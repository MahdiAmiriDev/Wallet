using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;
namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
	public class SiteVisitListViewModel:IBaseViewModel
	{
		public int ViewModelId
	    {
			get => Id;
	        set => Id = value;
	    }
		public int? ParentId { get; set; }
	
		public int Id { get; set; }
	
		public string Url { get; set; }
	
		public int VisitCount { get; set; }
	
		public string Device { get; set; }
	
		public string ParentTitle { get; set; }
	
		public string Code { get; set; }
	
		public string Title { get; set; }
	
		public string CodePath { get; set; }
	
		public string AccessLevelTitle { get; set; }
	
	}


	public class SiteVisitEditModel:IBaseViewModel
	{
		public int ViewModelId
		{
			get => Id;
		    set => Id = value;
		}
	
		public int Id { get; set; }		
	
		[StringLength(400)]
		[Required]
		
		public string Url { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int VisitCount { get; set; }		
	
		[StringLength(100)]
		[Required]
		
		public string Device { get; set; }		
	
		public int? ParentId { get; set; }		
	
		[StringLength(300)]
		public string Code { get; set; }		
	
		[StringLength(600)]
		public string Title { get; set; }		
	
		[StringLength(800)]
		public string CodePath { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int AccessLevelId { get; set; }		
	
	}

	public class SiteVisitSimpleViewModel:IBaseViewModel
	{
		public int ViewModelId
		{
			get => Id;
		    set => Id = value;
		}
		
		public int Id { get; set; }
		
		public int ParentId { get; set; }
		
		public string Title { get; set; }
		
		
	}

	/// <summary>
	/// ویو مدل مربوط به
	/// siteVisit
	/// </summary>
    public class SiteVisitViewModel
    {
		/// <summary>
		/// تعداد دفعات بازدید سایت در امروز
		/// </summary>
        public int CurrentDaySiteVisitCount { get; set; }

		/// <summary>
		/// آمار کل بازدید سایت
		/// </summary>
        public int TotalSiteVisitCount { get; set; }

		/// <summary>
		/// آمار بازدید دیروز سایت
		/// </summary>
        public int PerviousDayTotalSiteVisitCount { get; set; }
    }
}      
