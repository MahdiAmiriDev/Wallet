using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

   public class RewardPointsHistoryListViewModel:IBaseViewModel
    {
		public int ViewModelId
	        {
	            get => Id;
	            set => Id = value;
	        }
		public int? ParentId { get; set; }
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int StoreId { get; set; }

        public int Points { get; set; }

        public int? PointsBalance { get; set; }

        public decimal UsedAmount { get; set; }

        public int? ValidPoints { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string Title { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

    }


	    public class RewardPointsHistoryEditModel:IBaseViewModel
        {
	        public int ViewModelId
	            {
	                get => Id;
	                set => Id = value;
	            }
				public int Id { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int CustomerId { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int StoreId { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int Points { get; set; }		
				public int? PointsBalance { get; set; }		
 
				[Required]				public decimal UsedAmount { get; set; }		
				[StringValidation] public string Message { get; set; }		
				public int? ValidPoints { get; set; }		
				[StringLength(300)][StringValidation] public string Code { get; set; }		
				[StringLength(600)] [StringValidation] 
				[Required]
                public string Title { get; set; }		
				[StringValidation] public string Description { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int AccessLevelId { get; set; }		
}

  



public class RewardPointsHistorySimpleViewModel:IBaseViewModel
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
