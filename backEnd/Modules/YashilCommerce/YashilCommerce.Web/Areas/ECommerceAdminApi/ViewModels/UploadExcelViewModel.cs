using Yashil.Common.Core.Attributes;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class UploadExcelViewModel
    {
        [StringValidation] public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
    public class ProductFileModel
    {
        [StringValidation] public string FileName { get; set; }
    }
}
