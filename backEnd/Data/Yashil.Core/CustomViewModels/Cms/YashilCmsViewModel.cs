using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.CustomViewModels.Cms
{
    public class YashilCmsListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string Title { get; set; }

        [StringValidation] public string SubTitle { get; set; }

        [StringValidation] public string PagePositionTitle { get; set; }

        public int DisplayOrder { get; set; }
        /// <summary>
        /// موقعیت در صفحه
        /// </summary>
        public int PagePositionType { get; set; }
    }
    public class YashilCmsSimpleListViewModel
    {
        public int Id { get; set; }
        [StringValidation] public string Title { get; set; }
        [StringValidation] public string SubTitle { get; set; }
        [StringValidation] public string Url { get; set; }
        public string PagePositionTitle { get; set; }
    }
    public class YashilCmsEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringLength(300)]
        [StringValidation]
        public string Code { get; set; }

        [StringLength(600)]
        [Required]
        [StringValidation]
        public string Title { get; set; }

        public string SubTitle { get; set; }

        [Range(0, int.MaxValue)]
        [Required] public int PagePositionType { get; set; }

        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }

        [MaxLength(300)]
        [StringValidation]
        public string Url { get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// تصویر قابل برش است؟
        /// </summary>
        public bool? CroppableImage { get; set; }
        /// <summary>
        /// حداکثر ارتفاع
        /// </summary>
        public int? MaxHeight { get; set; }
        /// <summary>
        /// حداقل ارتفاع
        /// </summary>
        public int? MaxWidth { get; set; }

        /// <summary>
        /// حداکثر ارتفاع حالت موبایل
        /// </summary>
        public int? MobileMaxHeight { get; set; }
        /// <summary>
        /// حداکثر عرض حالت موبایل
        /// </summary>
        public int? MobileMaxWidth { get; set; }
    }
    public class YashilCmsCustomViewModel
    {
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

        [StringValidation] public string SubTitle { get; set; }
        [StringValidation] public string ModificationDate { get; set; }
        /// <summary>
        /// آدرس url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int DisplayOrder { get; set; }

    }
    public class YashilCmsSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
        [StringValidation] public string Url { get; set; }

    }
}
