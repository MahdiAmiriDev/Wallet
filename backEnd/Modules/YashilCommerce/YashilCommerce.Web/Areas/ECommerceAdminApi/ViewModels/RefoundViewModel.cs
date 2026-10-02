using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

   public class RefundListViewModel:IBaseViewModel
    {
		public int ViewModelId
	        {
	            get => Id;
	            set => Id = value;
	        }
		public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string OrderTitle { get; set; }

        public decimal OldPrice { get; set; }

        public decimal NewPrice { get; set; }

        public decimal? TotalRefundAmount { get; set; }

        public bool IsComplete { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

    }


	    public class RefundEditModel:IBaseViewModel
        {
	        public int ViewModelId
	            {
	                get => Id;
	                set => Id = value;
	            }
				public int Id { get; set; }		
				[StringLength(300)][StringValidation] public string Code { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int OrderId { get; set; }		
 
				[Required]				public decimal OldPrice { get; set; }		
 
				[Required]				public decimal NewPrice { get; set; }		
				public decimal? TotalRefundAmount { get; set; }		
				public bool IsComplete { get; set; }		
				[StringValidation] public string Description { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int AccessLevelId { get; set; }		
}

  



public class RefundSimpleViewModel:IBaseViewModel
        {
	        public int ViewModelId
	            {
	                get => Id;
	                set => Id = value;
	            }
        public int Id { get; set; }

							[StringValidation] public string Title { get; set; }
			    }

}      
