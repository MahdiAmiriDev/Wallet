using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class SubsidyListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

        public Int64 VezaratCeiling { get; set; }

        public Int64 SataCeiling { get; set; }

        /// <summary>
        /// سقف مبلغ یارانه وزارت به ازای هر شماره حکمت
        ///</summary>
        public int VezaratCeilingPerHekmatCard { get; set; }
        /// <summary>
        /// سقف مبلغ یارانه ساتا به ازای هر شماره حکمت
        ///</summary>
        public int SataCeilingPerHekmatCard { get; set; }

        /// <summary>
        /// سقف تعداد یارانه وزارت به ازای هر شماره حکمت
        ///</summary>
        public int VezaratSubsidyMaxCountPerHekmatCard { get; set; }

        /// <summary>
        /// سقف تعداد یارانه ساتا به ازای هر شماره حکمت
        ///</summary>
        public int SataSubsidyMaxCountPerHekmatCard { get; set; }

        public decimal VezaratProfitPercentage { get; set; }

        public decimal SataProfitPercentage { get; set; }

        public int Year { get; set; }

        public int UsedInVezarat { get; set; }

        public int UsedInSata { get; set; }

        public bool IsActive { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }
        /// <summary>
        /// یارانه وزارت فعال است
        /// </summary>
        public bool? IsVezaratActive { get; set; }
        /// <summary>
        /// یارانه ساتا فعال است
        /// </summary>
        public bool? IsSataActive { get; set; }
        /// <summary>
        /// شناسه گروه دسته بندی
        /// </summary>
        public int CategoryGroupId { get; set; }
        /// <summary>
        /// شناسه گروه یارانه
        ///</summary>
        public int SubsidyGroupId { get; set; }
        /// <summary>
        ///  گروه یارانه
        /// </summary>
        public string SubsidyGroupTitle { get; set; }

        /// <summary>
        /// شناسه گروه دسته بندی
        /// </summary>
        public string CategoryGroupTitle { get; set; }
        /// <summary>
        /// آیا ضرایب اعمال بشود یا نه
        /// </summary>
        public bool ApplySubsidyFactor { get; set; }
    }


    public class SubsidyEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(600)] [StringValidation]
        [Required]
        public string Title { get; set; }
        [Range(0, Int64.MaxValue)]
        [Required] public Int64 VezaratCeiling { get; set; }
        [Range(0, Int64.MaxValue)]
        [Required] public Int64 SataCeiling { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int VezaratCeilingPerHekmatCard { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int SataCeilingPerHekmatCard { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int VezaratSubsidyMaxCountPerHekmatCard { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int SataSubsidyMaxCountPerHekmatCard { get; set; }

        [Required] public decimal VezaratProfitPercentage { get; set; }

        [Required] public decimal SataProfitPercentage { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int Year { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int UsedInVezarat { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int UsedInSata { get; set; }
        public bool IsActive { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }

        /// <summary>
        /// یارانه وزارت فعال است
        /// </summary>
        public bool? IsVezaratActive { get; set; }
        /// <summary>
        /// یارانه ساتا فعال است
        /// </summary>
        public bool? IsSataActive { get; set; }
        /// <summary>
        /// شناسه گروه دسته بندی
        /// </summary>
        public int CategoryGroupId { get; set; }

        /// <summary>
        /// شناسه گروه یارانه
        /// </summary>
        public int SubsidyGroupId { get; set; }
        /// <summary>
        /// آیا ضرایب اعمال بشود یا نه
        /// </summary>
        public bool ApplySubsidyFactor { get; set; }

    }
    public class SubsidySimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

    }

}
