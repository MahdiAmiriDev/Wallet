using Yashil.Common.Core.Attributes;

namespace Yashil.Core.CustomViewModels
{
    public class DescriptionEditModel
    {
        public int Id { get; set; }
        [StringValidation] public string Description { get; set; }
        [StringValidation] public string PropertyName { get; set; }
    }
}