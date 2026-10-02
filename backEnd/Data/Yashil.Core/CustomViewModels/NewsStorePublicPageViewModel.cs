using System;
using System.Collections.Generic;

namespace Yashil.Core.CustomViewModels
{
    /// <summary>
    /// این ویو مدل جهت نمایش اخبار و اطلاعیه های موجود در صفحه ی هوم کاربرد دارد
    /// </summary>
    public class NewsStorePublicPageViewModel  
    {
        /// <summary>
        /// شناسه خبر
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// عنوان خبر
        /// </summary>
         public string Title { get; set; }
        /// <summary>
        /// خلاصه خبر
        /// </summary>
        public string Led { get; set; }
        /// <summary>
         /// نوع خبر
        ///</summary>
        public int NewsType { get; set; }
        /// <summary>
        /// زمان تغییر
        ///</summary>
        public string? ModificationDate { get; set; }
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// عنوان مرکز
        /// </summary>
        public string CompanyTitle { get; set; }

        /// <summary>
        /// تاریخ ایجاد
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// عنوان دسته بندی خبر
        /// </summary>
        public string ServiceTitle { get; set; }
        /// <summary>
        /// تاریخ انتشار خبر
        /// </summary>
        public int? EffluenceDateInt { get; set; }
        /// <summary>
        /// تعداد بازدید از این خبر
        ///</summary>
        public int? ViewCount { get; set; }
    }

    /// <summary>
    /// مدل مربوط به نمایش اخبار و اطلاعیه ها به همراه
    /// اطلاعات مرکز
    /// </summary>
    public class CompanyNewsStoreViewModel
    {
        public List<NewsStorePublicPageViewModel> NewsStoreModel { get; set; }

        public List<CompanyInformationModel> CompanyInformationModel { get; set; }

    }

    /// <summary>
    /// مدل مربوط به اطلاعات مرکز
    /// </summary>
    public class CompanyInformationModel
    {
        public int? CompanyId { get; set; }
        public string CompanyTitle { get; set; }
    }
}
