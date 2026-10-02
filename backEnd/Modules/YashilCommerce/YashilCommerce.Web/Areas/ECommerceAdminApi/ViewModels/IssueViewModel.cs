using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class IssueListViewModel : IBaseViewModel
    {
       
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

        [StringValidation] public string OrderItemTitle { get; set; }

        public int? IssueOwner { get; set; }
        /// <summary>
        /// شماره موبایل کاربری که در سایت عضو نیست
        /// </summary>
        [StringValidation] public string NonRegisteredUser { get; set; }

        [StringValidation] public string IssueTypeTitle { get; set; }

        [StringValidation] public string ResolutionTypeTitle { get; set; }

        [StringValidation] public string PriorityTypeTitle { get; set; }

        public int? Estimate { get; set; }

        public int? DueDate { get; set; }

        public int? ResolvedDate { get; set; }

        [StringValidation] public string IssueStateTypeTitle { get; set; }

        [StringValidation] public string AssigneeTitle { get; set; }
        public int Assignee { get; set; }
        [StringValidation] public string AssigneeLastName { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }
        public DateTime CreationDateTime { get; set; }
      
    }


    public class IssueEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(600)] [StringValidation]
        [Required]
        public string Title { get; set; }
        public int? OrderItemId { get; set; }
        [Range(0, int.MaxValue)]
        public int? IssueOwner { get; set; }

        [StringLength(50)] [StringValidation] public string NonRegisteredUser { get; set; }

        [Range(0, int.MaxValue)]
        [Required] public int IssueType { get; set; }

        [StringValidation] public string Description { get; set; }
        [StringValidation] public string Solution { get; set; }
        [Range(0, int.MaxValue)]
        public int? ResolutionType { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int PriorityType { get; set; }
        public int? Estimate { get; set; }
        public int? DueDate { get; set; }
        public int? ResolvedDate { get; set; }
        [Range(0, int.MaxValue)]
        public int? IssueStateType { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int Assignee { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
    }





    public class IssueSimpleViewModel : IBaseViewModel
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
