using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class AdditionalUserPropListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string UserTitle { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }

        [StringValidation] public string RepresentationTitle { get; set; }
    }


    public class AdditionalUserPropEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [Range(0, int.MaxValue)][Required] public int UserId { get; set; }
        [Range(0, int.MaxValue)][Required] public int CompanyId { get; set; }
        public int? RepresentationId { get; set; }
        public int? RepresentationCategoryId { get; set; }
    }




    /// <summary>
    /// برای دریافت اطلاعات اضافی کاربر لاگین شده
    /// </summary>
    public class CurrentUserAdditionalPropViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int? RepresentationId { get; set; }
        public int? RepresentationCategoryId { get; set; }
        public int? CategoryId { get; set; }
    }

    /// <summary>
    /// ویومدل مربوط به اطلاعات کاربر
    /// </summary>
    public class AdditionalUserPropInfoViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int? RepresentationId { get; set; }
        public int? RepresentationCategoryId { get; set; }
        public int ApplicationId { get; set; }
        public int OrganizationId { get; set; }
        [StringValidation] public string CompanyTitle { get; set; }
        [StringValidation] public string RepresentationTitle { get; set; }
        [StringValidation] public string RepresentationCategoryTitle { get; set; }
        [StringValidation] public string UserName { get; set; }
        [StringValidation] public string FullName { get; set; }
        [StringValidation] public string ApplicationTitle { get; set; }
        [StringValidation] public string OrganizationTitle { get; set; }
    }


    public class AdditionalUserPropSimpleViewModel : IBaseViewModel
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
    /// لیست کاربران گروه بندی شده
    /// </summary>
    public class AdditionalUserPropCustomListViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// نام کاربری 
        /// </summary>
        [StringValidation] public string UserTitle { get; set; }
        /// <summary>
        /// مرکز کاربر
        /// </summary>
        public string CompanyTitle { get; set; }
        /// <summary>
        /// نمایندگی کاربر
        /// </summary>
        public string RepresentationTitle { get; set; }

    }
}