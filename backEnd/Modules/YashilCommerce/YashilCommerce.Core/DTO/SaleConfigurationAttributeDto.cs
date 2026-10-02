using JetBrains.Annotations;

namespace YashilCommerce.Core.DTO
{
    public class SaleConfigurationAttributeDto
    {
        public int SaleConfigurationId { get; set; }

        public int DiscountType { get; set; }
        /// <summary>
        ///         /// استفاده از درصد برای تخفیف
        ///</summary>
        public bool UsePercentage { get; set; }
        /// <summary>
        ///         /// درصد تخفیف
        ///</summary>
        public decimal? DiscountPercentage { get; set; }
        /// <summary>
        ///         /// مقدار تخفیف
        ///</summary>
        public decimal? DiscountAmount { get; set; }
        /// <summary>
        ///         /// درصد سود از تامین کننده
        ///</summary>
        public decimal? ProfitPercentage { get; set; }
        /// <summary>
        ///         /// سایر کسورات
        ///</summary>
        public decimal? OtherDeductionsPercentage { get; set; }
        /// <summary>
        /// درصد کارمزد
        /// </summary>
        public decimal? WagePercentage { get; set; }
        /// <summary>
        ///         /// شناسه باشگاه مشتریان
        ///</summary>
        public int CustomerClubId { get; set; }

        #region SaleConfiguration Attributes

        /// <summary>
        ///         /// فروش اقساطی
        ///</summary>
        public bool AllowedToPayInInstallment { get; set; }
        /// <summary>
        ///         /// فرمول محاسبه قسط
        ///</summary>
        [CanBeNull]
        public string InstallmentExpression { get; set; }

        public int InstallmentCount { get; set; }

        public bool ExpressionBasedInstallmentCount { get; set; }
        public int? RepresentationId { get; set; }
        public int CompanyId { get; set; }
        /// <summary>
        ///درصد کسورات بیمه
        ///</summary>
        public decimal InsurancePercentage { get; set; }
        /// <summary>
        /// نوع فروش
        /// </summary>
        public int? SaleType { get; set; }

        public int? SaleConfigurationAttributeId { get; set; }

        #endregion
    }
}
