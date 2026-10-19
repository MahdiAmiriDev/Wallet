using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{

   public class InstallmentListViewModel:IBaseViewModel
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

        [StringValidation] public string? AccessLevelTitle { get; set; }

        public int InstallmentAmount { get; set; }

        public int DueDate { get; set; }

        [StringValidation] public string PaymentTitle { get; set; }

        [StringValidation] public string PaymentStatusTitle { get; set; }

    }


	    public class InstallmentEditModel:IBaseViewModel
        {
	        public int ViewModelId
	            {
	                get => Id;
	                set => Id = value;
	            }
				public int Id { get; set; }		
				[StringLength(300)][StringValidation] public string? Code { get; set; }		
				[StringLength(600)] [StringValidation] public string? Title { get; set; }		
				[StringValidation] public string? Description { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int AccessLevelId { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int InstallmentAmount { get; set; }		
 
				[Required]				public int DueDate { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int PaymentId { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int PaymentStatus { get; set; }		
}

  



public class InstallmentSimpleViewModel:IBaseViewModel
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
