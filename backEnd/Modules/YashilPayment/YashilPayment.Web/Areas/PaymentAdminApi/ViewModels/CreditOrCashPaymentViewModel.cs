using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{

   public class CreditOrCashPaymentListViewModel:IBaseViewModel
    {
		public int ViewModelId
	        {
	            get => Id;
	            set => Id = value;
	        }
		public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string? Code { get; set; }

        [StringValidation] public string? Title { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

        [StringValidation] public string CardTitle { get; set; }

        [StringValidation] public string MerchantTitle { get; set; }

        [StringValidation] public string OrderId { get; set; }

        public DateTime BuyDate { get; set; }

        public int TotalAmount { get; set; }

        public int? InstallmentNumber { get; set; }

        [StringValidation] public string StatusTitle { get; set; }

        public bool CreditOrCash { get; set; }

		[StringValidation] public string CreditOrCashString { get { if (CreditOrCash) { return "اعتباری"; } else { return "ریالی"; } } }

		public DateTime? ReturnedDate { get; set; }

        public int? ReturnedAmount { get; set; }

    }


	    public class CreditOrCashPaymentEditModel:IBaseViewModel
        {
	        public int ViewModelId
	            {
	                get => Id;
	                set => Id = value;
	            }
				public int Id { get; set; }		
				[StringLength(300)] [StringValidation] public string Code { get; set; }		
				[StringLength(600)] [StringValidation] public string? Title { get; set; }		
				[StringValidation] public string Description { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int AccessLevelId { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int CardId { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]	
                public int MerchantId { get; set; }		

				[StringLength(300)]
                [Required]			
                [StringValidation] public string OrderId { get; set; }		
 
				[Required]				public DateTime BuyDate { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int TotalAmount { get; set; }		
				public int? InstallmentNumber { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int Status { get; set; }		
				public bool CreditOrCash { get; set; }		
				public DateTime? ReturnedDate { get; set; }		
				public int? ReturnedAmount { get; set; }		
}

  



public class CreditOrCashPaymentSimpleViewModel:IBaseViewModel
        {
	        public int ViewModelId
	            {
	                get => Id;
	                set => Id = value;
	            }
        public int Id { get; set; }

			        [StringValidation] public string? Title { get; set; }

			    }

}      
