using Yashil.Common.Core.Attributes;

namespace YashilLight.Core.Dto
{
    public class MediaSessionProductInfoDto
    {
        public int Id { get; set; }
        [StringValidation] public string ProductSalesPlanningTitle { get; set; }
    }
}
