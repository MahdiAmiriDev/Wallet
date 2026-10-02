
using Yashil.Common.Core.Attributes;

namespace YashilLight.Core.Dto
{
    public class MediaSessionDto
    {
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string Title { get; set; }

        public int? ProductSalesPlanningId { get; set; }

        public string Description { get; set; }
    }
}
