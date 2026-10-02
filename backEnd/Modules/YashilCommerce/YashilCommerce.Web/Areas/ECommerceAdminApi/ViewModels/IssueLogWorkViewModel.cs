using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class IssueLogWorkListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string Description { get; set; }

        [StringValidation] public string IssueTitle { get; set; }
        [StringValidation] public string IssueTypeTitle { get; set; }

        public int TimeSpent { get; set; }
        public int CreateBy { get; set; }
        [StringValidation] public string CreateByFullName { get; set; }
        public DateTime CreationDate { get; set; }

    }


    public class IssueLogWorkEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [Range(0, int.MaxValue)]
        public int? IssueId { get; set; }

        [Required] [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int TimeSpent { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
        [Required]
        public int CreateBy { get; set; }

        public int? OrderId { get; set; }
        public int? OrderItemId { get; set; }
        public int? PaymentId { get; set; }

        public int? IssueType { get; set; }
        /// <summary>
        /// متن سفارشی برای ایجاد issue
        /// </summary>
        [StringValidation] public string CustomIssueTitle { get; set; }

    


    }





    public class IssueLogWorkSimpleViewModel : IBaseViewModel
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
