using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomViewModels;
using Yashil.Common.SharedKernel.Helpers;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class RecipientListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        /// <summary>
        /// شناسه رمز شده
        /// </summary>
        public string EncryptedId => CryptographyHelper.AesEncrypt(CryptographyHelper.Base64Encode(Id.ToString()));

        [StringValidation] public string Title { get; set; }

        [StringValidation] public string AddressTitle { get; set; }

        [StringValidation] public string FullAddress { get; set; }

        [StringValidation] public string NationalCode { get; set; }

        [StringValidation] public string CardNumber { get; set; }

        public int? CardType { get; set; }

        [StringValidation] public string MembershipTypeTitle { get; set; }
        public int MembershipType { get; set; }
        [StringValidation] public string CardTypeTitle { get; set; }

        [StringValidation] public string CityTitle { get; set; }

        public int? CityId { get; set; }
        /// <summary>
        /// شماره موبایل
        /// </summary>
       

        public string Name { get; set; }
        public string LastName { get; set; }
        public bool? IsEncryptedAddress { get; set; }
        [StringValidation] public string MobileNumber { get; set; }
    }


    public class RecipientEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringLength(300)] [StringValidation] public string Code { get; set; }

        [StringLength(600)] [StringValidation] public string Title { get; set; }

        [Range(0, int.MaxValue)]
        public int? AddressId { get; set; }

        [StringValidation] public string Description { get; set; }
       [StringLength(10)][StringValidation]
        [DisplayName("کدملی متقاضی")]
        [Required(ErrorMessage = @"لطفا کدملی را وارد نمایید")]
        [Remote(action: "VerifyNationalCode", controller: "User")]
       public string NationalCode { get; set; }
        [MaxLength(16)]
        [DisplayName("شماره کارت")]
        [Remote(action: "VerifyCardNumber", controller: "Recipient", AdditionalFields = "MembershipType", ErrorMessage = "این فیلد اجباری است")]
        [StringValidation] public string CardNumber { get; set; }

        [StringValidation] public string CardType { get; set; }

        [DisplayName("وضعیت")]
        [Required(ErrorMessage = @"وضعیت اجباری می باشد")]
        public int MembershipType { get; set; }

        public AddressEditModel Address { get; set; }

        /// <summary>
        /// is Application Subsidy Based or nor
        /// </summary>
        public bool SubsidyBased { get; set; }
        public bool HekmatBased { get; set; }
        /// <summary>
        /// is Application address Based or nor
        /// </summary>
        public bool AddressBased { get; set; }

        /// <summary>
        /// اجباری یا اختیاری بودن شماره کارت در وضعیت سایر
        /// </summary>
        public bool RequiredRecipientCardNumber { get; set; }
        /// <summary>
        /// نمایش شماره کارت  
        /// </summary>
        public bool ShowRecipientCardNumber { get; set; }
        /// <summary>
        /// شماره موبایل  تحویل گیرنده
        /// </summary>
        [StringLength(11)]
        [StringValidation]
        [DisplayName("شماره موبایل")]
        [Required(ErrorMessage = @"لطفا شماره موبایل را وارد نمایید")]
        public string MobileNumber { get; set; }
        [StringLength(200)]
        [DisplayName("نام تحویل گیرنده")]
        public string Name { get; set; }
        
        [StringLength(400)]
        [DisplayName("نام خانوادگی تحویل گیرنده")]
        public string LastName { get; set; }

        [DisplayName("[جنسیت]")]
        [Required(ErrorMessage = @"جنسیت اجباری می باشد")]
        public int GenderType { get; set; }

        [DisplayName("تاریخ تولد")]
        public int? BirthDate { get; set; }

        [StringLength(8)]
        [StringValidation]
        [DisplayName("تاریخ تولد")]
        [Required(ErrorMessage = @"لطفا تاریخ تولد را وارد نمایید")]
        public string BirthDateStr { get; set; }
    }

    public class RecipientSimpleViewModel : IBaseViewModel
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
