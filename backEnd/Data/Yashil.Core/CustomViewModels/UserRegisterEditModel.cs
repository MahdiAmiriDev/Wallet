using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Dtos;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.CustomViewModels
{
    /// <summary>
    /// برای ثبت نام کاربران در سایت فروش گاهی
    /// </summary>
    public class UserRegisterEditModel : BaseUserEditModel, ICustomDntCaptchaBase
    {
        public List<KeyValue> KeyValues { get; set; }
        [DisplayName("شماره موبایل")]
        [Required(ErrorMessage = @"لطفا شماره موبایل/نام کاربری را وارد نمایید")]
        [Remote(action: "CheckExistsUserName", controller: "User")]
        [RegularExpression(@"^09[0-9]{9}$", ErrorMessage = "لطفا شماره موبایل/ نام کاربری صحیح را وارد نمایید")]
        [StringValidation] public string UserName { get; set; }
        [StringLength(300)][StringValidation]
        [DisplayName("ایمیل")]
        [EmailAddress(ErrorMessage = @"فرمت ایمیل صحیح نیست.")]
        //[Required(ErrorMessage = @"ایمیل خود را وارد نمایید")]
        [Remote(action: "CheckExistsEmail", controller: "User")]
        public string Email { get; set; }

        [DisplayName("رمز عبور دلخواه")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = @"رمز عبور خود را وارد نمایید")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{1,}$",ErrorMessage = "کلمه عبور باید حداقل شامل یک حرف بزرگ، یک حرف کوچک و عدد باشد")]
        //[PasswordPolicy]
        [StringValidation] public string PasswordStr { get; set; }

        [DisplayName("تکرار رمز عبور دلخواه")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = @"لطفا رمز عبور خود را تکرار نمایید")]
        [StringValidation] public string PasswordStrConfirm { get; set; }
        /// <summary>
        /// این پارامتر فیلدهای ریجیستر را تعیین میکند
        /// </summary>
        public int RegistrationMode { get; set; }
        /// <summary>
        /// اگر برابر یک بود باید ایمیل را از کاربر دریافت کند
        /// </summary>
        public int PasswordResetRecoveryMode { get; set; }

        public bool AddressBased { get; set; }
        [StringValidation] public string DNTCaptchaText { get; set; }
        [StringValidation] public string DNTCaptchaToken { get; set; }
        /// <summary>
        /// متن ورودی کپچا
        /// </summary>
        /// <returns></returns>
        [DisplayName("کد امنیتی")]
        [Required(ErrorMessage = "لطفا کد امنیتی را وارد کنید")]
        [StringValidation] public string DNTCaptchaInputText { get; set; }
        /// <summary>
        /// شماره موبایل رمز شده برای چک کردن بعد از رجیستر
        /// </summary>
        [StringValidation] public string EncryptedMobileNumber { get; set; }
        /// <summary>
        /// در صورت ثبت نام موفق به کدام صفحه برود
        /// </summary>
        [StringValidation] public string BackUrl { get; set; }

        [StringValidation] public string Key { get; set; }
    }
    /// <summary>
    /// کلاس پایه ویرایش کاربران
    /// </summary>
    public class BaseUserEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public Dictionary<string, string> AdditionalInfoList { get; set; }

        public int Id { get; set; }

        [StringLength(200)][StringValidation]
        [DisplayName("نام")]
        public string Name { get; set; }

        [StringLength(400)][StringValidation]
        [DisplayName("نام خانوادگی")]
        public string LastName { get; set; }

        [StringLength(200)][StringValidation]
        [DisplayName(" نام پدر")]
        public string FatherName { get; set; }

        public int? Gender { get; set; }

        public int? BirthDate { get; set; }

        [DisplayName("شماره تلفن")]
        [StringValidation] public string PhoneNumber { get; set; }

        public int? EducationGrade { get; set; }

        public bool? IsActive { get; set; }

        [StringLength(20)]
        [DisplayName("موبایل")]
        [StringValidation] public string MobileNumber { get; set; }

       [StringLength(10)]
       [DisplayName("کد پستی")]
        [StringValidation] public string PostalCode { get; set; }

        public int? OrganizationId { get; set; }

        [StringLength(400)]
        [DisplayName("آدرس")]
        [StringValidation] public string Address { get; set; }

        public int AccessLevelId { get; set; }

        public int? CityId { get; set; }
        /// <summary>
        /// شماره شناسنامه
        /// </summary>
        public int? IdentityNumber { get; set; }
        /// <summary>
        /// شماره داشنجویی
        /// </summary>
        [StringValidation] public string StudentNumber { get; set; }
        /// <summary>
        /// آدرس کامل کاربر
        /// </summary>
        [DisplayName("آدرس")]
        [StringValidation] public string FullAddress { get; set; }
        /// <summary>
        /// آیا اجازه ویرایش شحص را دارد
        /// </summary>
        public bool AllowEditPerson { get; set; }
        /// <summary>
        /// کاربری که شخص را ایجاد کرده است
        /// </summary>
        public int? PersonCreatedBy { get; set; }

        [StringValidation] public string Key { get; set; }
    }
}
