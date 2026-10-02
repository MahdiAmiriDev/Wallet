using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{

   public class ValidHekmatCardListViewModel:IBaseViewModel
    {
		public int ViewModelId
	        {
	            get => Id;
	            set => Id = value;
	        }
		public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string CardNumber { get; set; }

        [StringValidation] public string HekmatCardTypeTitle { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

    }


	    public class ValidHekmatCardEditModel:IBaseViewModel
        {
	        public int ViewModelId
	            {
	                get => Id;
	                set => Id = value;
	            }
				public int Id { get; set; }		
				[StringLength(32)][StringValidation] 
				[Required]
                public string CardNumber { get; set; }		
				public int? HekmatCardType { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int AccessLevelId { get; set; }		
}

  



public class ValidHekmatCardSimpleViewModel:IBaseViewModel
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
