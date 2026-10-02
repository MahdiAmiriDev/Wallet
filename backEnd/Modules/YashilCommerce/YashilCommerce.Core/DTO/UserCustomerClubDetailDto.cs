namespace YashilCommerce.Core.DTO
{
    public class UserCustomerClubDetailDto
    {
        public int CustomerClubId { get; set; }
        /// <summary>
        /// آیا باشگاه مشتریان براساس آدرس می باشد 
        /// </summary>
        public bool AddressBased { get; set; }
        /// <summary>
        /// مدل ثبت نام باشگاه مشتریان
        /// Minimal = 2302  : take minimal input fields
        /// Medium = 2303 : take some of them
        /// Full = 2304 :  take all of them
        /// </summary>
        public int RegistrationModeType { get; set; }
        /// <summary>
        /// آیا باشگاه مشتریان دارای یارانه می باشد 
        /// </summary>
        public bool SubsidyBased { get; set; }
        /// <summary>
        /// آیا باشگاه مشتریان دارای درگاه پرداخت حکمت می باشد  
        /// </summary>
        public bool HekmatBased { get; set; }
        /// <summary>
        /// نوع پرداخت
        /// پرداخت نقدی : 1104
        /// پرداخت اقساطی : 1105
        /// نقدی و اقساطی : 2305
        /// </summary>
        public int SaleType { get; set; }
        /// <summary>
        /// درصد تخفیف باشگاه مشتریان کاربر
        /// </summary>
        public decimal DiscountPercentage { get; set; }
        /// <summary>
        /// درصد سود باشگاه مشتریان کاربر
        /// </summary>
        public decimal ProfitPercentage { get; set; }
        /// <summary>
        /// درصد سایر کسورات باشگاه مشتریان کاربر
        /// </summary>
        public decimal? OtherDeductionsPercentage { get; set; }
        /// <summary>
        /// درصد کارمزد باشگاه مشتریان کاربر
        /// </summary>
        public decimal WagePercentage { get; set; }
        /// <summary>
        /// قیمت با تخفیف
        /// </summary>
        public decimal? CalcedPrice { get; set; }
        /// <summary>
        /// مبلغ تخفیف
        /// </summary>
        public decimal? DiscountAmount { get; set; }

        public bool ExistsSaleConfigurationAttribute { get; set; }

    }
}
