using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

   public class UploadedFactorPaymentListViewModel:IBaseViewModel
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

        [StringValidation] public string AppDocumentTitle { get; set; }

        [StringValidation] public string FactorCode { get; set; }

        public bool FactorCodeIsInvalid { get; set; }

        [StringValidation] public string Price { get; set; }

        public bool PriceIsInvalid { get; set; }

        [StringValidation] public string PaymentDate { get; set; }

        public bool PaymentDateIsInvalid { get; set; }

        [StringValidation] public string InstallmentCount { get; set; }

        public bool InstallmentCountIsInvalid { get; set; }

        [StringValidation] public string BankAccountNumber { get; set; }

        public bool BankAccountNumberIsInvalid { get; set; }

    }


	    public class UploadedFactorPaymentEditModel:IBaseViewModel
        {
	        public int ViewModelId
	            {
	                get => Id;
	                set => Id = value;
	            }
				public int Id { get; set; }		
				[StringLength(300)][StringValidation] public string Code { get; set; }		
				[StringLength(600)] [StringValidation] public string Title { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int AppDocumentId { get; set; }		
				[StringLength(300)][StringValidation] public string FactorCode { get; set; }		
				public bool FactorCodeIsInvalid { get; set; }		
				[StringLength(300)][StringValidation] public string Price { get; set; }		
				public bool PriceIsInvalid { get; set; }		
				[StringLength(300)][StringValidation] public string PaymentDate { get; set; }		
				public bool PaymentDateIsInvalid { get; set; }		
				[StringLength(300)][StringValidation] public string InstallmentCount { get; set; }		
				public bool InstallmentCountIsInvalid { get; set; }		
				[StringLength(300)][StringValidation] public string BankAccountNumber { get; set; }		
				public bool BankAccountNumberIsInvalid { get; set; }		
				[StringValidation] public string Description { get; set; }		
}

  



public class UploadedFactorPaymentSimpleViewModel:IBaseViewModel
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
