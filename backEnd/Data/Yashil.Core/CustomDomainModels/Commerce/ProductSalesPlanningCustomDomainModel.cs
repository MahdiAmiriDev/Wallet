
using System;
using System.Collections.Generic;
using Yashil.Core.CustomDomainModels.YashilLight;

namespace Yashil.Core.CustomDomainModels.Commerce
{

    public class ProductSalesPlanningCustomDomainModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// نامی که در کار رنگ نشان داده میشود
        /// </summary>
        public string CssClass { get; set; }
        /// <summary>
        /// تعداد سرفصل ها
        /// یعنی تعداد media های زیر مجموعه مدیا اصلی
        /// </summary>
        public int ChapterCount { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        /// <summary>
        ///         /// ترتیب نمایش
        ///</summary>
        public int? DisplayOrder { get; set; }
        /// <summary>
        ///         /// تاریخ شروع شمسی 
        ///</summary>
        public int? StartDateInt { get; set; }
        /// <summary>
        ///         /// تاریخ پایان شمسی
        ///</summary>
        public int? EndDateInt { get; set; }
        /// <summary>
        ///         /// زمان ایجاد
        ///</summary>
        public DateTime CreationDate { get; set; }

        public string CourseStatusTitle { get; set; }
        public int? CourseStatusId { get; set; }
        public DateTime? ModificationDate { get; set; }
        /// <summary>
        /// مدت زمان دوره
        /// </summary>
        public int? Duration { get; set; }
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// عنوان مرکز
        /// </summary>
        public string CompanyTitle { get; set; }
        /// <summary>
        /// ترتیب نمایش مرکز
        /// </summary>
        public int CompanyDisplayOrder { get; set; }

        /// <summary>
        /// آیا دوره عمومی است؟
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// شناسه نمایندگی ها
        /// </summary>
        public int? RepresentationId { get; set; }

    }
    public class ProductSalesPlanningTeacherCustomDomainModel : ProductSalesPlanningCustomDomainModel
    {
        public IEnumerable<TeacherCustomDomainModel> Teachers { get; set; }
    }


}
