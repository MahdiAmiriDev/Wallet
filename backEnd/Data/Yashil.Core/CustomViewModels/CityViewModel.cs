using Yashil.Common.Core.Attributes;

namespace Yashil.Core.CustomViewModels
{

    public class CityTitleOnlyViewModel
    {
        /// <summary>
        /// City Id 
        /// </summary>
        [StringValidation] public string Data { get; set; }
        /// <summary>
        /// City Title
        /// </summary>
        [StringValidation] public string Value { get; set; }
    }
}
