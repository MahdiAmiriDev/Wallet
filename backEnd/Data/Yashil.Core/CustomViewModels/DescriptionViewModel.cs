using Yashil.Common.Core.Attributes;

namespace Yashil.Core.CustomViewModels
{
    public class DescriptionViewModel
    {
        public int Id { get; set; }
        [StringValidation] public string Description { get; set; }
    }
}
