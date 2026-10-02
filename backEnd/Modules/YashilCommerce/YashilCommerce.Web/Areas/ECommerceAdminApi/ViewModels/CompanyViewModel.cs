using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class CompanyListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        public int? ParentId { get; set; }
        [StringValidation] public string ParentTitle { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string Title { get; set; }

        // [StringValidation] public string Address { get; set; }

        public int? EstablishedLicenseType { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }
        [StringValidation] public string EstablishedLicenseTypeTitle { get; set; }
        public bool ShowInMainPage { get; set; }
        [StringValidation] public string ShortName { get; set; }
        /// <summary>
        /// فعال
        ///</summary>
        public bool? IsActive { get; set; }
        /// <summary>
        /// تایید شده
        ///</summary>
        public bool Confirmed { get; set; }
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int DisplayOrder { get; set; }

    }

    public class CompanyEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        public int? ParentId { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }

        [StringLength(600)]
        [Required]
        [StringValidation]

        public string Title { get; set; }
        public int? EstablishedLicenseType { get; set; }

        [Range(0, int.MaxValue)]
        [Required]

        public int AccessLevelId { get; set; }
        public AddressEditModel Address { get; set; }
        public bool ShowInMainPage { get; set; }
        [StringValidation] public string ShortName { get; set; }
        /// <summary>
        /// فعال
        ///</summary>
        public bool? IsActive { get; set; }
        /// <summary>
        /// تایید شده
        ///</summary>
        public bool Confirmed { get; set; }
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        [Range(0, int.MaxValue)]
        [Required]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// شناسه دسته بندی گروه مرکز
        /// </summary>
        public int? CompanyGroupId { get; set; }
    }

    public class CompanySimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [StringValidation] public string Title { get; set; }
    }

    public class CompanyCustomSimpleViewModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }
        [StringValidation] public string ShortName { get; set; }
        public int ApplicationId { get; set; }
        [StringValidation] public string ApplicationUrl { get; set; }
    }

    public class UpdateCanUserSeeOtherCompanyEditModel
    {
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// می تواند کاربر سایر مراکز را ببیند؟
        /// </summary>
        public bool CanUserSeeOtherCompany { get; set; }

    }
    /// <summary>
    /// ویو مدل نمایش یک مرکز به همراه نسل آن مرکز
    /// برای نمایش در لیست انتخابی مراکز
    /// </summary>
    public class CompanyWithChildrenForSelectViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// شناسه پدر
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// در صورتی که در خروجی کویری
        /// parent Id
        /// دارای مقدار باشد اما به عنوان بالاترین رکورد در حالت درختی باشد
        /// برای این که نمایش درختی در سمت کلاینت را بتوان درست نمایش داد
        /// برای رکورد بالای نمودار درختی مقدار این پروپرتی برابر با
        /// Null
        /// قرار می گیرد
        /// </summary>
        public int? HeadId { get; set; }
        [StringValidation] public string Title { get; set; }
    }

    /// <summary>
    /// ویو مدل نمایش یک مرکز به همراه نسل آن مرکز
    /// برای نمایش در لیست مراکز
    /// </summary>
    public class CompanyWithChildrenForListViewModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }
        public string ParentTitle { get; set; }
        /// <summary>
        /// عنوان مرکز
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// فعال
        ///</summary>
        public bool? IsActive { get; set; }
        /// <summary>
        /// تایید شده
        ///</summary>
        public bool Confirmed { get; set; }
        /// <summary>
        /// نمایش دکمه اطلاعات مرکز
        /// </summary>
        public bool ShowCompanyInformationButton { get; set; }
    }

}
