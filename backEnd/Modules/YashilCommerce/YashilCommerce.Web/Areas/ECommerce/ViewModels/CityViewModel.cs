using Yashil.Common.Core.Attributes;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class CityViewModel
    {
        public int Id { get; set; }
        [StringValidation] public string Title { get; set; }
        public int ParentId { get; set; }
    }
}
