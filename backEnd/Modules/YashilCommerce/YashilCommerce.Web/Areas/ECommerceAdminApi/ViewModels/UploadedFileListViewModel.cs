using System;
using Yashil.Common.Core.Attributes;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class UploadedFileListViewModel
    {
        /// <summary>
        ///         /// کد
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///         /// عنوان
        ///</summary>
        [StringValidation] public string? Title { get; set; }
        /// <summary>
        ///نام واقعی سند
        ///</summary>
        [StringValidation] public string? OrginalName { get; set; }
        /// <summary>
        ///         /// نوع داده
        ///</summary>
        [StringValidation] public string ContentType { get; set; }
        /// <summary>
        ///         /// ایجاد کننده
        ///</summary>
        public int CreateByName { get; set; }

        /// <summary>
        ///  نام و نام خانوادگی کاربر ایجاد کننده
        ///</summary>
        [StringValidation] public string CreateByFullName { get; set; }
        /// <summary>
        /// نام مرکز
        ///</summary>
        [StringValidation] public string CompanyTitle { get; set; }
        /// <summary>
        /// تاریخ درج
        /// </summary>
        public DateTime CreationDate { get; set; }

    }
}
