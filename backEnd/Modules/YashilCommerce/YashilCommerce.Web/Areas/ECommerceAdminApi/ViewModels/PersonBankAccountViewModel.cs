using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class PersonBankAccountListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }
        [StringValidation] public string BranchName { get; set; }
        [StringValidation] public string PersonTitle { get; set; }
        [StringValidation] public string NationalCode { get; set; }

        [StringValidation] public string BankTypeTitle { get; set; }

        [StringValidation] public string CardNumber { get; set; }

        [StringValidation] public string AccountNumber { get; set; }

        [StringValidation] public string ShebaNumber { get; set; }
        [StringValidation] public string? MobileNumber { get; set; }
    }

    public class PersonBankAccountEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string BranchName { get; set; }
        
        [Range(0, int.MaxValue)]
        public int? PersonId { get; set; }

        [Range(0, int.MaxValue)]
        [Required] 
        public int BankType { get; set; }
        
        [StringLength(16)] 
        [StringValidation]
        public string CardNumber { get; set; }
        [StringLength(50)] [Required] [StringValidation] public string AccountNumber { get; set; }
        [StringLength(26)] [StringValidation] public string ShebaNumber { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
        /// <summary>
        /// نام 
        /// </summary>
        [StringValidation] public string Name { get; set; }
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        [StringValidation] public string LastName { get; set; }

        [StringValidation] public string NationalCode { get; set; }
        [StringValidation] public string? MobileNumber { get; set; }
    }





    public class PersonBankAccountSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        /// <summary>
        /// نام و نام خانوادگی صاحب حساب
        /// </summary>
        [StringValidation] public string FullName { get; set; }
      
        /// <summary>
        /// شماره حساب
        /// </summary>
        [StringValidation] public string AccountNumber { get; set; }
        /// <summary>
        /// نام بانک
        /// </summary>
        [StringValidation] public string BankTypeTitle { get; set; }
    }

}
