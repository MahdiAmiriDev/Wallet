using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{

   public class YashilSchedulerCompanyRepresentationListViewModel:IBaseViewModel
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

        [StringValidation] public string YashilSchedulerTitle { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }

        [StringValidation] public string RepresentationTitle { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

    }


	    public class YashilSchedulerCompanyRepresentationEditModel:IBaseViewModel
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
				[Range(0,int.MaxValue)] 
				[Required]				public int YashilSchedulerId { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int CompanyId { get; set; }		
				public int? Representation { get; set; }		
				[StringValidation] public string Description { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int AccessLevelId { get; set; }		
}

  



public class YashilSchedulerCompanyRepresentationSimpleViewModel:IBaseViewModel
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
