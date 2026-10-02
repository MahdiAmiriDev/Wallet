using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Attributes;
using Yashil.Core.Entities;

namespace Yashil.Core.CustomViewModels.UserManagement
{

    public class UserFullNameSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }
    }


    public class UserSelectSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public bool NeedToSendSmsForAuthentication { get; set; }
    }
    [ListViewModel(nameof(User))]
    public class UserListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        public int PersonId { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NationalCode { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public string MobileNumber { get; set; }

        public string OrganizationTitle { get; set; }

        public string Address { get; set; }

        public string AccessLevelTitle { get; set; }

        public string FullName
        {
            get
            {
                return this.FirstName + ' ' + this.LastName;
            }
        }
        /// <summary>
        /// نیاز به ارسال پیامک برای احراز هویت
        /// </summary>
        public bool NeedToSendSmsForAuthentication { get; set; }
    }


    /// <summary>
    /// برای ثبت نام کاربران در فرم مدیریت سایت(Backend)
    /// </summary>
    public class UserEditModel : BaseUserEditModel
    {
        [DisplayName("موبایل/نام کاربری")]
        [Required(ErrorMessage = @"شماره موبایل/نام کاربری را وارد نمایید")]
        [Remote(action: "CheckExistsUserName", controller: "User")]
        public string UserName { get; set; }
        [StringLength(300)]
        [StringValidation]
        public string Email { get; set; }
        public string PasswordStr { get; set; }
        [StringLength(10)]
        [StringValidation]
        [DisplayName("کدملی")]
        [Required(ErrorMessage = @"کدملی اجباری است")]
        [Remote(action: "CheckPersonNationalCode", controller: "Person")]
        public string NationalCode { get; set; }
        /// <summary>
        /// نیاز به ارسال پیامک برای احراز هویت
        /// </summary>
        public bool NeedToSendSmsForAuthentication { get; set; }
        /// <summary>
        /// گروه کاربری
        /// </summary>
        public int UserGroupId { get; set; }

        public string Key { get; set; }
    }

    /// <summary>
    /// this editmodel used for registration user Page and extend from user Edit Model
    /// </summary>



    public class UserSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        public string Title { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string Email { get; set; }
        public string mobileNumber { get; set; }
    }

    public class OnlineUserListViewModel
    {

        public int Id { get; set; }

        public string UserName { get; set; }

    }
    /// <summary>
    /// ویو مدل مربوط به تغییر رمز عبور کاربر در پنل ادمین
    /// </summary>
    public class UserChangePasswordEditModel
    {
        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// کلمه عبور
        /// </summary>
        public string PasswordStr { get; set; }
        /// <summary>
        /// تکرار کلمه عبور
        /// </summary>
        public string RepeatPasswordStr { get; set; }
        /// <summary>
        /// کلید دیکریپت کردن رمز عبور
        /// </summary>
        public string Key { get; set; }

    }

    public class UpdateUserPrivateKeyParam
    {
        public int Id { get; set; }
        /// <summary>
        /// کلید خصوصی کاربر
        /// </summary>
        public string PrivateKey { get; set; }
    }

    public class UserPrivateKeyViewModel
    {
        /// <summary>
        /// کلید خصوصی کاربر
        /// </summary>
        public string PrivateKey { get; set; }
    }
    /// <summary>
    /// مدل به روز رسانی شخص کاربر
    /// </summary>
    public class UserPersonEditModel
    {
        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// شناسه شخص
        /// </summary>
        public int PersonId { get; set; }
    }

    /// <summary>
    /// مدل مربوط به ویرایش اطلاعات مربوط به شخص توسط کاربر جاری در سیستم
    /// </summary>
    public class EditPersonByUserEditModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string MobileNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }

    public class UserSimpleListViewModel
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// نام کاربری
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// کدملی
        /// </summary>
        public string NationalCode { get; set; }
        /// <summary>
        /// ایمیل
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// آیا فعال است
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// نیاز به ارسال پیامک برای احراز هویت
        /// </summary>
        public bool NeedToSendSmsForAuthentication { get; set; }
        /// <summary>
        /// سازمان
        /// </summary>
        public string CompanyTitle { get; set; }
    }

    public class UserSimpleListWithStudentRoleViewModel
    {

        /// <summary>
        /// شناسه کاربری دانشجو به دلیل این که یک پروپرتی با نام 
        /// Id
        /// در سمت پنل ادمین برای 
        /// devx
        /// نیاز است باید همنین مقداری داشته باشیم تا با خطای سمت کاربر مواجه نشویم
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// برابر مقدار UserId
        /// </summary>
        public int StudentId { get; set; }
        /// <summary>
        //  نام و نام خانوادگی 
        /// </summary>
        public string StudentTitle { get; set; }
        /// <summary>
        /// نام کاربری 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// نام مرکز
        /// </summary>
        public string CompanyTitle { get; set; }
        /// <summary>
        /// نام نمایندگی
        /// </summary>
        public string RepresentationTitle { get; set; }

    }
}
