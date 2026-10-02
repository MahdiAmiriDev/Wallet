using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

   public class CustomerClubCardListViewModel:IBaseViewModel
    {
		public int ViewModelId
	        {
	            get => Id;
	            set => Id = value;
	        }
		public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string Title { get; set; }

        [StringValidation] public string PersonTitle { get; set; }

        [StringValidation] public string CustomerClubTitle { get; set; }

        [StringValidation] public string CardNumber { get; set; }

        public decimal? MaximumPurchaseAmount { get; set; }

        public int? MaximumNumberPurchases { get; set; }

        public decimal? AmountUsed { get; set; }

        public int ToDate { get; set; }

        public int FromDate { get; set; }

        public bool IsActive { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

    }


	    public class CustomerClubCardEditModel:IBaseViewModel
        {
	        public int ViewModelId
	            {
	                get => Id;
	                set => Id = value;
	            }
				public int Id { get; set; }		
				[StringLength(300)][StringValidation] public string Code { get; set; }		
				[StringLength(600)] [StringValidation] public string Title { get; set; }		
				public int? PersonId { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int CustomerClubId { get; set; }		
				[StringLength(200)][StringValidation]
				[Required]
                public string CardNumber { get; set; }		
				public decimal? MaximumPurchaseAmount { get; set; }		
				public int? MaximumNumberPurchases { get; set; }		
				public decimal? AmountUsed { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int ToDate { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int FromDate { get; set; }		
				public bool IsActive { get; set; }		
				[StringValidation] public string Description { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int AccessLevelId { get; set; }		
}

  



public class CustomerClubCardSimpleViewModel:IBaseViewModel
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
