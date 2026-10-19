using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;
namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{

    public class PaymentGatewayListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        public string? Code { get; set; }

        public string? Title { get; set; }

        public string? AccessLevelTitle { get; set; }

        public bool IsActive { get; set; }

    }


    public class PaymentGatewayEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)] public string? Code { get; set; }
        [StringLength(600)]
        [Required] public string? Title { get; set; }
        public string? Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
        public bool IsActive { get; set; }
    }





    public class PaymentGatewaySimpleViewModel : IBaseViewModel
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
