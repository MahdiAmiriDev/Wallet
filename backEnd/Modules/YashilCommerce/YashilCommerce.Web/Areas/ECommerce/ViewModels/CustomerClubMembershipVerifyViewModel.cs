using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class CustomerClubMembershipVerifyViewModel
    {
        [StringValidation] public string CardNumberLabel { get; set; }
        
        [Required(ErrorMessage = "لطفا شماره کارت را وارد نمایید")]
        [StringValidation] public string CardNumber { get; set; }
        public bool HasCustomCode { get; set; }
        public bool HasNationalCode { get; set; }
        public bool HasMobileNumber { get; set; }
        [StringValidation] public string Code { get; set; }

        [Remote(action: "VerifyNationalCode", controller: "User")]
        [DisplayName("کدملی")]
        [StringLength(maximumLength: 10, MinimumLength = 10,ErrorMessage = "کد ملی باید ده رقم باشد")]
        [Required(ErrorMessage = "کد ملی اجباری است")]
        [StringValidation] public string NationalCode { get; set; }
        
        [StringValidation] public string MobileNumber { get; set; }
    }
}
