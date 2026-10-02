using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class CategoryListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string ParentTitle { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string Title { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }

        public int DisplayOrder { get; set; }
        [StringValidation] public string AccessLevelTitle { get; set; }
        [StringValidation] public string ProductTypeTitle { get; set; }
        [StringValidation] public string CssClass { get; set; }
        /// <summary>
        /// درصد سقف یارانه وزارت برای این دسته که بر روی سقف یارانه وزارت هر پروژه اعمال میشود  
        /// </summary>
        public decimal VezaratSubsidyFactor { get; set; }
        /// <summary>
        /// درصد سقف یارانه ساتا برای این دسته که بر روی سقف یارانه ساتا هر پروژه اعمال میشود  
        /// </summary>
        public decimal SataSubsidyFactor { get; set; }
        /// <summary>
        ///         /// شامل بیمه است
        ///</summary>
        public bool IsIncludeInsurance { get; set; }
        /// <summary>
        /// شناسه گروه دسته بندی
        /// </summary>
        public int CategoryGroupId { get; set; }
        /// <summary>
        /// مرکز دسته بندی
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// اجازه ویرایش و حذف 
        /// </summary>
        public bool AllowEditAndDelete { get; set; }

    }
    public class CategoryEditModel : IBaseViewModel
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
        [StringValidation] public string Title { get; set; }
        public int? CompanyId { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int DisplayOrder { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int AccessLevelId { get; set; }
        public int? ProductTypeId { get; set; }
        [StringValidation] public string CssClass { get; set; }
        /// <summary>
        /// درصد سقف یارانه وزارت برای این دسته که بر روی سقف یارانه وزارت هر پروژه اعمال میشود  
        /// </summary>
        [Required]
        public decimal VezaratSubsidyFactor { get; set; }
        /// <summary>
        /// درصد سقف یارانه ساتا برای این دسته که بر روی سقف یارانه ساتا هر پروژه اعمال میشود  
        /// </summary>
        [Required]
        public decimal SataSubsidyFactor { get; set; }
        /// <summary>
        ///         /// شامل بیمه است
        ///</summary>
        public bool IsIncludeInsurance { get; set; }
        /// <summary>
        /// واحد سنجش
        /// </summary>
        [StringLength(150)]
        [StringValidation] public string UnitOfMeasurement { get; set; }
        /// <summary>
        /// منتشر شده
        /// </summary>
        public bool Published { get; set; }

        public int CategoryGroupId { get; set; }
        /// <summary>
        ///         /// جزییات تبلیغات سریع محصول های ابن دسته بندی
        ///</summary>
        public string ProductQuickPromoDetail { get; set; }
    }

    public class CategorySimpleViewModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }
        public int? DisplayOrder { get; set; }

        public string Title { get; set; }
        public string CssClass { get; set; }
        /// <summary>
        ///         زمان تغییر
        ///</summary>
        public string ModificationDate { get; set; }
    }
    /// <summary>
    /// استفاده برای نمایش دسته بندی های اصلی یک نمایندگی
    /// </summary>
    public class MainCategorySimpleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class CategoryRepresentationListViewModel
    {
        /// <summary>
        /// شناسه دسته بندی
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// شناسه دسته بندی
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// شناسه نمایندگی
        /// </summary>
        public int RepresentationId { get; set; }
        /// <summary>
        /// عنوان دسته بندی
        /// </summary>
        public string CategoryTitle { get; set; }
    }
    /// <summary>
    /// این ویومدل جهت دریافت دسته بندی ها برای انتخاب از طریق
    /// GetByCustomForSelect
    /// کاربرد دارد
    /// </summary>
    public class CategoryCustomViewModelForSelect
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }
        public int? DisplayOrder { get; set; }

        public string Title { get; set; }
        public string CssClass { get; set; }
        /// <summary>
        ///         زمان تغییر
        ///</summary>
        public string ModificationDate { get; set; }
        /// <summary>
        /// شناسه دسته بندی 
        /// </summary>
        public int CategoryGroupId { get; set; }
    }
}
