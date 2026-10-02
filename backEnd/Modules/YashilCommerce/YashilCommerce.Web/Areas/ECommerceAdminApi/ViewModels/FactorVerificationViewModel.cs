using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class FactorVerificationListViewModel : IBaseViewModel
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
        public int FactorId { get; set; }

        [StringValidation] public string FactorTitle { get; set; }
        /// <summary>
        /// نام و نام خانوادگی تایید یا رد کننده بررسی فاکتور
        /// </summary>
        [StringValidation] public string VerifierFullName { get; set; }
        [StringValidation] public string FactorCode { get; set; }
        /// <summary>
        /// تاریخ صدور فاکتور
        /// </summary>
        public DateTime FactorDate { get; set; }
        [StringValidation] public string CompanyTitle { get; set; }
        [StringValidation] public string RepresentationTitle { get; set; }

        [StringValidation] public string FactorConfirmationTypeTitle { get; set; }
        public int FactorConfirmationType { get; set; }

        [StringValidation] public string FactorVerificationConfigurationRoleTitle { get; set; }
        public int VerificationOrder { get; set; }
        [StringValidation] public string VerificationOrderTitle { get; set; }
        [StringValidation] public string VerificationName { get; set; }
        public decimal ProfitPercentage { get; set; }
        public decimal DiscountPercentage { get; set; }
        [StringValidation] public string AccessLevelTitle { get; set; }
        /// <summary>
        /// دسته بندی مالیاتی فاکتور
        /// </summary>
        [StringValidation] public string TaxCategoryTitle { get; set; }
        [StringValidation] public string SaleConfigurationTitle { get; set; }

        public DateTime? ValidationDate { get; set; }
        /// <summary>
        /// شماره گردشی که فاکتور رد شده و جهت بررسی مجددا ارسال شده است
        /// </summary>
        public int TryNumber { get; set; }
        /// <summary>
        /// توضیحات رد فاکتور
        /// </summary>
        [StringValidation] public string Description { get; set; }
        /// <summary>
        /// تاریخی که فاکتور جهت بررسی ارسال شده است
        /// </summary>
        public DateTime CreationDate { get; set; }
    }

    /// <summary>
    /// برای نمایش لیست توضیحات بررسی های فاکتور
    /// </summary>
    public class FactorVerificationDescriptionListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        /// <summary>
        /// نام و نام خانوادگی بررسی کننده فاکتور
        /// </summary>
        [StringValidation] public string VerifierFullName { get; set; }

        public int FactorId { get; set; }

        [StringValidation] public string Description { get; set; }
    }
    public class FactorVerificationEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(600)] [StringValidation] public string Title { get; set; }
        [Range(0, int.MaxValue)] [Required] public int FactorId { get; set; }
        [Range(0, int.MaxValue)] [Required] public int FactorConfirmationType { get; set; }
        [Range(0, int.MaxValue)] [Required] public int FactorVerificationConfigurationRoleId { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)] [Required] public int AccessLevelId { get; set; }

        public int VerificationOrder { get; set; }
    }


    public class FactorVerificationSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
    }


    /// <summary>
    /// برای رد فاکتور مورد استفاده میباشد
    /// زیرا در رد بررسی فاکتور تنها توضیحات را از کاربر دریافت میکند
    /// </summary>
    public class CustomFactorVerificationSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

        [StringValidation] public string Description { get; set; }
    }
}