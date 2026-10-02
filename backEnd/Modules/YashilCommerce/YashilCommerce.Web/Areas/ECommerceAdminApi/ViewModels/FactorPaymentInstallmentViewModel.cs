using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;
namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class FactorPaymentInstallmentListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }

        public int Id { get; set; }

        public string ParentTitle { get; set; }

        public string Code { get; set; }

        public string FactorGroupItemTitle { get; set; }

        public decimal SumAmountPaymentToSupplier { get; set; }

        public int FactorInstallmentCount { get; set; }

        public int DueDateYearMonthInt { get; set; }

        public int InstallmentNumber { get; set; }

        public decimal? RemainingPaymentToSupplier { get; set; }

        public decimal InstallmentAmount { get; set; }

        public decimal? RefundAmount { get; set; }

        public decimal? PaidAmount { get; set; }

        public string InstallmentPaymentStatusTypeTitle { get; set; }

        public string CodePath { get; set; }

        public string AccessLevelTitle { get; set; }

    }


    public class FactorPaymentInstallmentEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        public int? ParentId { get; set; }

        [StringLength(300)]
        public string Code { get; set; }

        [Range(0, int.MaxValue)]

        [Required]

        public int FactorGroupItemId { get; set; }

        [Required]

        public decimal SumAmountPaymentToSupplier { get; set; }

        [Range(0, int.MaxValue)]

        [Required]

        public int FactorInstallmentCount { get; set; }

        [Range(0, int.MaxValue)]

        [Required]

        public int DueDateYearMonthInt { get; set; }

        [Range(0, int.MaxValue)]

        [Required]

        public int InstallmentNumber { get; set; }

        public decimal? RemainingPaymentToSupplier { get; set; }

        [Required]

        public decimal InstallmentAmount { get; set; }

        public decimal? RefundAmount { get; set; }

        public decimal? PaidAmount { get; set; }

        public int? InstallmentPaymentStatusType { get; set; }

        [StringLength(800)]
        public string CodePath { get; set; }

        [Range(0, int.MaxValue)]

        [Required]

        public int AccessLevelId { get; set; }
        /// <summary>
        ///         /// شناسه مرجع پرداخت
        ///</summary>
        public string PaymentReferenceId { get; set; }
        /// <summary>
        ///         /// زمان ثبت شناسه مرجع پرداخت
        ///</summary>
        public string PaymentReferencePersianDateTime { get; set; }
    }

    public class FactorPaymentInstallmentSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        /// <summary>
        /// نام دسته بندی
        /// </summary>
        public string CategoryGroupTitle { get; set; }
        /// <summary>
        /// نام مرکز
        /// </summary>
        public string CompanyOrRepresentationTitle { get; set; }
        /// <summary>
        /// کد نمایندگی مرکز
        /// </summary>
        public string CompanyOrRepresentationCode { get; set; }
        /// <summary>
        /// مبلغ قسط
        /// </summary>
        public string InstallmentAmount { get; set; }
        /// <summary>
        /// شماره قسط
        /// </summary>
        public string InstallmentNumber { get; set; }
    }

}
