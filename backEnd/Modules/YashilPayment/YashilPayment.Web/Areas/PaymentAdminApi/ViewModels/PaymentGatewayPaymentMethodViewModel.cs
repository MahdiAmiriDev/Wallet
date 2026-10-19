using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;
namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{

    public class PaymentGatewayPaymentMethodListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        public string? Code { get; set; }

        public string? PaymentGatewayTitle { get; set; }

        public string? PaymentMethodTypeTitle { get; set; }

        public string? AccessLevelTitle { get; set; }

    }


    public class PaymentGatewayPaymentMethodEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)] public string? Code { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int PaymentGatewayId { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int PaymentMethodType { get; set; }
        public string? Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
    }





    public class PaymentGatewayPaymentMethodSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        public string? Title { get; set; }
    }

}
