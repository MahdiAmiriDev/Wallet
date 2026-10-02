using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
     public class CustomerClubListViewModel : IBaseViewModel
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

        public bool PayWithWallet { get; set; }

        public bool IpgBased { get; set; }

        public bool ServiceBased { get; set; }

        public bool IsInstallmentsPaid { get; set; }

        public bool IsLocalClub { get; set; }

        public bool SupportSubsidy { get; set; }
        /// <summary>
        /// ارسال کد خاص باشگاه
        ///</summary>
        public bool HasCustomCode { get; set; }
        /// <summary>
        /// ارسال کد ملی
        ///</summary>
        public bool HasNationalCode { get; set; }
        /// <summary>
        /// ارسال شماره موبایل
        ///</summary>
        public bool HasMobileNumber { get; set; }
       
        /// <summary>
        /// نوع مدل ثبت نام
        /// </summary>
        public string RegistrationModeTypeTitle { get; set; }
        [StringValidation] public string CardNumberLabel { get; set; }
    }
    public class CustomerClubEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(600)] [StringValidation]
        [Required]
        public string Title { get; set; }
        [StringValidation] public string Description { get; set; }
        public bool PayWithWallet { get; set; }
        public bool IpgBased { get; set; }
        public bool ServiceBased { get; set; }
        public bool IsInstallmentsPaid { get; set; }
        public bool IsLocalClub { get; set; }
        [StringLength(600)] [StringValidation] public string ServiceUrl { get; set; }
        [StringLength(600)] [StringValidation] public string IpgPageUrl { get; set; }
        [StringLength(200)][StringValidation] public string UserName { get; set; }
        [StringLength(600)] [StringValidation] public string Password { get; set; }
        public bool SupportSubsidy { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
        
        /// <summary>
        /// نوع مدل ثبت نام
        /// </summary>
        public int RegistrationModeType { get; set; }
    }
    public class CustomerClubSimpleViewModel : IBaseViewModel
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
