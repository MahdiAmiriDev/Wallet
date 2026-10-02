using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Yashil.Common.Core.Attributes;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels.ResumeModels
{
    /// <summary>
    /// مدل مربوط به دریافت فایل مدارک برای رزومه کاربر
    /// </summary>
    public class ResumeAttachment 
    {
        /// <summary>
        /// عنوان سند
        /// </summary>
        [StringLength(600)]
        [StringValidation] public string Title { get; set; }
        /// <summary>
        /// شناسه نوع سند
        /// </summary>
        public int DocTypeId { get; set; }

        /// <summary>
        /// فایل ضمیمه
        /// </summary>
        public List<IFormFile> Attachments { get; set; }
     




    }
}
