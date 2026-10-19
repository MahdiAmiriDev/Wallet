using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{

    public class CardAmountHistoryListViewModel : IBaseViewModel
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

        [StringValidation] public string AccessLevelTitle { get; set; }

        public int CardInfoId { get; set; }

        public bool CreditOrCash { get; set; }

        [StringValidation] public string CreditOrCashString { get { if (CreditOrCash) { return "اعتباری"; } else { return "ریالی"; } } }

        public int CurrentAmount { get; set; }

        public int NewAmount { get; set; }

        public int? Amount { get; private set; }

        [StringValidation] public string CardNumber { get; set; }

        [StringValidation] public string OwnerName { get; set; }

        [StringValidation] public string CardType { get; set; }

        [StringValidation] public string OrganizationTitle { get; set; }

        public DateTime CreationDate { get; set; }

        [StringValidation] public string NationalCode { get; set; }

    }


    public class CardAmountHistoryEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(600)] [StringValidation] public string? Title { get; set; }
        [StringValidation] public string? Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int CardInfoId { get; set; }
        [Required] public bool CreditOrCash { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int CurrentAmount { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int NewAmount { get; set; }

        public int? Amount { get; private set; }
    }





    public class CardAmountHistorySimpleViewModel : IBaseViewModel
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
