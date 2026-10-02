namespace YashilCommerce.Core.DTO
{
    public class SaleConfigurationPriceParametersDto
    {
        /// <summary>
        /// درصد تخفیف
        /// </summary>
        public decimal DiscountPercentage { get; set; }
        /// <summary>
        /// درصد سود
        /// </summary>
        public decimal ProfitPercentage { get; set; }
        /// <summary>
        /// درصد سایر کسورات - هزینه تبلیغات
        /// </summary>

        public decimal? OtherDeductionsPercentage { get; set; }
        /// <summary>
        /// درصد کارمزد باشگاه مشتریان کاربر
        /// </summary>
        public decimal WagePercentage { get; set; }
        /// <summary>
        /// آیا تنظیم فروش دارای ویژگی تنظیم فروش می باشد
        /// </summary>
        public bool SaleConfigurationHaveAttribute { get; set; }
    }
}
