using System;
using System.Collections.Generic;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    /// <summary>
    /// مدل مربوط به دریافت نتیجه ی جست جو
    /// براساس رشته کلید
    /// </summary>
    public class SearchByKeyViewModel
    {
        public List<ProductSalesPlanningSearchViewModel> SearchInProductSalesPlanningResult { get; set; }
        public List<CompanySearchViewModel> SearchInCompanyResult{ get; set; }
        public List<CategorySearchViewModel> SearchInCategoryResult { get; set; }

    }

    /// <summary>
    /// مدل جستجو در برنامه ریزی دوره
    /// </summary>
    public class ProductSalesPlanningSearchViewModel
    {
        /// <summary>
        /// شناسه برنامه ریزی دوره
        /// </summary>
        public int ProductSalesPlanningId { get; set; }
        /// <summary>
        /// عنوان برنامه ریزی دوره
        /// </summary>
        public string ProductSalesPlanningTitle { get; set; }

        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// عنوان مرکز
        /// </summary>
        public string CompanyTitle { get; set; }

        /// <summary>
        /// شناسه نمایندگی
        /// </summary>
        public int? RepresentationId { get; set; }

        /// <summary>
        /// عنوان نمایندگی
        /// </summary>
        public string? RepresentationTitle { get; set; }
    }

    /// <summary>
    /// مدل جستجو در مراکز
    /// </summary>
    public class CompanySearchViewModel
    {
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// عنوان مرکز
        /// </summary>
        public string Title { get; set; }
    }

    /// <summary>
    /// مدل جستجو در دسته بندی ها
    /// </summary>
    public class CategorySearchViewModel
    {
        /// <summary>
        /// شناسه دسته بندی
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// عنوان دسته بندی
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// آیا دسته بندی ریشه است؟
        /// </summary>
        public bool IsLevelOneCategory { get; set; }
    }


    /// <summary>
    /// مدل اطلاعات حضور غیاب دانشجو
    /// </summary>
    public class StudentRollCallViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// عنوان کلاس
        /// </summary>
        public string VirtualClassTitle { get; set; }

        /// <summary>
        /// شناسه کلاس
        /// </summary>
        public int VirtualClassId { get; set; }

        /// <summary>
        /// آیا در دانشجو در کلاس حضور داشته؟
        /// </summary>
        public bool IsPresent { get; set; }
    }

}
