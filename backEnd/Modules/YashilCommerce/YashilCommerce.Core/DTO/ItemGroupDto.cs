using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Core.DTO
{
    /// <summary>
    /// آیتم های گروه
    /// </summary>
    public class ItemGroupDto : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

        public int ParentId { get; set; }
    }
}
