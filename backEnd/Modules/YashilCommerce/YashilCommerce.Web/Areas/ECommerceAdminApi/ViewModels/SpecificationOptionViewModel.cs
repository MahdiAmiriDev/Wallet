using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

   public class SpecificationOptionListViewModel:IBaseViewModel
    {
		public int ViewModelId
	        {
	            get => Id;
	            set => Id = value;
	        }
		public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string SpecificationTitle { get; set; }

        [StringValidation] public string Title { get; set; }

        public int DisplayOrder { get; set; }

        [StringValidation] public string ColorSquaresRgb { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

    }


	    public class SpecificationOptionEditModel:IBaseViewModel
        {
	        public int ViewModelId
	            {
	                get => Id;
	                set => Id = value;
	            }
				public int Id { get; set; }		
				[StringLength(300)][StringValidation] public string Code { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int SpecificationId { get; set; }		
				[StringLength(600)] [Required]				[StringValidation] public string Title { get; set; }		
				[StringValidation] public string Description { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int DisplayOrder { get; set; }		
				[StringLength(200)][StringValidation] public string ColorSquaresRgb { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int AccessLevelId { get; set; }		
}

  



public class SpecificationOptionSimpleViewModel:IBaseViewModel
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
