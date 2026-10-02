using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Enums;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class FactorGroupListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        public int? ParentId { get; set; }
        [StringValidation] public string Code { get; set; }

        [StringValidation] public string Title { get; set; }

        [StringValidation] public string ParentTitle { get; set; }
        public int ChildsCount { get; set; }

        /// <summary>
        /// وضعیت گروه بندی فاکتور
        /// </summary>
        public int FactorGroupStatus { get; set; }

        /// <summary>
        /// تعداد آیتمهای گروه بندی فاکتور (فاکتور)
        /// </summary>
        public int FactorGroupItemCount { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }

        /// <summary>
        /// showFactorGroupItemBtn
        /// </summary>
        public bool AllowModifyFactorGroupItems
        {
            get
            {

                if (ChildsCount > 0 || FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Confirmed)
                {
                    return false;
                }
                else if (FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Initial && FactorGroupItemCount >= 0)
                {
                    return true;
                }
                return false;


            }
        }
        /// <summary>
        /// showConfirmFactorGroupBtn
        /// </summary>
        public bool AllowConfirmFactorGroupBtn
        {
            get
            {
                if (ChildsCount > 0 || FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Confirmed)
                {
                    return false;
                }
                else if (FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Initial && FactorGroupItemCount > 0)
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// showReportFactorGroupBtn
        /// </summary>
        public bool allowGenerateFactorReport
        {
            get
            {
                if (FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Confirmed)
                {
                    return true;
                }
                return false;
            }
        }
    }

    public class FactorGroupEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(600)] [StringValidation]
        [Required]
        public string Title { get; set; }
        public int? ParentId { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int FactorGroupStatus { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int AccessLevelId { get; set; }
    }


    public class FactorGroupSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

        public int ParentId { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
    }
}
