using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{

    public class CardInfoListViewModel : IBaseViewModel
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

        [StringValidation] public string AccessLevelTitle { get; set; }

        [StringValidation] public string CardNumber { get; set; }

        [StringValidation] public string? Cvv2 { get; set; }

        [StringValidation] public string FirstPassword { get; set; }

        [StringValidation] public string? SecondPassword { get; set; }

        public int ExpireDate { get; set; }

        public int CreditAmount { get; set; }

        public int CashAmount { get; set; }

        [StringValidation] public string CardTypeTitle { get; set; }

        [StringValidation] public string PersonTitle { get; set; }

        [StringValidation] public string OrganizationTitle { get; set; }

        [StringValidation] public string NationalCode { get; set; }
    }


    public class CardInfoEditModel : IBaseViewModel
    {
        private int? _expireDate;
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string? Code { get; set; }
        [StringLength(600)] [StringValidation] public string? Title { get; set; }
        [StringValidation] public string? Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] 
        public int AccessLevelId { get; set; }
        [StringLength(32)][StringValidation]
        [Required]
        public string? CardNumber { get; set; }

        [StringLength(8)]
        [StringValidation]
        public string? Cvv2 { get; set; }

        [StringLength(8)] [Required] [StringValidation] public string? FirstPassword { get; set; }

        [StringLength(24)] [StringValidation] public string? SecondPassword { get; set; }
        
        public int? ExpireDate { get { if (ExpireDateYear == null || ExpireDateMonth == null) { _expireDate = null; } else { _expireDate = Convert.ToInt32(ExpireDateYear + ExpireDateMonth); } return _expireDate; } set { _expireDate = value; ExpireDateYear = _expireDate == null ? null : _expireDate.ToString().Substring(0,4); ExpireDateMonth = _expireDate == null ? null : _expireDate.ToString().Substring(4, 2); } }
        [StringLength(2)]
        [StringValidation] public string? ExpireDateMonth { get; set; }
        [StringLength(4)]
        [StringValidation] public string? ExpireDateYear { get; set; }
        public int CreditAmount { get; set; }
        public int CashAmount { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int CardType { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int PersonId { get; set; }
    }





    public class CardInfoSimpleViewModel : IBaseViewModel
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
