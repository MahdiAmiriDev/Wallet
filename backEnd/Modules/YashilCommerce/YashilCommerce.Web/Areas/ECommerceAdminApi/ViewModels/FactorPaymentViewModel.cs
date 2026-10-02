using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class FactorPaymentListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int? ParentId { get; set; }
        public int Id { get; set; }
        /// <summary>
        /// تاریخ پرداخت
        /// </summary>
        public int PaymentDate { get; set; }
        /// <summary>
        /// قیمت
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public int InstallmentCount { get; set; }
    }

    public class FactorPaymentEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [Range(0, int.MaxValue)] [Required] public int FactorId { get; set; }
        /// <summary>
        /// تاریخ پرداخت
        /// </summary>
        [Range(0, int.MaxValue)] [Required] public int PaymentDate { get; set; }
        /// <summary>
        /// قیمت
        /// </summary>
        [Range(0, Double.MaxValue)] [Required] public decimal Price { get; set; }
        /// <summary>
        /// تعداد اقساط
        /// </summary>
        [Range(0,int.MaxValue)] [Required] public int InstallmentCount { get; set; }
    }

    public class FactorPaymentSimpleViewModel : IBaseViewModel
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