using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

   public class ContractPaymentMethodListViewModel:IBaseViewModel
    {
		public int ViewModelId
	        {
	            get => Id;
	            set => Id = value;
	        }
		public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string ContractTitle { get; set; }

        [StringValidation] public string PaymentMethodTypeTitle { get; set; }

        public bool ApplyByExpression { get; set; }

        [StringValidation] public string Expression { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

    }


	    public class ContractPaymentMethodEditModel:IBaseViewModel
        {
	        public int ViewModelId
	            {
	                get => Id;
	                set => Id = value;
	            }
				public int Id { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int ContractId { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int PaymentMethodType { get; set; }		
				public bool ApplyByExpression { get; set; }		
				[StringLength(1000)]				[StringValidation] public string Expression { get; set; }		
				[StringValidation] public string Description { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int AccessLevelId { get; set; }		
}

  



public class ContractPaymentMethodSimpleViewModel:IBaseViewModel
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
