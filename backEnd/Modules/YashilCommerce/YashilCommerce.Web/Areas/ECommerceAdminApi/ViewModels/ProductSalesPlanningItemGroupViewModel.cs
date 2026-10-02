using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;
namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class ProductSalesPlanningItemGroupListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        public string ParentTitle { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string ItemGroupTitle { get; set; }

        public string ProductSalesPlanningTitle { get; set; }

        public string CodePath { get; set; }

        public string AccessLevelTitle { get; set; }

    }


    public class ProductSalesPlanningItemGroupEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        public int? ParentId { get; set; }
        [StringLength(300)] public string Code { get; set; }
        [StringLength(600)]
        [Required] public string Title { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int ItemGroupId { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int ProductSalesPlanningId { get; set; }
        public string Description { get; set; }
        [StringLength(800)] public string CodePath { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
    }
    /// <summary>
    /// این ویو برای نمایش آیتم گروه های اساین شده به برنامه ریزی دوره استفاده می شود
    /// </summary>
    public class ProductSalesPlanningItemGroupSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        public int ParentId { get; set; }

        /// <summary>
        /// عنوان آیتم گروه
        /// </summary>
        public string ItemGroupTitle { get; set; }
        /// <summary>
        /// عنوان گروه پدر
        /// </summary>
        public string ParentTitle { get; set; }

        /// <summary>
        /// شناسه ItemGroup
        /// </summary>
        public int ItemGroupId { get; set; }

        public string ItemGroupTitleWitParent { get; set; }
    }
    /// <summary>
    /// این ویو مدل برای اساین کردن برنامه ریزی دوره به آیتم گروه های انتخابی استفاده می شود
    /// </summary>
    public class AssignProductSalePlanningToItemEditModel
    {
        /// <summary>
        /// شناسه برنامه ریزی دوره
        /// </summary>
        public int ProductSalesPlanningId { get; set; }
        /// <summary>
        /// شناسه آیتم گروه های انتخابی
        /// </summary>
        public List<int> GroupsId { get; set; }

       
            
    }

    public class ItemGroupAssignedToProductSalesPlanningViewModel
    {
        /// <summary>
        /// شناسه آستم گروه
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// عنوان آیتم گروه
        /// </summary>
        [StringValidation] public string Title { get; set; }

    }

}
