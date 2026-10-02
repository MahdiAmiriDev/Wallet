using Yashil.Common.Core.Attributes;

namespace YashilLight.Core.Dto
{
    public class ProductSalesPlanningDto
    {
        public int Id { get; set; }
        [StringValidation] public string ProductTitle { get; set; }
        public int? StartDate { get; set; }

        public int? EndDate { get; set; }

        public string CompanyTitle { get; set;}

        public string RepresentationTitle { get; set; }
    }
}
