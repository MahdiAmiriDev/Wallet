using System.Collections.Generic;
using Yashil.Common.Core.Attributes;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    /// <summary>
    /// مدل مورد استفاده در پاسخ ایجاد رزمه یا بروزرسانی آن
    /// </summary>
    public class ResumeResponseModel
    {
        public ResumeResponseModel(bool isSuccess, List<string> message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public ResumeResponseModel()
        {

        }

        public bool IsSuccess { get; set; }
        public int EntityId { get; set; }
        public List<string> Message { get; set; }
    }

    /// <summary>
    /// مدل مربوط به نمایش رزومه به کاربر
    /// </summary>
    public class ResumeEditModel
    {
        public int Id { get; set; }
        /// <summary>
        /// رزئمه کاربر به صورت Json
        /// </summary>
        [StringValidation]
        public string UserResume { get; set; }
    }
}
