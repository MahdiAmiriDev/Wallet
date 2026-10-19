using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{

    public class MerchantTerminalInfoListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string? Code { get; set; }

        [StringValidation] public string? Title { get; set; }

        [StringValidation] public string? AccessLevelTitle { get; set; }

        [StringValidation] public string MerchantTitle { get; set; }

        [StringValidation] public string Ip { get; set; }

        public int Port { get; set; }
        [StringValidation] public string? Url { get; set; }

    }


    public class MerchantTerminalInfoEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string? Code { get; set; }
        [StringLength(600)]
        [StringValidation]
        public string? Title { get; set; }
        [StringValidation] public string? Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int MerchantId { get; set; }

        [Required] [StringValidation] public string PublicKey { get; set; }
        [StringLength(30)]
        [StringValidation]
        [Required]
        public string Ip { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int Port { get; set; }
        [StringValidation] public string? Url { get; set; }
    }





    public class MerchantTerminalInfoSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string? Title { get; set; }

    }

}
