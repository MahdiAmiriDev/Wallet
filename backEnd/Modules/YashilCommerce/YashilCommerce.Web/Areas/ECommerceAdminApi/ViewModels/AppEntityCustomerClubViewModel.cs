			using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;
namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

   public class AppEntityCustomerClubListViewModel:IBaseViewModel
    {
		public int ViewModelId
	        {
	            get => Id;
	            set => Id = value;
	        }
		public int? ParentId { get; set; }
        public int Id { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public Int64 ObjectId { get; set; }

        public string AppEntityTitle { get; set; }

        public string CustomerclubTitle { get; set; }

        public string AccessLevelTitle { get; set; }

    }


	    public class AppEntityCustomerClubEditModel:IBaseViewModel
        {
	        public int ViewModelId
	            {
	                get => Id;
	                set => Id = value;
	            }
				public int Id { get; set; }		
				[StringLength(300)]				public string Code { get; set; }		
				[StringLength(600)] 
				[Required]				public string Title { get; set; }		
 
				[Required]				public Int64 ObjectId { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int AppEntityId { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int CustomerclubId { get; set; }		
				public string Description { get; set; }		
				[Range(0,int.MaxValue)] 
				[Required]				public int AccessLevelId { get; set; }		
}

  



public class AppEntityCustomerClubSimpleViewModel:IBaseViewModel
        {
	        public int ViewModelId
	            {
	                get => Id;
	                set => Id = value;
	            }
        public int Id { get; set; }

			        public string Title { get; set; }

			    }

}      
