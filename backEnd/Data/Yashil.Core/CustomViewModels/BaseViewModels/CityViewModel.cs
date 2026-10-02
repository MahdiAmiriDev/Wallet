using Yashil.Common.Core.Attributes;

namespace Yashil.Core.CustomViewModels.BaseViewModels
{
    public class CityViewModel
    {
        public int Id { get; set; }
    
        public int? ParentId { get; set; }
  
 
        /// <summary>
        ///         /// مرکز استان
        ///</summary>
        public bool? ProvinceCenter { get; set; }
        /// <summary>
        ///         /// عنوان
        ///</summary>
        [StringValidation] public string Title { get; set; }

        [StringValidation] public string FullTitle { get; set; }

    }
}
