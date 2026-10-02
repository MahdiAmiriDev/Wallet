using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class CompanyMainCategoryListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }

        [StringValidation] public string MainCategoryTitle { get; set; }

        public int DisplayOrder { get; set; }
    }


    public class CompanyMainCategoryEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [Range(0, int.MaxValue)][Required] public int CompanyMainCategory { get; set; }
        [Range(0, int.MaxValue)][Required] public int MainCategoryId { get; set; }
        [Range(0, int.MaxValue)][Required] public int DisplayOrder { get; set; }
        [StringValidation] public string Description { get; set; }
    }

    public class CompanyMainCategoryCustomEditModelItem
    {
        public CompanyMainCategoryCustomEditModelItem(){ }
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [Range(0, int.MaxValue)][Required] public int DisplayOrder { get; set; }
    }
    public class CompanyMainCategoryCustomEditModel
    {
        public List<string> ChangedProps { get; set; }
        public CompanyMainCategoryCustomEditModelItem EditModel { get; set; }
    }

    public class CompanyMainCategorySimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        public int MainCategoryId { get; set; }
        public int CompanyMainCategory { get; set; }
        public int DisplayOrder { get; set; }
        [StringValidation] public string Title { get; set; }
    }
}