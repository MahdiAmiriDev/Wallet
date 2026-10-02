using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class CustomerClubPaymentInstallmentListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string PaymentTitle { get; set; }

        public decimal InstallmentAmount { get; set; }

        [StringValidation] public string YearMonthTitle { get; set; }

        public int InstallmentNumber { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

        [StringValidation] public string DemandGroupTitle { get; set; }
    }


    public class CustomerClubPaymentInstallmentEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)] [Required] public int PaymentId { get; set; }

        [Required] public decimal InstallmentAmount { get; set; }
        [Range(0, int.MaxValue)] [Required] public int YearMonth { get; set; }
        [Range(0, int.MaxValue)] [Required] public int InstallmentNumber { get; set; }
        [Range(0, int.MaxValue)] [Required] public int AccessLevelId { get; set; }
        public int? DemandGroupId { get; set; }
    }


    public class CustomerClubPaymentInstallmentSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
    }

    /// <summary>
    /// جهت نمایش لیست مطالبات
    /// </summary>
    public class CustomerClubPaymentInstallmentGroupItemListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }
        /// <summary>
        ///         /// مبلغ قسط
        ///</summary>
        public decimal InstallmentAmount { get; set; }
        /// <summary>
        /// شماره گردش
        /// </summary>
        [StringValidation] public string ReservationNumber { get; set; }
        /// <summary>
        /// تاریخ تایید
        /// </summary>
        public int PaidDateInt { get; set; }
        /// <summary>
        /// شماره کارت
        /// </summary>
        [StringValidation] public string CardNumber { get; set; }
        /// <summary>
        /// کد ملی صاحب حکمت
        /// </summary>
        [CanBeNull]
        [StringValidation] public string CustomerClubCardCode { get; set; }
        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public int? InstallmentCount { get; set; }
        /// <summary>
        /// شماره قسط
        /// </summary>
        public int InstallmentNumber { get; set; }
        /// <summary>
        /// نام طرح
        /// </summary>
        [StringValidation] public string ApplicationTitle { get; set; }
        /// <summary>
        /// سال
        /// </summary>
        public int YearNumber { get; set; }
        [StringValidation] public string MonthTitle { get; set; }

    }
}