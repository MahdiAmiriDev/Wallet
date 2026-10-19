using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;
namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{
	public class YashilHekmatTerminalSupplierListViewModel:IBaseViewModel
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
	
		public int YashilHekmatTerminalId { get; set; }
	
		public string UserTitle { get; set; }
	
		public int SupplierId { get; set; }
	
		public string SupplierTitle { get; set; }
	
		public string CodePath { get; set; }
	
		public string AccessLevelTitle { get; set; }
	
	}


	public class YashilHekmatTerminalSupplierEditModel:IBaseViewModel
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
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int YashilHekmatTerminalId { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int UserId { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int SupplierId { get; set; }		
	
		[StringLength(1000)]
		public string SupplierTitle { get; set; }		
	
		[StringLength(800)]
		public string CodePath { get; set; }		
	
		[Range(0,int.MaxValue)]
		
		[Required]
		
		public int AccessLevelId { get; set; }		
	
	}

	public class YashilHekmatTerminalSupplierSimpleViewModel:IBaseViewModel
	{
		public int ViewModelId
		{
			get => Id;
		    set => Id = value;
		}
		
		public int Id { get; set; }
        
		
		public string Title { get; set; }

        public int SupplierId { get; set; }

        public int YashilHekmatTerminalId { get; set; }

    }
}      
