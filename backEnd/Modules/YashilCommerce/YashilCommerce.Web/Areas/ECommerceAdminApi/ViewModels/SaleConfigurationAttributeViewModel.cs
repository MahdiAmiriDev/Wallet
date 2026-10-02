using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class SaleConfigurationAttributeListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        [StringValidation] public string Title { get; set; }
        /// <summary>
        /// نوع تخفیف
        /// </summary>
        [StringValidation] public string DiscountTypeTitle { get; set; }
        /// <summary>
        /// درصد تخفیف
        /// </summary>
        public decimal DiscountPercentage { get; set; }
        /// <summary>
        /// مقدار تخفیف
        /// </summary>
        public decimal DiscountAmount { get; set; }
        /// <summary>
        /// درصد سود 
        /// </summary>
        public decimal ProfitPercentage { get; set; }
        /// <summary>
        /// درصد سایر کسورات
        /// </summary>
        public decimal? OtherDeductionsPercentage { get; set; }

        /// <summary>
        /// اختصاص به زیر دسته
        /// </summary>
        public bool AppliedToSubCategories { get; set; }
        /// <summary>
        /// عنوان تنظیم فروش
        /// </summary>
        [StringValidation] public string SaleConfigurationTitle { get; set; }
        /// <summary>
        /// عنوان باشگاه مشتریان
        /// </summary>
        public string CustomerClubTitle { get; set; }
        /// <summary>
        ///  مرکز
        /// </summary>
        [StringValidation] public string CompanyTitle { get; set; }
        /// <summary>
        /// نمایندگی
        /// </summary>
        [StringValidation] public string RepresentationTitle { get; set; }
        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public DateTime? EndDate { get; set; }

        public decimal WagePercentage { get; set; }
        /// <summary>
        /// نوع فروش
        /// </summary>
        public string SaleTypeTitle { get; set; }
        /// <summary>
        /// فعال بودن
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        ///         /// محاسبه اقساط بر اساس فرمول
        ///</summary>
        public bool ExpressionBasedInstallmentCount { get; set; }
        /// <summary>
        ///         /// تعداد اقساط
        ///</summary>
        public int InstallmentCount { get; set; }
        /// <summary>
        ///         /// تعداد اقساط جهت تسویه فاکتور
        ///</summary>
        public int FactorInstallmentCount { get; set; }

    }


    public class SaleConfigurationAttributeEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        [StringLength(600)] [StringValidation]
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// درصد تخفیف
        /// </summary>
        //[Range(0, int.MaxValue)]
        //[Required] public int DiscountType { get; set; }
        /// <summary>
        /// آیا تخفیف درصدی باشد
        /// </summary>
        public bool UsePercentage { get; set; }
        /// <summary>
        /// درصد تخفیف
        /// </summary>
        [Required] public decimal DiscountPercentage { get; set; }
        /// <summary>
        /// مقدار تخفیف
        /// </summary>
         public decimal DiscountAmount { get; set; }
        /// <summary>
        /// حداکثر مقدار تخفیف
        /// </summary>
        public decimal? MaximumDiscountAmount { get; set; }
        /// <summary>
        /// درصد سود
        /// </summary>
        [Required] public decimal ProfitPercentage { get; set; }
        /// <summary>
        /// درصد سایر کسورات
        /// </summary>
        public decimal? OtherDeductionsPercentage { get; set; }
        /// <summary>
        /// نوع محدودیت تخفیف
        /// </summary>
        //[Range(0, int.MaxValue)]
        //[Required] public int DiscountLimitationType { get; set; }
        /// <summary>
        /// تعداد محدودیت تخفیف
        /// </summary>
        //[Range(0, int.MaxValue)]
        //[Required] public int LimitationTimes { get; set; }
        /// <summary>
        /// اختصاص به زیر دسته
        /// </summary>
        public bool AppliedToSubCategories { get; set; }
        /// <summary>
        /// توضضیح مدیر
        /// </summary>
        [StringValidation] public string AdminComment { get; set; }

        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
        /// <summary>
        ///  شناسه تنظیم فروش
        /// </summary>
        [Range(0, int.MaxValue)]
        [Required] public int SaleConfigurationId { get; set; }
        /// <summary>
        /// شناسه باشگاه مشتریان
        /// </summary>
        [Required] public int CustomerClubId { get; set; }

        [Required] public decimal WagePercentage { get; set; }

        /// <summary>
        /// نوع فروش
        /// </summary>
        public int SaleType { get; set; }
        /// <summary>
        /// فعال بودن
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        ///         /// تعداد اقساط
        ///</summary>
        public int InstallmentCount { get; set; }
        /// <summary>
        ///         /// محاسبه اقساط بر اساس فرمول
        ///</summary>
        public bool ExpressionBasedInstallmentCount { get; set; }
        /// <summary>
        ///         /// فرمول محاسبه قسط
        ///</summary>
        public string InstallmentExpression { get; set; }
        /// <summary>
        ///         /// اجازه تعیین اقساط در تعریف محصول
        ///</summary>
        public bool AllowSetInstallmentInProduct { get; set; }
        /// <summary>
        /// تعداد اقساط جهت تسویه فاکتور
        /// </summary>
        [Required]
        [Range(1, 60)]
        public int FactorInstallmentCount { get; set; }

    }

    public class SaleConfigurationAttributeSimpleViewModel : IBaseViewModel
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
