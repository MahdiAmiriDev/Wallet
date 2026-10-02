using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class HekmatCanceledTransactionListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string CartNumber { get; set; }

        public int? RecieverCode { get; set; }

        [StringValidation] public string StoreName { get; set; }

        public int? TerminalNumber { get; set; }

        public Int64? SendTransactionPrice { get; set; }

        [StringValidation] public string TransactionDate { get; set; }

        [StringValidation] public string ProcessDate { get; set; }

        [StringValidation] public string CancleDate { get; set; }

        public Int64? FullAnnouncedInstallmentPrice { get; set; }

        public Int64? RefundPrice { get; set; }

        public int? RefundDetailId { get; set; }
    }


    public class HekmatCanceledTransactionEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [StringLength(32)][StringValidation] public string CartNumber { get; set; }
        public int? RecieverCode { get; set; }
        [StringLength(200)][StringValidation] public string StoreName { get; set; }
        public int? TerminalNumber { get; set; }
        public Int64? SendTransactionPrice { get; set; }
        [StringLength(50)] [StringValidation] public string TransactionDate { get; set; }
        [StringLength(50)] [StringValidation] public string ProcessDate { get; set; }
        [StringLength(50)] [StringValidation] public string CancleDate { get; set; }
        public Int64? FullAnnouncedInstallmentPrice { get; set; }
        public Int64? RefundPrice { get; set; }
        public int? RefundDetailId { get; set; }
    }


    public class HekmatCanceledTransactionSimpleViewModel : IBaseViewModel
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