using JetBrains.Annotations;

namespace YashilCommerce.Core.DTO
{
    public class ItemPaymentSummaryDto
    {
        public int? Id { get; set; }
        /// <summary>
        /// روش پرداخت
        /// </summary>
        [CanBeNull]
        public string PaymentMethodType { get; set; }
        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public int? InstallmentCount { get; set; }
        /// <summary>
        /// مبلغ پرداخت
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// مجموع مبلغ پرداخت
        /// </summary>
        public decimal? FactorItemPaymentTotal { get; set; }
        /// <summary>
        /// درصد پرداخت
        /// </summary>
        public decimal? TotalPricePercentage { get; set; }
    }
}
