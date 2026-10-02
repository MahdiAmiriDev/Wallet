using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class TaxCategoryListViewModel : IBaseViewModel
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

        public decimal Percentage { get; set; }

        public bool AllowDelete { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }
        /// <summary>
        ///         /// شامل بیمه است
        ///</summary>
        public bool IsIncludeInsurance { get; set; }

    }


    public class TaxCategoryEditModel : IBaseViewModel
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

        [Required]
        public decimal Percentage { get; set; }
        [StringValidation] public string Description { get; set; }
        public bool AllowDelete { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int AccessLevelId { get; set; }
        [StringLength(300)][StringValidation] public string TitleForConvert { get; set; }
        /// <summary>
        /// واحد سنجش
        /// </summary>
        [StringLength(150)] [StringValidation]
        public string UnitOfMeasurement { get; set; }
        /// <summary>
        ///         /// شامل بیمه است
        ///</summary>
        public bool IsIncludeInsurance { get; set; }
    }
    public class TaxCategorySimpleViewModel : IBaseViewModel
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
    /// از این ویودل برای دریافت نام کالا کد کالا و واحد سنجش مربوط به فاکتور و کانورت در تنظیم فروش استفاده میشود
    /// در واقع مقادیر پیش فرض از دسته بندی مالیاتی خوانمده میشود و در تنظیمات فروش نشان و ذخیره میشود
    /// کاربر میتواند این مقدایر را تغییر دهد
    /// </summary>
    public class TaxCategoryDefaultValuesViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        /// <summary>
        /// کد دسته مالیاتی
        /// بعنوان کد کالا در کانورت در صورتی که کانورت بر اساس دسته مالیاتی باشد در نظر گرفته میشود
        /// </summary>
        [StringValidation] public string Code { get; set; }

        /// <summary>
        /// در صد مالیات
        /// </summary>
        public decimal Percentage { get; set; }
        /// <summary>
        /// عنوان برای کانورت
        /// بعنوان نام کالا در کانورت در صورتی که کانورت بر اساس دسته بندی مالیاتی در نظر گرفته میشود
        /// </summary>
        [StringValidation] public string TitleForConvert { get; set; }
        /// <summary>
        /// واحد سنجش
        /// بعنوان واجد سنجش در کانورت در صورتی که کانورت بر اساس دسته بندی مالیاتی باشد در نظر گرفته میشود
        /// </summary>
        [StringValidation] public string UnitOfMeasureMent { get; set; }

    }
}
