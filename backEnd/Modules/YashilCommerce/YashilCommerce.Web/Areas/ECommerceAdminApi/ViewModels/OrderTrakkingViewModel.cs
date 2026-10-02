using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

   public class OrderTrakkingListViewModel:IBaseViewModel
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

        public int? ProblemType { get; set; }

        public int? Customer { get; set; }

        [StringValidation] public string ReferenceToTitle { get; set; }

        public int? Type { get; set; }

        [StringValidation] public string OrderTitle { get; set; }

        [StringValidation] public string OrderItemTitle { get; set; }

        public bool? Archive { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

    }


	    public class OrderTrakkingEditModel:IBaseViewModel
        {
	        public int ViewModelId
	            {
	                get => Id;
	                set => Id = value;
	            }
				public int Id { get; set; }		
				[StringLength(300)][StringValidation] public string Code { get; set; }		
				[StringLength(600)] [StringValidation] 
				[Required]
                public string Title { get; set; }		
				public int? ProblemType { get; set; }		
				public int? Customer { get; set; }		
				public int? ReferenceTo { get; set; }		
				[StringValidation] public string ApproachDescription { get; set; }		
				[StringValidation] public string ActionDescription { get; set; }		
				public int? Type { get; set; }		
				public int? OrderId { get; set; }		
				public int? OrderItemId { get; set; }		
				public bool? Archive { get; set; }		
				[StringValidation] public string Description { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int AccessLevelId { get; set; }		
}

  



public class OrderTrakkingSimpleViewModel:IBaseViewModel
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
