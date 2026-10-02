using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

   public class HekmatEffectlessCanceledTransactionListViewModel:IBaseViewModel
    {
		public int ViewModelId
	        {
	            get => Id;
	            set => Id = value;
	        }
		public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string CartNumber { get; set; }

        public int? TerminalNumber { get; set; }

        public int? TerminalRotationRow { get; set; }

        [StringValidation] public string TransactionDate { get; set; }

        public Int64? ModifiedTransactionPrice { get; set; }

        [StringValidation] public string StoreName { get; set; }

        public int? RefundDetailId { get; set; }

    }


	    public class HekmatEffectlessCanceledTransactionEditModel:IBaseViewModel
        {
	        public int ViewModelId
	            {
	                get => Id;
	                set => Id = value;
	            }
				public int Id { get; set; }		
				[StringLength(32)][StringValidation] public string CartNumber { get; set; }		
				public int? TerminalNumber { get; set; }		
				public int? TerminalRotationRow { get; set; }		
				[StringLength(50)] [StringValidation] public string TransactionDate { get; set; }		
				public Int64? ModifiedTransactionPrice { get; set; }		
				[StringLength(200)][StringValidation] public string StoreName { get; set; }		
				public int? RefundDetailId { get; set; }		
}

  



public class HekmatEffectlessCanceledTransactionSimpleViewModel:IBaseViewModel
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
