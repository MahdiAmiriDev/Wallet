using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;
namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{
	public class HekmatPaymentGatewayTransactionBulkRequestCategoryListViewModel:IBaseViewModel
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
	
		public string YashilHekmatTerminalTitle { get; set; }

        public int yashilHekmatTerminalId { get; set; }

        public string CodePath { get; set; }
	
		public string AccessLevelTitle { get; set; }
	
	}


	public class HekmatPaymentGatewayTransactionBulkRequestCategoryEditModel:IBaseViewModel
	{
		public int ViewModelId
		{
			get => Id;
		    set => Id = value;
		}
	
		public int Id { get; set; }		
	
		public int? ParentId { get; set; }		
	
		[StringLength(300)]
		public string? Code { get; set; }		
	
		[StringLength(600)]
		[Required]
		
		public string Title { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int YashilHekmatTerminalId { get; set; }		
	
		[StringLength(800)]
		public string? CodePath { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		public int AccessLevelId { get; set; }		
	
	}

	public class HekmatPaymentGatewayTransactionBulkRequestCategorySimpleViewModel:IBaseViewModel
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
}      
